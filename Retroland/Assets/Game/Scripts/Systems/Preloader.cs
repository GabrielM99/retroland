#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems
{
    /*
    Preloads important resources.
    @Author: Gabriel de Mello.
    */
    [InitializeOnLoad]
    public static class Preloader
    {
        private const string PreloadScene = "Preload";

        private static bool IsPreloaded { get; set; }
        private static string PreviousScene { get; set; }

        static Preloader()
        {
            Preload();
        }

        /*
        Preloads by loading the preload scene before our current active scene.
        */
        private static void Preload()
        {
            //Iterates over all open scenes.
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                string path = scene.path;

                //If the preload scene is open.
                if (Path.GetFileNameWithoutExtension(path) == PreloadScene)
                {
                    //Sets the current active scene as the previous scene.
                    PreviousScene = EditorSceneManager.GetActiveScene().name;
                    //When a scene is loaded.
                    SceneManager.sceneLoaded += (scene, mode) =>
                    {
                        //If we haven't preloaded and the loaded scene is the preload scene.
                        if (!IsPreloaded && scene.name == PreloadScene)
                        {
                            //We can go back to the previous scene.
                            SceneManager.LoadScene(PreviousScene);
                            //The preload in complete.
                            IsPreloaded = true;
                        }
                    };
                    //Loads the preload scene when clicking the play button.
                    EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                    break;
                }
            }
        }
    }
}
#endif