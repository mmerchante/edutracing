using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Light))]
public class LightInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Light light = target as Light;
        LightAdapter adapter = light.gameObject.GetComponent<LightAdapter>();

        if (!adapter)
            adapter = light.gameObject.AddComponent<LightAdapter>();

        adapter.lightType = (SimpleSceneDescription.LightType) EditorGUILayout.EnumPopup("Type", adapter.lightType);

        light.color = EditorGUILayout.ColorField("Color", light.color);
        light.intensity = Mathf.Max(0f, EditorGUILayout.FloatField("Intensity", light.intensity));

        adapter.castShadows = EditorGUILayout.Toggle("Cast shadows", adapter.castShadows);

        switch(adapter.lightType)
        {
            case SimpleSceneDescription.LightType.Ambient:
                light.type = LightType.Point;
                break;

            case SimpleSceneDescription.LightType.Point:
                light.type = LightType.Point;
                break;

            case SimpleSceneDescription.LightType.Directional:
                light.type = LightType.Directional;
                break;

            case SimpleSceneDescription.LightType.Spot:
                light.spotAngle = EditorGUILayout.Slider("Angle", light.spotAngle, 1f, 179f);
                light.type = LightType.Spot;
                break;

            case SimpleSceneDescription.LightType.Rectangle:
                adapter.width = EditorGUILayout.FloatField("Width", adapter.width);
                adapter.height = EditorGUILayout.FloatField("Height", adapter.height);
                light.type = LightType.Area;
                break;

            case SimpleSceneDescription.LightType.Sphere:
                adapter.radius = EditorGUILayout.FloatField("Radius", adapter.radius);
                light.type = LightType.Area;
                break;

            case SimpleSceneDescription.LightType.Dome:
                adapter.texture = (Texture2D) EditorGUILayout.ObjectField("Texture", adapter.texture, typeof(Texture2D), false);
                break;

        }
    }
}