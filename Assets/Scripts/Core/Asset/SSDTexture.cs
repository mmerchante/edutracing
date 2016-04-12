using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDTexture : SSDAsset
    {
        protected override AssetType AssetType { get { return SimpleSceneDescription.AssetType.Texture; } }

        private Texture2D texture;

        public SSDTexture(Texture2D texture) : base(texture)
        {
            this.texture = texture;
        }

        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["base64PNG"] = SerializationUtils.EncodePNGToBase64(texture);
        }
    }
}