using UnityEditor;
using UnityEngine;
using AkaitoAi.AudioSystem;

namespace AkaitoAi
{
    public class CreatePrefabs : Editor
    {
        [MenuItem("AkaitoAi/Setup/Tweaks")]
        public static void CreateTweaks()
        {
            // Check if already exists in the scene
            if (GameObject.FindObjectOfType<Tweaks>())
            {
                EditorUtility.DisplayDialog("Scene has Tweaks already!", "Scene has Tweaks already!", "Close");
                Selection.activeGameObject = GameObject.FindObjectOfType<Tweaks>().gameObject;
            }
            else
            {
                // Ensure the path includes ".prefab"
                string prefabPath = "Assets/_Editor/Prefabs/Tweaks.prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                // Check if the prefab is found
                if (prefab != null)
                {
                    // Instantiate the prefab in the scene
                    GameObject tweaks = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    tweaks.transform.position = Vector3.zero;
                    tweaks.transform.rotation = Quaternion.identity;
                    tweaks.name = "Tweaks";
                    Selection.activeGameObject = tweaks;
                }
                else
                {
                    // Log an error if the prefab is not found
                    Debug.LogError("Prefab not found at path: " + prefabPath);
                }
            }
        }

        [MenuItem("AkaitoAi/Setup/Profiler")]
        public static void CreateProfiler()
        {
            // Check if already exists in the scene
            if (GameObject.FindObjectOfType<ProfileReader>())
            {
                EditorUtility.DisplayDialog("Scene has Profiler already!", "Scene has Profiler already!", "Close");
                Selection.activeGameObject = GameObject.FindObjectOfType<ProfileReader>().gameObject;
            }
            else
            {
                // Ensure the path includes ".prefab"
                string prefabPath = "Assets/_Editor/Prefabs/Profiler.prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                // Check if the prefab is found
                if (prefab != null)
                {
                    // Instantiate the prefab in the scene
                    GameObject profiler = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    profiler.transform.position = Vector3.zero;
                    profiler.transform.rotation = Quaternion.identity;
                    profiler.name = "Profiler";
                    Selection.activeGameObject = profiler;
                }
                else
                {
                    // Log an error if the prefab is not found
                    Debug.LogError("Prefab not found at path: " + prefabPath);
                }
            }
        }

        [MenuItem("AkaitoAi/Misc/InEditorGridGenetator")]
        public static void CreateInEditorGridGenetator()
        {
            // Ensure the path includes ".prefab"
            string prefabPath = "Assets/_Editor/Prefabs/GridGeneratorEditor.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // Check if the prefab is found
            if (prefab != null)
            {
                // Instantiate the prefab in the scene
                GameObject gridGenerator = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                gridGenerator.transform.position = Vector3.zero;
                gridGenerator.transform.rotation = Quaternion.identity;
                gridGenerator.name = "GridGeneratorEditor";
                Selection.activeGameObject = gridGenerator;
            }
            else
            {
                // Log an error if the prefab is not found
                Debug.LogError("Prefab not found at path: " + prefabPath);
            }
        }

        [MenuItem("AkaitoAi/Misc/RuntimeGridGenetator")]
        public static void CreateRuntimeGridGenetator()
        {
            // Ensure the path includes ".prefab"
            string prefabPath = "Assets/_Editor/Prefabs/GridGenerator.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // Check if the prefab is found
            if (prefab != null)
            {
                // Instantiate the prefab in the scene
                GameObject gridGenerator = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                gridGenerator.transform.position = Vector3.zero;
                gridGenerator.transform.rotation = Quaternion.identity;
                gridGenerator.name = "GridGenerator";
                Selection.activeGameObject = gridGenerator;
            }
            else
            {
                // Log an error if the prefab is not found
                Debug.LogError("Prefab not found at path: " + prefabPath);
            }
        }

        

        [MenuItem("AkaitoAi/Misc/RuntimeMeshCombine")]
        public static void CreateRuntimeMeshCombine()
        {
            // Ensure the path includes ".prefab"
            string prefabPath = "Assets/_Editor/Prefabs/RuntimeMeshCombine.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // Check if the prefab is found
            if (prefab != null)
            {
                // Instantiate the prefab in the scene
                GameObject runtimeMeshCombine = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                runtimeMeshCombine.transform.position = Vector3.zero;
                runtimeMeshCombine.transform.rotation = Quaternion.identity;
                runtimeMeshCombine.name = "RuntimeMeshCombine";
                Selection.activeGameObject = runtimeMeshCombine;
            }
            else
            {
                // Log an error if the prefab is not found
                Debug.LogError("Prefab not found at path: " + prefabPath);
            }
        }

        [MenuItem("AkaitoAi/Misc/LineRenderer Route System")]
        public static void CreateLineRendererRouteSystem()
        {
            // Ensure the path includes ".prefab"
            string prefabPath = "Assets/_Editor/Prefabs/LineRendererRoute.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // Check if the prefab is found
            if (prefab != null)
            {
                // Instantiate the prefab in the scene
                GameObject lineRendererRoute = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                lineRendererRoute.transform.position = Vector3.zero;
                lineRendererRoute.transform.rotation = Quaternion.identity;
                lineRendererRoute.name = "LineRendererRoute";
                Selection.activeGameObject = lineRendererRoute;
            }
            else
            {
                // Log an error if the prefab is not found
                Debug.LogError("Prefab not found at path: " + prefabPath);
            }
        }

        [MenuItem("AkaitoAi/Setup/AudioManager")]
        public static void CreateAudioManager()
        {
            // Check if already exists in the scene
            if (GameObject.FindObjectOfType<AudioManager>())
            {
                EditorUtility.DisplayDialog("Scene has AudioManager already!", "Scene has AudioManager already!", "Close");
                Selection.activeGameObject = GameObject.FindObjectOfType<AudioManager>().gameObject;
            }
            else
            {
                // Ensure the path includes ".prefab"
                string prefabPath = "Assets/_Editor/Prefabs/AudioManager.prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                // Check if the prefab is found
                if (prefab != null)
                {
                    // Instantiate the prefab in the scene
                    GameObject audioManager = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    audioManager.transform.position = Vector3.zero;
                    audioManager.transform.rotation = Quaternion.identity;
                    audioManager.name = "AudioManager";
                    Selection.activeGameObject = audioManager;
                }
                else
                {
                    // Log an error if the prefab is not found
                    Debug.LogError("Prefab not found at path: " + prefabPath);
                }
            }
        }
    }
}
