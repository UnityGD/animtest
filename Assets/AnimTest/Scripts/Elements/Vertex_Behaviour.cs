using UnityEngine;
using System.Collections;

public class Vertex_Behaviour : MonoBehaviour
{
    public Material IDLE;
    public Material Selected;

    Elements_Manager elementsManager;
    bool IsThisVertexSelected = false;

    public C_Vertex Vertex;

    void Start()
    {
        elementsManager = GameObject.Find("Main_Manager").GetComponent<Elements_Manager>();
    }

    public void SetVertex()
    {
        Vertex = new C_Vertex(transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.tag == "Pointer") && (!elementsManager.IsVertexSelected()))
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
        }
    }

    public void DeleteVertex()
    {
        Vertex.DeleteVertex();
        Object.Destroy(gameObject);
    }
}
