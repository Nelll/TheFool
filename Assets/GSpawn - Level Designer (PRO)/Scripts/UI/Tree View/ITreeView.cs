#if UNITY_EDITOR
namespace GSpawn_Pro
{
    public interface ITreeView
    {
        int             numSelectedItems        { get; }
        int             dragAndDropInitiatorId  { get; }
        System.Object   dragAndDropData         { get; }
        bool            listModeEnabled         { get; set; }
    }
}
#endif