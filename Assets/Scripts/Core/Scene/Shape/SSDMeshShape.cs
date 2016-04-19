using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDMeshShape : SSDShape
    {
        protected override ShapeType ShapeType { get { return ShapeType.Mesh; } }

        protected Mesh mesh;

        public SSDMeshShape(GameObject gameObject) : base(gameObject)
        {   
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();

            if (renderer)
                material = renderer.sharedMaterial;

            MeshFilter filter = gameObject.GetComponent<MeshFilter>();

            if (filter)
                mesh = filter.sharedMesh;
        }

        protected override void OnGetDependencies(System.Collections.Generic.List<Object> objects)
        {
            base.OnGetDependencies(objects);
            objects.Add(mesh);
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["meshId"] = mesh;
        }
    }
}