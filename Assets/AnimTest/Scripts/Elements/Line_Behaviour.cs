using UnityEngine;
using System.Collections;

public class Line_Behaviour : MonoBehaviour
{
    public Material IDLE;
    public Material Selected;

    LineRenderer Line;

    Transform Start_Vertex;
    Transform End_Vertex;

    public C_Vertex Start_C_Vertex;
    public C_Vertex End_C_Vertex;

    BoxCollider2D Line_Collider;

    void Start()
    {
        Line = gameObject.GetComponent<LineRenderer>();
    }

    public void SetPositions()
    {
        Line.SetPosition(0, Start_Vertex.position);
        Line.SetPosition(1, End_Vertex.position);

        ApplyCollider();
    }

    public void SetStartVertex(Transform startVertex)
    {
        Start_Vertex = startVertex;
        Start_C_Vertex = startVertex.GetComponent<Vertex_Behaviour>().Vertex;
    }

    public Transform GetStartVertex()
    {
        return Start_Vertex;
    }

    public void SetEndVertex(Transform endVertex)
    {
        End_Vertex = endVertex;
        End_C_Vertex = endVertex.GetComponent<Vertex_Behaviour>().Vertex;
    }

    public Transform GetEndVertex()
    {
        return End_Vertex;
    }

    public void CreateCollider()
    {
        GameObject go = new GameObject();
        go.transform.parent = transform;
        go.AddComponent<BoxCollider2D>();
        Line_Collider = go.GetComponent<BoxCollider2D>();
        Line_Collider.isTrigger = true;

        go.AddComponent<Line_Collider_Behaivour>();
        go.GetComponent<Line_Collider_Behaivour>().SetMaterials(IDLE, Selected);

        ApplyCollider();
    }

    public void ApplyCollider()
    {
        float length = Vector3.Distance(Start_Vertex.position, End_Vertex.position);
        Line_Collider.size = new Vector2(length, 0.1f);
        Line_Collider.transform.position = (Start_Vertex.position + End_Vertex.position) / 2;
        Vector2 diff = (End_Vertex.position - Start_Vertex.position).normalized;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Line_Collider.transform.rotation = Quaternion.identity;
        Line_Collider.transform.Rotate(0f, 0f, angle);
    }

    public void DeleteLine()
    {
        Start_C_Vertex.DeleteLine(this);
    }
}
