using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(Light))]
public class LightAdapter : MonoBehaviour
{
    [HideInInspector]
    public bool castShadows = false;

    [HideInInspector] 
    public float width = 1f;

    [HideInInspector]
    public float height = 1f;

    [HideInInspector] 
    public float radius = 1f;

    [HideInInspector] 
    public Texture2D texture; // For dome lights

    [HideInInspector]
    public SimpleSceneDescription.LightType lightType = SimpleSceneDescription.LightType.Ambient;

    public static SimpleSceneDescription.LightType GetSSDLightTypeFromUnityLight(Light light)
    {
        LightType type = light.type;
        LightAdapter lightAdapter = light.gameObject.GetComponent<LightAdapter>();

        if (lightAdapter)
        {
            // Adapter has correct type, always
            return lightAdapter.lightType;
        }
        else
        {
            switch (type)
            {
                case LightType.Area:
                    return SimpleSceneDescription.LightType.Rectangle;

                case LightType.Directional:
                    return SimpleSceneDescription.LightType.Directional;

                case LightType.Point:
                    return SimpleSceneDescription.LightType.Point;

                case LightType.Spot:
                    return SimpleSceneDescription.LightType.Spot;

                default:
                    return SimpleSceneDescription.LightType.Point;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Light light = GetComponent<Light>();

        if (light)
            Gizmos.color = light.color;

        Gizmos.matrix = this.transform.localToWorldMatrix;

        switch (lightType)
        {
            case SimpleSceneDescription.LightType.Rectangle:
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, 0f));
                break;
            case SimpleSceneDescription.LightType.Sphere:
                Gizmos.DrawWireSphere(Vector3.zero, radius);
                break;
            case SimpleSceneDescription.LightType.Dome:
                Gizmos.color *= 2f;
                // Invert normals!
                Gizmos.matrix = this.transform.localToWorldMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * -1f);
                Gizmos.DrawWireSphere(-this.transform.position, 100f);
                break;
        }
    }
}