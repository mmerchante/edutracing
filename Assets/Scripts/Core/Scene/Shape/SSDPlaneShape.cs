using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDPlaneShape : SSDShape
    {
        protected bool finite = true;
        protected float width = 10f;
        protected float depth = 10f;

        protected override ShapeType ShapeType { get { return ShapeType.Plane; } }

        public SSDPlaneShape(GameObject gameObject) : base(gameObject)
        {
            PlanePrimitive plane = gameObject.GetComponent<PlanePrimitive>();

            this.Material = plane.material;
            this.finite = plane.finite;
            this.width = plane.width;
            this.depth = plane.depth;
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["finite"] = finite;
            if (finite)
            {
                ht["width"] = width;
                ht["depth"] = depth;
            }
        }
    }
}