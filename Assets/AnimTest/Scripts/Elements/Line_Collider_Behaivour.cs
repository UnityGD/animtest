using UnityEngine;
using System.Collections;

public class Line_Collider_Behaivour : MonoBehaviour
{
    Material IDLE;
    Material Selected;

    Elements_Manager elementsManager;
    bool IsThisLineSelected = false;

    void Start()
    {
        elementsManager = GameObject.Find("Main_Manager").GetComponent<Elements_Manager>();
    }

    public void SetMaterials(Material idle, Material selected)
    {
        IDLE = idle;
        Selected = selected;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.tag == "Pointer") && (!elementsManager.IsLineSelected()))
        {
            transform.parent.GetComponent<LineRenderer>().material = Selected;
            elementsManager.Set_Selected_Line(transform.parent.GetComponent<Line_Behaviour>(), false);
            IsThisLineSelected = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.tag == "Pointer") && (IsThisLineSelected))
        {
            transform.parent.GetComponent<LineRenderer>().material = IDLE;
            elementsManager.Set_Selected_Line(transform.parent.GetComponent<Line_Behaviour>(), true);
        }
    }
}
