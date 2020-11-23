using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]
#endif

public class ControlMeshMono : MonoBehaviour
{
    public bool visible_gizmo = true;
    ControlMesh controlMesh;
    public bool initControlMesh = false;
    public float w1 = 0.1f, w2 = 0.1f;

    // Update is called once per frame
    void OnGUI()
    {
#if UNITY_EDITOR
        if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
        {
            EditorUtility.SetDirty(this); // this is important, if omitted, "Mouse down" will not be display
        }
#endif

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0) //left mouse down
        {
            Event e = Event.current;

            Debug.Log("Button down");
        }
    }
 
    void OnDrawGizmos()
    {
        if(visible_gizmo && initControlMesh && controlMesh!=null)
        {
            var mesh = GetComponent<MeshFilter>().sharedMesh;
            var vertices = mesh.vertices;

            Debug.Assert(mesh.vertexCount == controlMesh.vertexParms.Count);

            //for (int i = 0; i < vertices.Length; i++)
            foreach(var vertex_param in controlMesh.vertexParms)
            {
                int vid = vertex_param.Key;
                VertexParm param = vertex_param.Value;

                var vertics_world = transform.localToWorldMatrix.MultiplyPoint3x4(vertices[vid]);
                float line_length = transform.lossyScale.x * 0.02f;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(vertics_world, vertics_world + param.U1 * line_length);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(vertics_world, vertics_world + param.U2 * line_length);

                //Gizmos.color = Color.white;
                //Gizmos.DrawLine(vertics_world, vertics_world + tan_sum * line_length);
            }
        }
    }
    public void InitControlMesh()
    {
        controlMesh = new ControlMesh(GetComponent<MeshFilter>().sharedMesh);
        initControlMesh = true;
    }
    public void UpdateMesh(float w1, float w2)
    {
        if (initControlMesh && controlMesh != null)
        {
            var mesh = GetComponent<MeshFilter>().sharedMesh;
            var vertices = mesh.vertices;

            Debug.Assert(mesh.vertexCount == controlMesh.vertexParms.Count);

            Vector3[] newVertices = new Vector3[mesh.vertexCount];

            //for (int i = 0; i < vertices.Length; i++)
            foreach (var vertex_param in controlMesh.vertexParms)
            {
                int vid = vertex_param.Key;
                VertexParm param = vertex_param.Value;

                var vec_sum = param.U1 * w1 + param.U2 * w2;

                newVertices[vid] = vertices[vid] + vec_sum;
            }
            mesh.vertices = newVertices;
        }

    }

}

