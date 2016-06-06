using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDRectangleLight : SSDLight
    {
        protected float width;
        protected float height;

        public SSDRectangleLight(Light light)
            : base(light)
        {
            LightAdapter adapter = light.GetComponent<LightAdapter>();

            if (adapter)
            {
                this.width = adapter.width;
                this.height = adapter.height;
            }
        }

        protected override LightType LightType { get { return LightType.Rectangle; } }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["width"] = width;
            ht["height"] = height;
        }
    }
}