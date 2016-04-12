using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(Light))]
public class LightAdapter : MonoBehaviour
{
    [HideInInspector]
    public bool castShadows = false;

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
                    return SimpleSceneDescription.LightType.Area;

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
}