#if UNITY_EDITOR
namespace GSpawn_Pro
{
    public interface IUIItemStateProvider
    {
        bool            uiSelected      { get; set; }
        CopyPasteMode   uiCopyPasteMode { get; set; }
        PluginGuid      guid            { get; }
    }
}
#endif