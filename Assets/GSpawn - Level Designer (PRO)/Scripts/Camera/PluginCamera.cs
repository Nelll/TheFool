#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GSpawn_Pro
{
    public class PluginCamera
    {
        public static Camera camera { get { return SceneView.lastActiveSceneView.camera; } }
    }
}
#endif