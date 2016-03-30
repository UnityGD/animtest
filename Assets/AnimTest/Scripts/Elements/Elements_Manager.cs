using UnityEngine;
using System.Collections;

public class Elements_Manager : MonoBehaviour
{
    public UI_Manager uiManager;
    public PF_Manager pfManager;

    Modes Current_Mode;
    Click_State Current_Click_State;

    void Update()
    {
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
        }
    }

    void Create_Vertex(Click_State currentState, Vector3 mousePos)
    {
        if (currentState == Click_State.Down)
        {
            GameObject vertex = Object.Instantiate(pfManager.PF_Vertex, V3toV2(mousePos), Quaternion.identity) as GameObject;
        }
    }

    void Create_Line()
    {

    }

    public void Set_Current_Mode(Modes mode)
    {
        Current_Mode = mode;
    }

    Vector2 V3toV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
}

enum Click_State
{
    IDLE, Down, Pressed, Up
}
