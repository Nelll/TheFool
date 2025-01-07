#if UNITY_EDITOR
namespace GSpawn_Pro
{
    public static class UIRefresh
    {
        public static void refreshShortcutToolTips()
        {
            PluginInspectorUI.instance.refresh();
        }
    }
}
#endif