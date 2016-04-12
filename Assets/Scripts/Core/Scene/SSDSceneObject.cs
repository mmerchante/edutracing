using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleSceneDescription
{
    [System.Serializable]
    public enum SceneObjectType
    {
        None = 0, // Empty object
        Camera = 1,
        Light = 2,
        Shape = 3,
    }

    /// <summary>
    /// Scene objects are the elements that will ultimately get rendered: it is the base class for
    /// anything that can be in the scene
    /// </summary>
    public class SSDSceneObject : JSONSerializable
    {
        private int id = -1;
        public int Id { get { return id; } }
        protected virtual SceneObjectType SceneObjectType { get { return SceneObjectType.None; } }

        protected Transform transform;
        protected GameObject gameObject;

        protected SSDSceneObject[] children;

        public SSDSceneObject(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.transform = gameObject.transform;
        }

        public void Initialize(int id, Dictionary<GameObject, SSDSceneObject> sceneMap)
        {
            this.id = id;
            this.children = new SSDSceneObject[this.transform.childCount];

            for (int i = 0; i < this.transform.childCount; i++)
                this.children[i] = sceneMap[this.transform.GetChild(i).gameObject];
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
            ht["name"] = this.gameObject.name;
            ht["localPosition"] = SerializationUtils.ToJSON(this.transform.localPosition);
            ht["localRotation"] = SerializationUtils.ToJSON(this.transform.localEulerAngles);
            ht["localScale"] = SerializationUtils.ToJSON(this.transform.localScale);
            ht["type"] = this.SceneObjectType.ToString();
            ht["children"] = SerializationUtils.ToJSON(children);
        }

        public Hashtable ToJSON()
        {
            Hashtable ht = new Hashtable();
            OnToJSON(ht);
            return ht;
        }
    }
}