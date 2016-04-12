using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using SimpleSceneDescription;
using System.IO;

[CustomEditor(typeof(SSDExporter))]
public class SSDExporterInspector : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SSDExporter exporter = target as SSDExporter;

        if (GUILayout.Button("Export"))
        {
            string data = exporter.SerializeScene();
            File.WriteAllText(Path.Combine(Application.dataPath, "output.txt"), data);
            AssetDatabase.Refresh();
        }
    }
}