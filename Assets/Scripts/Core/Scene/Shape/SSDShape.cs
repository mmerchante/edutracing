using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    [System.Serializable]
    public enum ShapeType
    {
        Plane = 0,
        Sphere = 1,
        Cube = 2,
        Mesh = 3,
    }

    public abstract class SSDShape : SSDSceneObject
    {
        protected sealed override SceneObjectType SceneObjectType { get { return SceneObjectType.Shape; } }
        protected abstract ShapeType ShapeType { get; }

        protected Material material;

        public SSDShape(GameObject gameObject) : base(gameObject) {}

        protected override void OnGetDependencies(System.Collections.Generic.List<Object> objects)
        {
            base.OnGetDependencies(objects);

            if(material)
                objects.Add(material);
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["shapeType"] = this.ShapeType.ToString();
            ht["materialID"] = material;
        }
    }
}