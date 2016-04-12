using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDPlane : SSDShape
    {
        public bool finite = true;

        public float width = 10f;
        public float height = 10f;
        public Vector3 normal = Vector3.up;

        protected override ShapeType ShapeType { get { return ShapeType.Plane; } }

        public SSDPlane(GameObject gameObject) : base(gameObject) { }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["finite"] = finite;
            ht["width"] = width;
            ht["height"] = height;
            ht["normal"] = SerializationUtils.ToJSON(normal);
        }
    }
}