using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererInspector : Editor
{
    public override void OnInspectorGUI()
    {
        MeshRenderer renderer = target as MeshRenderer;
        renderer.sharedMaterial = (Material) EditorGUILayout.ObjectField("Material", renderer.sharedMaterial, typeof(Material), false);
    }
}