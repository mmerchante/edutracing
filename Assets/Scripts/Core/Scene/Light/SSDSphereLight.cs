using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDSphereLight : SSDLight
    {
        protected float radius;

        public SSDSphereLight(Light light) : base(light)
        {
            LightAdapter adapter = light.GetComponent<LightAdapter>();

            if (adapter)
                this.radius = adapter.radius;
        }

        protected override LightType LightType { get { return LightType.Sphere; } }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["radius"] = radius;
        }
    }
}