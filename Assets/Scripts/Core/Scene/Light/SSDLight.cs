using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    [System.Serializable]
    public enum LightType
    {
        Ambient = 0,
        Directional = 1,
        Point = 2,
        Spot = 3,
        Area = 4
    }

    public abstract class SSDLight : SSDSceneObject
    {
        protected Color color = Color.white;
        protected float intensity = 1f;

        protected bool castShadows = false;

        protected abstract LightType LightType { get; }
        protected sealed override SceneObjectType SceneObjectType { get { return SceneObjectType.Light; } }

        public SSDLight(Light light) : base(light.gameObject) 
        {
            this.color = light.color;
            this.intensity = light.intensity;

            LightAdapter adapter = light.gameObject.GetComponent<LightAdapter>();

            if (adapter)
                this.castShadows = adapter.castShadows;
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["lightType"] = LightType.ToString();
            ht["color"] = SerializationUtils.ToJSON(color);
            ht["intensity"] = intensity;
        }
    }
}