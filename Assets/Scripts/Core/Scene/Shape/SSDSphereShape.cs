using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDSphereShape : SSDShape
    {
        protected float radius = 1f;

        protected override ShapeType ShapeType { get { return ShapeType.Sphere; } }

        public SSDSphereShape(GameObject gameObject) : base(gameObject)
        {
            SpherePrimitive sphere = gameObject.GetComponent<SpherePrimitive>();

            this.Material = sphere.material;
            this.radius = sphere.radius;
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["radius"] = radius;
        }
    }
}