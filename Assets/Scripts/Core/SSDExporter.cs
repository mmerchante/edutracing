using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SimpleSceneDescription
{
    public class SSDExporter : MonoBehaviour
    {
        // The scene can have various cameras, but the renderer should use only one for now!
        public Camera mainCamera;

        public int width = 640;
        public int height = 360;

        [Range(1, 8)]
        public int antialiasingSamples = 1;

        [Range(0, 32)]
        public int threads = 0; // 0 means automatic

        [Range(8, 512)]
        public int bucketSize = 32;

        [Range(0,10)]
        public int reflectionTraceDepth = 5;

        [Range(0, 10)]
        public int refractionTraceDepth = 5;
        
        private List<SSDSceneObject> exportableObjects = new List<SSDSceneObject>();
        private List<SSDAsset> assets = new List<SSDAsset>();
        private SSDRenderOptions options;

        private Dictionary<UnityEngine.Object, SSDAsset> dependenciesMap = new Dictionary<UnityEngine.Object, SSDAsset>();

        private int mainCameraId = -1;

        private void ParseScene()
        {
            this.dependenciesMap = new Dictionary<UnityEngine.Object, SSDAsset>();
            this.exportableObjects = new List<SSDSceneObject>();
            this.assets = new List<SSDAsset>();
            int id = 1234; // Magic number for debugging

            Dictionary<GameObject, SSDSceneObject> sceneMap = new Dictionary<GameObject, SSDSceneObject>();

            Transform[] sceneTransforms = GameObject.FindObjectsOfType<Transform>();

            // Get all objects, ignore self
            for(int i = 0; i < sceneTransforms.Length; i++)
                if(sceneTransforms[i] != this.transform)
                    sceneMap[sceneTransforms[i].gameObject] = SSDTranslatorFactory.TranslateSceneObject(sceneTransforms[i].gameObject);

            // Initialize objects, let them find their children, etc.
            foreach (SSDSceneObject sceneObj in sceneMap.Values)
            {
                sceneObj.Initialize(id++, sceneMap);

                // Initialize dependencies that the scene needs
                Queue<UnityEngine.Object> dependencies = new Queue<UnityEngine.Object>(sceneObj.GetDependencies());

                while (dependencies.Count > 0)
                {
                    UnityEngine.Object dependencyAsset = dependencies.Dequeue();
                    
                    if (!dependenciesMap.ContainsKey(dependencyAsset))
                    {
                        SSDAsset asset = SSDTranslatorFactory.TranslateUnityObject(dependencyAsset);
                        asset.Initialize(id++);

                        dependenciesMap[dependencyAsset] = asset;
                        assets.Add(asset);

                        // Make sure all dependencies are considered
                        List<UnityEngine.Object> assetDependencies = asset.GetDependencies();
                        for (int i = 0; i < assetDependencies.Count; i++)
                            dependencies.Enqueue(assetDependencies[i]);
                    }
                }
            }

            // Find all root objects and save them
            GameObject[] roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            for (int i = 0; i < roots.Length; i++)
                if (sceneMap.ContainsKey(roots[i]))
                {
                    SSDSceneObject sceneObject = sceneMap[roots[i]];
                    this.exportableObjects.Add(sceneObject);

                    // Save main camera Id for later
                    if (roots[i] == mainCamera.gameObject && sceneObject != null)
                        mainCameraId = sceneObject.Id;
                }
        }
        
        private void PrepareData()
        {
            ParseScene();

            // Now add all assets (materials, textures, etc)
            this.options = new SSDRenderOptions(width, height, antialiasingSamples, threads, bucketSize, reflectionTraceDepth, refractionTraceDepth);
        }

        private bool ValidateScene()
        {
            if (!mainCamera)
            {
                Debug.LogError("Main camera was not set up! Invalid scene.");
                return false;
            } 
            else if(width <= 0 || height <= 0)
            {
                Debug.LogError("Invalid image size!");
                return false;
            }

            return true;
        }

        public string SerializeScene()
        {
            PrepareData();

            if (!ValidateScene())
                return "";
            
            Hashtable htScene = new Hashtable();
            htScene["objects"] = SerializationUtils.ToJSON(exportableObjects.ToArray());
            htScene["renderOptions"] = SerializationUtils.ToJSON(options);
            htScene["assets"] = SerializationUtils.ToJSON(assets.ToArray());
            htScene["mainCameraId"] = mainCameraId;

            InjectReferenceIDs(htScene);

            return SerializationUtils.FormatJson(htScene.encodeToJSON());
        }

        private void InjectReferenceIDs(ArrayList array)
        {
            for (int i = 0; i < array.Count; i++ )
            {
                object o = array[i];

                if (o is Hashtable)
                    InjectReferenceIDs(o as Hashtable);
                else if (o is ArrayList)
                    InjectReferenceIDs(o as ArrayList);
                else if (o is UnityEngine.Object)
                {
                    UnityEngine.Object unityObject = o as UnityEngine.Object;

                    if (dependenciesMap.ContainsKey(unityObject))
                        array[i] = dependenciesMap[unityObject].Id;
                }
            }
        }

        private void InjectReferenceIDs(Hashtable ht)
        {
            Hashtable injections = new Hashtable();

            foreach (object o in ht.Keys)
            {
                if (ht[o] is Hashtable)
                    InjectReferenceIDs(ht[o] as Hashtable);
                else if (ht[o] is ArrayList)
                    InjectReferenceIDs(ht[o] as ArrayList);
                else if (ht[o] is UnityEngine.Object)
                {
                    UnityEngine.Object unityObject = ht[o] as UnityEngine.Object;

                    if (dependenciesMap.ContainsKey(unityObject))
                        injections[o] = dependenciesMap[unityObject].Id;
                }
            }

            foreach (object o in injections.Keys)
                ht[o] = injections[o];
        }
    }
}