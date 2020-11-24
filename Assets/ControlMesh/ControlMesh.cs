using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlMesh
{
    public ControlMesh(Mesh mesh)
    {
        this.mesh = mesh;
        //vertexParms = new VertexParm[mesh.vertexCount];
        MakeControlMesh(mesh);
    }
    public void UpdateMeshTest(float w1, float w2)
    {
        var vertices = mesh.vertices;
        Debug.Assert(mesh.vertexCount == vertexParms.Count);
        Vector3[] newVertices = new Vector3[mesh.vertexCount];

        foreach (var vertex_param in vertexParms)
        {
            int vid = vertex_param.Key;
            VertexParm param = vertex_param.Value;

            var vec_sum = param.U1 * w1 + param.U2 * w2;

            newVertices[vid] = vertices[vid] + vec_sum;
        }
        mesh.vertices = newVertices;
    }
    void MakeControlMesh(Mesh mesh)
    {
        int[] indices = mesh.triangles;
        int faceCount = indices.Length / 3;
        Debug.Assert(mesh.subMeshCount == 1, "Invalid submesh count");
        Debug.Assert(indices.Length % 3 == 0, "Invalid mesh");

        for (int face_id = 0; face_id < faceCount; face_id++)
        {
            int v1_id = indices[face_id * 3 + 0];
            int v2_id = indices[face_id * 3 + 1];
            int v3_id = indices[face_id * 3 + 2];

            InitVertexParam(face_id, v1_id);
            InitVertexParam(face_id, v2_id);
            InitVertexParam(face_id, v3_id);

            var newFace = new Face(face_id, v1_id, v2_id, v3_id);
            faces.Add(face_id, newFace);
        }
        Debug.Assert(vertexParms.Count == mesh.vertices.Length, "Fail to make control mesh.");
    }
    void InitVertexParam(int face_id, int vertex_id)
    {
        //if (init_ok[vertex_id]) return;
        if (vertexParms.ContainsKey(vertex_id))
            return;

        var tangent = mesh.tangents[vertex_id];
        var normal = mesh.normals[vertex_id];

        var tan3 = new Vector3(tangent.x, tangent.y, tangent.z);
        var u1 = Vector3.Cross(tan3, normal).normalized * tangent.w;
        var u2 = Vector3.Cross(u1, normal).normalized;

        float angle1 = Vector3.Angle(u1, normal) - 90;
        float angle2 = Vector3.Angle(u2, normal) - 90;
        float angle3 = Vector3.Angle(u1, u2) - 90;

        Debug.Assert(angle1 <= float.Epsilon, angle1.ToString());
        Debug.Assert(angle2 <= float.Epsilon, angle2.ToString());
        Debug.Assert(angle3 <= float.Epsilon, angle3.ToString());

        var parm = new VertexParm(vertex_id, face_id, u1, u2);
        vertexParms.Add(vertex_id, parm);
    }

    Mesh mesh;
    public Dictionary<int, VertexParm> vertexParms { get; } = new Dictionary<int, VertexParm>();
    Dictionary<int, Face> faces { get; } = new Dictionary<int, Face>();
}
public class VertexParm
{
    int _vertex_id;
    int _face_id;
    Vector3 _u1, _u2;
    public VertexParm(int vertex_id, int face_id, Vector3 u1, Vector3 u2)
    {
        _vertex_id = vertex_id;
        _face_id = face_id;
        _u1 = u1;
        _u2 = u2;
    }

    public Vector3 U1 { get => _u1; }
    public Vector3 U2 { get => _u2; }
}
public class Face
{
    int _face_id;
    HashSet<int> _indices; //The length must be three.

    public Face(int face_id, int v1_id, int v2_id, int v3_id)
    {
        _face_id = face_id;
        _indices = new HashSet<int>();
        AddVertexID(v1_id);
        AddVertexID(v2_id);
        AddVertexID(v3_id);
    }
    void AddVertexID(int vertex_id)
    {
        bool ret = _indices.Add(vertex_id);
        Debug.Assert(ret, "Invalid vertex_id");
    }
}