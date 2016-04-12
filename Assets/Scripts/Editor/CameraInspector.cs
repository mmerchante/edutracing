using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Camera))]
public class CameraInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Camera camera = target as Camera;
        camera.fieldOfView = EditorGUILayout.Slider("Field of view", camera.fieldOfView, 1f, 179f);
    }
}