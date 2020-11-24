using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ControlMeshMono))]
public class ControlMeshEditor : Editor
{
    ControlMeshMono mono;

    private void OnEnable()
    {
        mono = (ControlMeshMono)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Init param"))
        {
            //Debug.Log(mono.GetComponent<MeshFilter>().mesh.vertices.Length);
            mono.InitControlMesh();
        }
        if (GUILayout.Button("Update param"))
        {
            mono.UpdateMeshTest(mono.w1, mono.w2);
        }
    }
}