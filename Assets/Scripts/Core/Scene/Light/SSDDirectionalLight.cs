using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDDirectionalLight : SSDLight
    {
        public SSDDirectionalLight(Light light) : base(light) { }

        protected override LightType LightType { get { return LightType.Directional; } }
    }
}