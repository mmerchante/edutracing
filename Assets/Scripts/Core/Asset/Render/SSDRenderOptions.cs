using UnityEngine;
using System.Collections;

namespace SimpleSceneDescription
{
    public class SSDRenderOptions : SSDAsset
    {
        private int width;
        private int height;
        private int antialiasing;
        public int threads;
        public int bucketSize;

        protected override AssetType AssetType { get { return AssetType.RenderOptions; } }

        public SSDRenderOptions(int width, int height, int antialiasing, int threads, int bucketSize)
            : base(null)
        {
            this.width = width;
            this.height = height;
            this.antialiasing = antialiasing;
            this.threads = threads;
            this.bucketSize = bucketSize;
        }

        public override void OnToJSON(Hashtable ht)
        {
            ht["width"] = width;
            ht["height"] = height;
            ht["antialiasing"] = antialiasing;
            ht["threads"] = threads;
            ht["bucketSize"] = bucketSize;
        }
    }
}