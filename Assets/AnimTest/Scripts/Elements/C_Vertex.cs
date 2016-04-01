using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C_Vertex
{
    //int ID;
    List<Vector3> Frames_Positions;
    List<C_Vertex> Connected_Vertices;
    List<Line_Behaviour> Lines;

    public C_Vertex(Vector3 start_frame_pos)
    {
        //ID = id;
        Frames_Positions = new List<Vector3>();
        Frames_Positions.Add(start_frame_pos);
        Connected_Vertices = new List<C_Vertex>();
        Lines = new List<Line_Behaviour>();
    }

    public void SetCurrentFrame(int frame, Vector3 position)
    {
        Frames_Positions[frame] = position;
    }

    public void AddConnectedVertices(C_Vertex vertex)
    {
        Connected_Vertices.Add(vertex);
    }

    public void AddLine(Line_Behaviour line)
    {
        Lines.Add(line);
    }

    public bool IsVertexAlreadyConnected(C_Vertex vertex)
    {
        for (int i = 0; i < Connected_Vertices.Count; i++)
        {
            if (Connected_Vertices[i] == vertex)
                return true;
        }
        return false;
    }

    public void ApplyLinesPositions()
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            Lines[i].SetPositions();
        }
    }

    public void DeleteVertex()
    {
        while (Lines.Count != 0)
        {
            DeleteLine(Lines[0]);
        }
    }

    public void DeleteConnection(C_Vertex vertex)
    {
        Connected_Vertices.Remove(vertex);
    }

    public void DeleteLine(Line_Behaviour line)
    {
        line.Start_C_Vertex.RemoveLine(line);
        line.Start_C_Vertex.DeleteConnection(line.End_C_Vertex);

        line.End_C_Vertex.RemoveLine(line);
        line.End_C_Vertex.DeleteConnection(line.Start_C_Vertex);
        Object.Destroy(line.gameObject);
    }

    public void RemoveLine(Line_Behaviour line)
    {
        Lines.Remove(line);
    }
}
