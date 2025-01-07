#if UNITY_EDITOR
namespace GSpawn_Pro
{
    public interface IPluginCommand
    {
        void enter  ();
        void exit   ();
    }
}
#endif