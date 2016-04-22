using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDCubeShape : SSDShape
    {
        protected float width = 1f;
        protected float height = 1f;
        protected float depth = 1f;

        protected override ShapeType ShapeType { get { return ShapeType.Cube; } }

        public SSDCubeShape(GameObject gameObject) : base(gameObject)
        {
            CubePrimitive cube = gameObject.GetComponent<CubePrimitive>();

            this.Material = cube.material;
            this.width = cube.width;
            this.height = cube.height;
            this.depth = cube.depth;
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["width"] = width;
            ht["height"] = height;
            ht["depth"] = depth;
        }
    }
}