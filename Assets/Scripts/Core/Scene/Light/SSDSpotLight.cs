using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDSpotLight : SSDLight
    {
        protected float spotAngle;

        public SSDSpotLight(Light light) : base(light) 
        {
            this.spotAngle = light.spotAngle;
        }

        protected override LightType LightType { get { return LightType.Spot; } }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["spotAngle"] = spotAngle;
        }
    }
}