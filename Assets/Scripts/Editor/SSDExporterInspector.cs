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
            string path = exporter.filename;
            if (path.Length == 0) {
                Debug.LogError("SSDExporter Error: Filename cannot be empty.");
                return;
            }

            path = Path.Combine(Application.dataPath, path);

            if (!exporter.overwrite && File.Exists(path)) {
                Debug.LogError("SSDExporter Error: File already exists.  Enable overwrite option to overwrite the file.");
                return;
            }

            Debug.Log("SSDExporter: Serializing scene...");
            string data = exporter.SerializeScene();
            File.WriteAllText(path, data);
            Debug.Log("SSDExporter: File '" + path + "' created successfully.");
            AssetDatabase.Refresh();
        }
    }
}
