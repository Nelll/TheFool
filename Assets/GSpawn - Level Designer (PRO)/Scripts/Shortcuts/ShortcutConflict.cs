#if UNITY_EDITOR
using UnityEngine;
using System;

namespace GSpawn_Pro
{
    [Serializable]
    public class ShortcutConflict
    {
        [SerializeField]
        public string categoryName = string.Empty;
        [SerializeField]
        public string shortcutName = string.Empty;
    }
}
#endif