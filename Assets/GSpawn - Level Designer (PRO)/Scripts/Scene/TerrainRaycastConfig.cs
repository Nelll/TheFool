#if UNITY_EDITOR
namespace GSpawn_Pro
{
    public struct TerrainRaycastConfig
    {
        public bool useInterpolatedNormal;

        public static readonly TerrainRaycastConfig defaultConfig = new TerrainRaycastConfig() { useInterpolatedNormal = true };
    }
}
#endif