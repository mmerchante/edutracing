using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDDomeLight : SSDLight
    {
        private Texture2D texture;

        public SSDDomeLight(Light light)
            : base(light)
        {
            LightAdapter adapter = light.GetComponent<LightAdapter>();

            if (adapter)
                this.texture = adapter.texture;
        }

        protected override LightType LightType { get { return LightType.Dome; } }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["textureId"] = texture;
        }
    }
}