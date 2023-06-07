#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class Builder
    {
        private const string ClientPath = "Builds";
        private const string ServerPath = "Builds/Server";

        [MenuItem("Build/Build All")]
        public static void BuildAll()
        {
            BuildClient();
            BuildServer();
        }

        [MenuItem("Build/Build Client")]
        public static void BuildClient()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = $"{Path.Combine(ClientPath, Application.productName)} Client";
            buildPlayerOptions.target = BuildTarget.WebGL;
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select((scene) => scene.path).ToArray();
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        [MenuItem("Build/Build Server")]
        public static void BuildServer()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = $"{Path.Combine(ServerPath, Application.productName)} Server.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select((scene) => scene.path).ToArray();
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
    }
}
#endif