﻿using System.Collections;
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

        if (GUILayout.Button("Init"))
        {
            //Debug.Log(mono.GetComponent<MeshFilter>().mesh.vertices.Length);
            
            mono.InitControlMesh();
        }
        if (GUILayout.Button("Visible"))
        {
            mono.visible = !mono.visible;
        }
        if (GUILayout.Button("Update param"))
        {
            mono.UpdateMesh(mono.w1, mono.w2);
        }
    }
}