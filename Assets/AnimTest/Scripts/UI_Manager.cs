using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UI_Manager : MonoBehaviour
{
    public Button Vertex_Btn;
    public Button Line_Btn;
    public Button Drag_Btn;
    public Button Trash_Btn;

    public Button PlayPause_Btn;
    public Button Previous_Frame_Btn;
    public Button Next_Frame_Btn;
    public Text Current_Frame_Txt;

    public Elements_Manager elementsManager;

    public GameObject PF_Vertex;
    public GameObject PF_Line;

    Button Current_Btn;

    void Start()
    {
        Current_Btn = Vertex_Btn;

        UI_Switch_Btn(Vertex_Btn.gameObject);
        UI_Set_Mode("Vertex");
    }

    public void UI_Switch_Btn(GameObject sender)
    {
        if (!elementsManager.Get_Line_State())
        {
            Current_Btn.interactable = true;
            sender.GetComponent<Button>().interactable = false;
            Current_Btn = sender.GetComponent<Button>();
        }
    }

    public void UI_Set_Mode(string mode)
    {
        if (!elementsManager.Get_Line_State())
        {
            try
            {
                elementsManager.Set_Current_Mode((Modes)Enum.Parse(typeof(Modes), mode));
            }
            catch
            {
                Debug.Log("'" + mode + "' mode not found!");
            }
        }
    }

    public void UI_Select_Frame(bool IsForward)
    {
        if (!elementsManager.Get_Line_State())
        {
            elementsManager.Move_Frame(IsForward);
            Current_Frame_Txt.text = elementsManager.Get_Current_Frame().ToString();
        }
    }
}

public enum Modes
{
    Vertex, Line, Drag, Trash, Play
}