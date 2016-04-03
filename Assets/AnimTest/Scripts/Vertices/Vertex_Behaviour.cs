using UnityEngine;
using System.Collections;

public class Vertex_Behaviour : MonoBehaviour
{
    public Material IDLE;
    public Material Selected;

    Elements_Manager elementsManager;
    bool IsThisVertexSelected = false;

    public C_Vertex Vertex;

    public void Set_Vertex()
    {
        elementsManager = GameObject.Find("Main_Manager").GetComponent<Elements_Manager>();
        Vertex = new C_Vertex(elementsManager.Get_Frames_Count(), transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.tag == "Pointer") && (!elementsManager.Is_Vertex_Selected()))
        {
            GetComponent<SpriteRenderer>().material = Selected;
            elementsManager.Set_Selected_Vertex(transform, false);
            IsThisVertexSelected = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.tag == "Pointer") && (IsThisVertexSelected))
        {
            GetComponent<SpriteRenderer>().material = IDLE;
            elementsManager.Set_Selected_Vertex(transform, true);
            IsThisVertexSelected = false;
        }
    }

    public void Apply_Animation(int start_frame, float delta)
    {
        transform.position = Vertex.Get_Next_Position(start_frame, delta);
        Vertex.Apply_Lines_Positions();
    }

    public void Destroy_Vertex_Behaviour()
    {
        Vertex.Destroy_Vertex();
        Object.Destroy(gameObject);
    }
}
