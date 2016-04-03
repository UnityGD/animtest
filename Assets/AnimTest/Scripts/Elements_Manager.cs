using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Elements_Manager : MonoBehaviour
{
    public UI_Manager uiManager;
    public Transform Pointer;

    public Transform Vertices;
    public Transform Lines;

    Modes Current_Mode;
    Click_State Current_Click_State;

    bool IsLineStarted = false;
    LineRenderer Last_Line;

    Transform Selected_Vertex = null;
    Line_Behaviour Selected_Line = null;

    Line_Behaviour Last_Line_Behaviour;
    Vertex_Behaviour Start_Vertex_Behaviour;

    int Current_Frame = 0;
    int Frames_Count = 2;
    float Current_Time = 0f;

    List<Vertex_Behaviour> L_Vertices = new List<Vertex_Behaviour>();

    void Update()
    {
        if ((!EventSystem.current.IsPointerOverGameObject()) || (Current_Mode == Modes.Play))
            switch (Current_Mode)
            {
                case Modes.Vertex:
                    {
                        Current_Click_State = Click_State.IDLE;
                        if (Input.GetMouseButtonDown(0))
                        {
                            Current_Click_State = Click_State.Down;
                        }

                        Create_Vertex(Current_Click_State, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        break;
                    }

                case Modes.Line:
                    {
                        Current_Click_State = Click_State.IDLE;
                        Vector3 Current_Mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Pointer.position = V3z0(Current_Mouse_Pos);

                        if (Input.GetMouseButtonDown(0))
                        {
                            Current_Click_State = Click_State.Down;
                        }

                        if (IsLineStarted)
                        {
                            Last_Line.SetPosition(1, V3z0(Current_Mouse_Pos));
                        }

                        if (Selected_Vertex != null)
                            Create_Line(Current_Click_State, Selected_Vertex.position);
                        break;
                    }

                case Modes.Drag:
                    {
                        Current_Click_State = Click_State.IDLE;
                        Vector3 Current_Mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Pointer.position = V3z0(Current_Mouse_Pos);

                        if (Input.GetKey(KeyCode.Mouse0))
                        {
                            Current_Click_State = Click_State.Pressed;
                        }

                        if (Selected_Vertex != null)
                            Drag_Vertex(Current_Click_State, Current_Mouse_Pos);
                        break;
                    }

                case Modes.Trash:
                    {
                        Current_Click_State = Click_State.IDLE;
                        Vector3 Current_Mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Pointer.position = V3z0(Current_Mouse_Pos);

                        if (Input.GetMouseButtonDown(0))
                        {
                            Current_Click_State = Click_State.Down;
                        }

                        if (Selected_Vertex != null)
                            Trash_Vertex(Current_Click_State);

                        if (Selected_Line != null)
                            Trash_Line(Current_Click_State);
                        break;
                    }

                case Modes.Play:
                    {
                        bool IsStart = false;
                        Current_Time += Time.deltaTime;

                        if (Current_Time >= 1f)
                        {
                            Current_Frame++;
                            Current_Time = 0f;
                        }

                        if (Current_Frame == Frames_Count - 1)
                        {
                            Current_Frame = 0;
                            IsStart = true;
                        }

                        uiManager.Current_Frame_Txt.text = Current_Frame.ToString();

                        if (IsStart == false)
                        {
                            for (int i = 0; i < L_Vertices.Count; i++)
                            {
                                L_Vertices[i].Apply_Animation(Current_Frame, Current_Time);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < L_Vertices.Count; i++)
                            {
                                L_Vertices[i].Vertex.Get_Current_Position(Current_Frame);
                            }
                        }

                        break;
                    }
            }
    }

    void Create_Vertex(Click_State currentState, Vector3 mousePos)
    {
        if (currentState == Click_State.Down)
        {
            GameObject vertex = Object.Instantiate(uiManager.PF_Vertex, V3toV2(mousePos), Quaternion.identity) as GameObject;
            vertex.transform.parent = Vertices;
            vertex.GetComponent<Vertex_Behaviour>().Set_Vertex();
            L_Vertices.Add(vertex.GetComponent<Vertex_Behaviour>());
        }
    }

    void Create_Line(Click_State currentState, Vector3 mousePos)
    {
        if (currentState == Click_State.Down)
        {
            if ((!IsLineStarted) && (Is_Line_May_Be_Created(Selected_Vertex.GetComponent<Vertex_Behaviour>())))
            {
                GameObject line = Object.Instantiate(uiManager.PF_Line, Vector2.zero, Quaternion.identity) as GameObject;
                line.transform.parent = Lines;
                Last_Line = line.GetComponent<LineRenderer>();
                Last_Line_Behaviour = line.GetComponent<Line_Behaviour>();
                Start_Vertex_Behaviour = Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>();
                Last_Line.SetPosition(0, V3z0(mousePos));
                Last_Line.SetPosition(1, V3z0(mousePos));
                Last_Line_Behaviour.Set_Start_Vertex(Selected_Vertex);
                Start_Vertex_Behaviour.Vertex.Add_Line(Last_Line_Behaviour);

                IsLineStarted = !IsLineStarted;
            }
            else if (IsLineStarted)
            {
                if (!Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>().Vertex.Is_Vertex_Already_Connected(Start_Vertex_Behaviour.Vertex))
                {
                    Last_Line.SetPosition(1, V3z0(mousePos));
                    Last_Line_Behaviour.Set_End_Vertex(Selected_Vertex);
                    Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>().Vertex.Add_Connected_Vertices(Start_Vertex_Behaviour.Vertex);
                    Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>().Vertex.Add_Line(Last_Line_Behaviour);
                    Start_Vertex_Behaviour.Vertex.Add_Connected_Vertices(Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>().Vertex);
                    Last_Line_Behaviour.Create_Collider();

                    IsLineStarted = !IsLineStarted;
                }
            }
        }
    }

    void Drag_Vertex(Click_State currentState, Vector3 mousePos)
    {
        if (currentState == Click_State.Pressed)
        {
            Selected_Vertex.position = V3z0(mousePos);
            Selected_Vertex.GetComponent<Vertex_Behaviour>().Vertex.Set_Frame_Position(Current_Frame, V3z0(mousePos));
            Selected_Vertex.GetComponent<Vertex_Behaviour>().Vertex.Apply_Lines_Positions();
        }
    }

    void Trash_Vertex(Click_State currentState)
    {
        if (currentState == Click_State.Down)
        {
            Selected_Vertex.GetComponent<Vertex_Behaviour>().Destroy_Vertex_Behaviour();
            L_Vertices.Remove(Selected_Vertex.GetComponent<Vertex_Behaviour>());
        }
    }

    void Trash_Line(Click_State currentState)
    {
        if (currentState == Click_State.Down)
        {
            Selected_Line.Destroy_Line();
        }
    }

    public void Set_Current_Mode(Modes mode)
    {
        if ((mode != Modes.Play) && (Current_Mode == Modes.Play))
        {
            for (int i = 0; i < L_Vertices.Count; i++)
            {
                L_Vertices[i].Apply_Animation(Current_Frame, 0f);
            }
        }

        Current_Mode = mode;
        Current_Time = 0f;

        if ((Current_Mode == Modes.Vertex) || (Current_Mode == Modes.Play))
        {
            Pointer.gameObject.SetActive(false);
        }
        else
        {
            Pointer.gameObject.SetActive(true);
        }
    }

    public void Set_Selected_Vertex(Transform vertex, bool free)
    {
        if (free == true)
        {
            if (Selected_Vertex == vertex)
            {
                Selected_Vertex = null;
            }
        }
        else
        {
            if (Selected_Vertex == null)
            {
                Selected_Vertex = vertex;
            }
        }
    }

    public void Set_Selected_Line(Line_Behaviour line, bool free)
    {
        if (free == true)
        {
            if (Selected_Line == line)
            {
                Selected_Line = null;
            }
        }
        else
        {
            if (Selected_Line == null)
            {
                Selected_Line = line;
            }
        }
    }

    public bool Is_Vertex_Selected()
    {
        if (Selected_Vertex == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool Is_Line_Selected()
    {
        if (Selected_Line == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Move_Frame(bool isForward)
    {
        if (isForward)
        {
            Current_Frame++;
            if (Current_Frame + 1 > Frames_Count)
            {
                Add_Frame();
            }
        }
        else
        {
            if (Current_Frame - 1 > -1)
            {
                Current_Frame--;
            }
        }

        for (int i = 0; i < L_Vertices.Count; i++)
        {
            L_Vertices[i].transform.position = L_Vertices[i].Vertex.Get_Current_Position(Current_Frame);
            L_Vertices[i].Vertex.Apply_Lines_Positions();
        }
    }

    void Add_Frame()
    {
        Frames_Count++;
        for (int i = 0; i < L_Vertices.Count; i++)
        {
            L_Vertices[i].Vertex.Add_Frame_Position(Frames_Count, L_Vertices[i].transform.position);
        }
    }

    bool Is_Line_May_Be_Created(Vertex_Behaviour vertex_behaviour)
    {
        int connections = 0;

        for (int i = 0; i < L_Vertices.Count; i++)
        {
            if (vertex_behaviour != L_Vertices[i])
            {
                if (vertex_behaviour.Vertex.Is_Vertex_Already_Connected(L_Vertices[i].Vertex))
                {
                    connections++;
                }
            }
        }

        if (connections == L_Vertices.Count - 1)
            return false;
        else
            return true;
    }

    public Modes Get_Current_Mode()
    {
        return Current_Mode;
    }

    public int Get_Current_Frame()
    {
        return Current_Frame;
    }

    public int Get_Frames_Count()
    {
        return Frames_Count;
    }

    public bool Get_Line_State()
    {
        return IsLineStarted;
    }

    Vector2 V3toV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    Vector3 V3z0(Vector3 v3)
    {
        return new Vector3(v3.x, v3.y, 0f);
    }
}

enum Click_State
{
    IDLE, Down, Pressed
}
