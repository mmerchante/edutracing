using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDMesh : SSDAsset
    {
        protected override AssetType AssetType { get { return SimpleSceneDescription.AssetType.Mesh; } }

        private Mesh mesh;

        public SSDMesh(Mesh mesh) : base(mesh)
        {
            this.mesh = mesh;
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["base64OBJ"] = SerializationUtils.EncodeMeshToBase64OBJ(mesh);
        }
    }
}