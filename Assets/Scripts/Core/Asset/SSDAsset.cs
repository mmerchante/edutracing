using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleSceneDescription
{
    [System.Serializable]
    public enum AssetType
    {
        None = 0,
        RenderOptions = 1,
        Material = 2,
        Texture = 3,
        Mesh = 4,
    }

    /// <summary>
    /// Scene assets are objects that are shared throughout the scene, such as
    /// materials, textures, meshes, etc.
    /// </summary>
    public abstract class SSDAsset : JSONSerializable
    {
        protected abstract AssetType AssetType { get; }

        public int Id { get { return id; } }

        private string name = "";
        private int id = -1;

        public SSDAsset(UnityEngine.Object obj) 
        {
            if(obj)
                this.name = obj.name;
        } 

        public void Initialize(int id)
        {
            this.id = id;
        }
        
        protected virtual void OnGetDependencies(List<UnityEngine.Object> objects)
        {
        }

        public virtual List<UnityEngine.Object> GetDependencies()
        {
            List<UnityEngine.Object> list = new List<Object>();
            OnGetDependencies(list);
            return list;
        }

        public virtual void OnToJSON(Hashtable ht)
        {
            ht["id"] = this.id;
            ht["name"] = this.name; ;
            ht["assetType"] = this.AssetType.ToString();
        }

        public Hashtable ToJSON()
        {
            Hashtable ht = new Hashtable();
            OnToJSON(ht);
            return ht;
        }
    }
}