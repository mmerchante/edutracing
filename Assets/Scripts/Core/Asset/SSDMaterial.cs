using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleSceneDescription
{
    [System.Serializable]
    public enum MaterialType
    {
        None = 0,
        Color = 1,
        Diffuse = 2,
        Phong = 3,
        Reflective = 4,
        Refractive = 5
    }

    public class SingleChannel
    {
        public float value;
        public Texture2D texture;
        public Vector2 offset;
        public Vector2 scale;
        
        public void ToJSON(string channelName, Hashtable ht)
        {
            if (texture)
            {
                ht[channelName + "TextureId"] = texture;
                ht[channelName + "Offset"] = SerializationUtils.ToJSON(offset);
                ht[channelName + "Scale"] = SerializationUtils.ToJSON(scale);
            }
            else
                ht[channelName] = value;
        }
    }

    public class ColorChannel
    {
        public Color value;
        public Texture2D texture;
        public Vector2 offset;
        public Vector2 scale;

        public void ToJSON(string channelName, Hashtable ht)
        {
            if (texture)
            {
                ht[channelName + "TextureId"] = texture;
                ht[channelName + "Offset"] = SerializationUtils.ToJSON(offset);
                ht[channelName + "Scale"] = SerializationUtils.ToJSON(scale);
            }
            else
                ht[channelName] = SerializationUtils.ToJSON(value);
        }
    }

    public class SSDMaterial : SSDAsset
    {
        protected sealed override AssetType AssetType { get { return AssetType.Material; } }

        protected Dictionary<string, ColorChannel> colorChannels = new Dictionary<string, ColorChannel>();
        protected Dictionary<string, SingleChannel> singleChannels = new Dictionary<string, SingleChannel>();

        protected MaterialType materialType;

        public SSDMaterial(Material material) : base(material)
        {
            if (material.shader.name == "SSD/SSDColorShader")
            {
                this.materialType = SimpleSceneDescription.MaterialType.Color;
                AddColorChannel("color", "_Color", "_ColorTexture", material);
            }
            else if (material.shader.name == "SSD/SSDDiffuseShader")
            {
                this.materialType = SimpleSceneDescription.MaterialType.Diffuse;
                AddColorChannel("color", "_Color", "_ColorTexture", material);
            }
            else if (material.shader.name == "SSD/SSDPhongShader")
            {
                this.materialType = SimpleSceneDescription.MaterialType.Phong;
                AddColorChannel("color", "_Color", "_ColorTexture", material);
                AddColorChannel("specularColor", "_SpecularColor", "_SpecularTexture", material);
                AddSingleChannel("exponent", "_Exponent", "_ExponentTexture", material);
            }
            else if (material.shader.name == "SSD/SSDReflectionShader")
            {
                this.materialType = SimpleSceneDescription.MaterialType.Reflective;
                AddColorChannel("reflectivityColor", "_ReflectivityColor", "_ReflectivityTexture", material);
            }
            else if (material.shader.name == "SSD/SSDReflectionShader")
            {
                this.materialType = SimpleSceneDescription.MaterialType.Reflective;
                AddColorChannel("reflectivityColor", "_ReflectivityColor", "_ReflectivityTexture", material);
                AddColorChannel("refractionColor", "_RefractionColor", "_RefractionTexture", material);
            }
        }

        public void AddSingleChannel(string channelName, string propertyName, string texturePropertyName, Material material)
        {
            float value = material.GetFloat(propertyName);
            Texture texture = material.GetTexture(texturePropertyName);
            Vector2 offset = material.GetTextureOffset(texturePropertyName);
            Vector2 scale = material.GetTextureScale(texturePropertyName);

            SingleChannel channel = new SingleChannel();
            channel.value = value;
            channel.texture = texture is Texture2D ? (texture as Texture2D) : null;
            channel.offset = offset;
            channel.scale = scale;

            this.singleChannels[channelName] = channel;
        }

        public void AddColorChannel(string channelName, string propertyName, string texturePropertyName, Material material)
        {
            Color value = material.GetColor(propertyName);
            Texture texture = material.GetTexture(texturePropertyName);
            Vector2 offset = material.GetTextureOffset(texturePropertyName);
            Vector2 scale = material.GetTextureScale(texturePropertyName);
            
            ColorChannel channel = new ColorChannel();
            channel.value = value;
            channel.texture = texture is Texture2D ? (texture as Texture2D) : null;
            channel.offset = offset;
            channel.scale = scale;

            this.colorChannels[channelName] = channel;
        }

        protected override void OnGetDependencies(List<Object> objects)
        {
            base.OnGetDependencies(objects);

            foreach(ColorChannel channel in colorChannels.Values)
                if (channel.texture)
                    objects.Add(channel.texture);

            foreach(SingleChannel channel in singleChannels.Values)
                if (channel.texture)
                    objects.Add(channel.texture);
        }
        
        public override void OnToJSON(Hashtable ht)
        {
            base.OnToJSON(ht);
            ht["materialType"] = this.materialType.ToString();

            foreach (string key in colorChannels.Keys)
                colorChannels[key].ToJSON(key, ht);

            foreach (string key in singleChannels.Keys)
                singleChannels[key].ToJSON(key, ht);
        }
    }
}