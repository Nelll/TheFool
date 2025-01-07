#if UNITY_EDITOR
using UnityEngine;

namespace GSpawn_Pro
{
    public class TerrainObjectOverlapFilter
    {
        public bool filterObject(GameObject gameObject)
        {
            return true;
        }
    }
}
#endif