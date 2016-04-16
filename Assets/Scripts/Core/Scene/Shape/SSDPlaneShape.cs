using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDPlaneShape : SSDShape
    {
        protected bool finite = true;
        protected float width = 10f;
        protected float height = 10f;

        protected override ShapeType ShapeType { get { return ShapeType.Plane; } }

        public SSDPlaneShape(GameObject gameObject) : base(gameObject)
        {
            PlanePrimitive plane = gameObject.GetComponent<PlanePrimitive>();

            this.material = plane.material;
            this.finite = plane.finite;
            this.width = plane.width;
            this.height = plane.height;
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["finite"] = finite;
            ht["width"] = width;
            ht["height"] = height;
        }
    }
}