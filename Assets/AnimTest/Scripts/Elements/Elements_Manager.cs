using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Elements_Manager : MonoBehaviour
{
    public UI_Manager uiManager;
    public PF_Manager pfManager;
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


    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
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
            }
    }

    void Create_Vertex(Click_State currentState, Vector3 mousePos)
    {
        if (currentState == Click_State.Down)
        {
            GameObject vertex = Object.Instantiate(pfManager.PF_Vertex, V3toV2(mousePos), Quaternion.identity) as GameObject;
            vertex.transform.parent = Vertices;
            vertex.GetComponent<Vertex_Behaviour>().SetVertex();
        }
    }

    void Create_Line(Click_State currentState, Vector3 mousePos)
    {
        if (currentState == Click_State.Down)
        {
            if (!IsLineStarted)
            {
                GameObject line = Object.Instantiate(pfManager.PF_Line, Vector2.zero, Quaternion.identity) as GameObject;
                line.transform.parent = Lines;
                Last_Line = line.GetComponent<LineRenderer>();
                Last_Line_Behaviour = line.GetComponent<Line_Behaviour>();
                Start_Vertex_Behaviour = Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>();
                Last_Line.SetPosition(0, V3z0(mousePos));
                Last_Line.SetPosition(1, V3z0(mousePos));
                Last_Line_Behaviour.SetStartVertex(Selected_Vertex);
                Start_Vertex_Behaviour.Vertex.AddLine(Last_Line_Behaviour);

                IsLineStarted = !IsLineStarted;
            }
            else
            {
                if (!Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>().Vertex.IsVertexAlreadyConnected(Start_Vertex_Behaviour.Vertex))
                {
                    Last_Line.SetPosition(1, V3z0(mousePos));
                    Last_Line_Behaviour.SetEndVertex(Selected_Vertex);
                    Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>().Vertex.AddConnectedVertices(Start_Vertex_Behaviour.Vertex);
                    Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>().Vertex.AddLine(Last_Line_Behaviour);
                    Start_Vertex_Behaviour.Vertex.AddConnectedVertices(Selected_Vertex.gameObject.GetComponent<Vertex_Behaviour>().Vertex);
                    Last_Line_Behaviour.CreateCollider();

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
            Selected_Vertex.GetComponent<Vertex_Behaviour>().Vertex.SetCurrentFrame(0, V3z0(mousePos));
            Selected_Vertex.GetComponent<Vertex_Behaviour>().Vertex.ApplyLinesPositions();
        }
    }

    void Trash_Vertex(Click_State currentState)
    {
        if (currentState == Click_State.Down)
        {
            Selected_Vertex.GetComponent<Vertex_Behaviour>().DeleteVertex();
        }
    }

    void Trash_Line(Click_State currentState)
    {
        if (currentState == Click_State.Down)
        {
            Selected_Line.DeleteLine();
        }
    }

    public void Set_Current_Mode(Modes mode)
    {
        Current_Mode = mode;

        if (Current_Mode == Modes.Vertex)
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

    public bool IsVertexSelected()
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

    public bool IsLineSelected()
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
    IDLE, Down, Pressed, Up
}
