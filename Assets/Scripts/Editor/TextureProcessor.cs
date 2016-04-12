using UnityEngine;
using UnityEditor;
using System.Collections;

public class TextureProcessor : AssetPostprocessor
{
    public void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter) assetImporter;
        textureImporter.isReadable = true;
        textureImporter.textureFormat = TextureImporterFormat.RGBA32;
        textureImporter.mipmapEnabled = false;
    }
}