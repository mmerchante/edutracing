using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDPinholeCamera : SSDCamera
    {
        private Camera camera;

        public SSDPinholeCamera(Camera camera) : base(camera.gameObject) 
        {
            this.camera = camera;
        }

        protected override CameraType CameraType { get { return CameraType.Pinhole; } }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["fieldOfView"] = this.camera.fieldOfView;
        }
    }
}