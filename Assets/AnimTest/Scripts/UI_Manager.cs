using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UI_Manager : MonoBehaviour
{
    public Button Vertex_Btn;
    public Button Line_Btn;
    public Button Drag_Btn;

    public Elements_Manager elementsManager;
    
    Button Current_Btn;

    void Start()
    {
        Current_Btn = Vertex_Btn;

        UI_Switch_Btn(Vertex_Btn.gameObject);
        UI_Set_Mode("Vertex");
    }

    public void UI_Switch_Btn(GameObject sender)
    {
        Current_Btn.interactable = true;
        sender.GetComponent<Button>().interactable = false;
        Current_Btn = sender.GetComponent<Button>();
    }

    public void UI_Set_Mode(string mode)
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

public enum Modes
{
    Vertex, Line, Drag
}