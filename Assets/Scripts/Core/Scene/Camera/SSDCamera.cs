using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    [System.Serializable]
    public enum CameraType
    {
        Pinhole = 0, // Simple camera
    }

    public abstract class SSDCamera : SSDSceneObject
    {
        public SSDCamera(GameObject gameObject) : base(gameObject) { }

        protected sealed override SceneObjectType SceneObjectType { get { return SceneObjectType.Camera; } }
        protected abstract CameraType CameraType { get; }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["cameraType"] = this.CameraType.ToString();
        }
    }
}