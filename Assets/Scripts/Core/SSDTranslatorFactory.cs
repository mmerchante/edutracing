using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDTranslatorFactory
    {
        private static SSDTranslatorFactory instance;

        public static SSDTranslatorFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new SSDTranslatorFactory();

                return instance;
            }
        }

        public static SSDAsset TranslateUnityObject(UnityEngine.Object obj)
        {
            SSDAsset asset = null;

            if(obj is Material)
            {
                asset = new SSDMaterial(obj as Material);
            }
            else if(obj is Texture2D)
            {
                asset = new SSDTexture(obj as Texture2D);
            }
            else if (obj is Mesh)
            {
                asset = new SSDMesh(obj as Mesh);
            }

            return asset;
        }

        public static SSDSceneObject TranslateSceneObject(GameObject gameObject)
        {
            SSDSceneObject result = null;

            if(gameObject.GetComponent<Camera>())
            {
                result = new SSDPinholeCamera(gameObject.GetComponent<Camera>());
            } 
            else if(gameObject.GetComponent<MeshFilter>() && gameObject.GetComponent<MeshRenderer>())
            {
                result = new SSDMeshShape(gameObject);
            }
            else if(gameObject.GetComponent<PlanePrimitive>())
            {
                result = new SSDPlaneShape(gameObject);
            }
            else if (gameObject.GetComponent<CubePrimitive>())
            {
                result = new SSDCubeShape(gameObject);
            }
            else if (gameObject.GetComponent<SpherePrimitive>())
            {
                result = new SSDSphereShape(gameObject);
            }
            else if(gameObject.GetComponent<Light>())
            {
                Light light = gameObject.GetComponent<Light>();

                switch(LightAdapter.GetSSDLightTypeFromUnityLight(light))
                {
                    case LightType.Directional:
                        result = new SSDDirectionalLight(light);
                        break;

                    case LightType.Point:
                        result = new SSDPointLight(light);
                        break;

                    case LightType.Ambient:
                        result = new SSDAmbientLight(light);
                        break;

                    case LightType.Spot:
                        result = new SSDSpotLight(light);
                        break;

                    case LightType.Rectangle:
                        result = new SSDRectangleLight(light);
                        break;

                    case LightType.Sphere:
                        result = new SSDSphereLight(light);
                        break;

                    case LightType.Dome:
                        result = new SSDDomeLight(light);
                        break;
                }
            }
            else
            {
                // Empty object
                result = new SSDSceneObject(gameObject);
            }

            return result;
        }    
    }
}