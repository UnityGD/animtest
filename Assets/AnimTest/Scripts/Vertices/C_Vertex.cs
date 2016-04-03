using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C_Vertex
{
    List<Vector3> Frames_Positions;
    List<C_Vertex> Connected_Vertices;
    List<Line_Behaviour> Lines;

    public C_Vertex(int frames, Vector3 start_frame_pos)
    {
        Frames_Positions = new List<Vector3>();
        Add_Frame_Position(frames, start_frame_pos);
        Connected_Vertices = new List<C_Vertex>();
        Lines = new List<Line_Behaviour>();
    }

    public void Add_Frame_Position(int frames, Vector3 position)
    {
        while (frames > Frames_Positions.Count)
        {
            Frames_Positions.Add(position);
        }
    }

    public void Set_Frame_Position(int frame, Vector3 position)
    {
        Frames_Positions[frame] = position;
    }

    public Vector3 Get_Next_Position(int start_frame, float delta)
    {
        return Vector3.Lerp(Frames_Positions[start_frame], Frames_Positions[start_frame + 1], delta);
    }

    public Vector3 Get_Current_Position(int frame)
    {
        return Frames_Positions[frame];
    }

    public void Add_Connected_Vertices(C_Vertex vertex)
    {
        Connected_Vertices.Add(vertex);
    }

    public void Add_Line(Line_Behaviour line)
    {
        Lines.Add(line);
    }

    public bool Is_Vertex_Already_Connected(C_Vertex vertex)
    {
        for (int i = 0; i < Connected_Vertices.Count; i++)
        {
            if (Connected_Vertices[i] == vertex)
                return true;
        }
        return false;
    }

    public void Apply_Lines_Positions()
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            Lines[i].Set_Positions();
        }
    }

    public void Destroy_Vertex()
    {
        while (Lines.Count != 0)
        {
            Destroy_Line(Lines[0]);
        }
    }

    public void Remove_Connection(C_Vertex vertex)
    {
        Connected_Vertices.Remove(vertex);
    }

    public void Destroy_Line(Line_Behaviour line)
    {
        line.Start_C_Vertex.Remove_Line(line);
        line.Start_C_Vertex.Remove_Connection(line.End_C_Vertex);

        line.End_C_Vertex.Remove_Line(line);
        line.End_C_Vertex.Remove_Connection(line.Start_C_Vertex);

        Object.Destroy(line.gameObject);
    }

    public void Remove_Line(Line_Behaviour line)
    {
        Lines.Remove(line);
    }
}
