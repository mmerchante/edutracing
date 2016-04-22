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

        private Material material;

        // Use a fallback material!
        private Material GetDefaultMaterial()
        {
            Material m = new Material(Shader.Find("SSD/SSDColorShader"));
            m.SetColor("_Color", Color.magenta);
            m.name = "ErrorMaterial";
            return m;
        }

        protected Material Material 
        {
            get 
            {
                if(!material)
                    material = GetDefaultMaterial();

                return material; 
            }
            set
            {
                material = value;

                if(!material)
                    material = GetDefaultMaterial();
            }
        }

        public SSDShape(GameObject gameObject) : base(gameObject) {}

        protected override void OnGetDependencies(System.Collections.Generic.List<Object> objects)
        {
            base.OnGetDependencies(objects);
            objects.Add(Material);
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["shapeType"] = this.ShapeType.ToString();
            ht["materialID"] = Material;
        }
    }
}