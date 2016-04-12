using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDAmbientLight : SSDLight
    {
        public SSDAmbientLight(Light light) : base(light) { }

        protected override LightType LightType { get { return LightType.Ambient; } }
    }
}