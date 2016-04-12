using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDPointLight : SSDLight
    {
        public SSDPointLight(Light light) : base(light) { }

        protected override LightType LightType { get { return LightType.Point; } }
    }
}