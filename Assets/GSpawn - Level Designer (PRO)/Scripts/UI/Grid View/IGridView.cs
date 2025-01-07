#if UNITY_EDITOR
namespace GSpawn_Pro
{
    public interface IGridView
    {
        int             dragAndDropInitiatorId  { get; }
        System.Object   dragAndDropData         { get; }
    }
}
#endif