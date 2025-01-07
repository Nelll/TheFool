#if UNITY_EDITOR
namespace GSpawn_Pro
{
    public class DelayedTreeViewItemRename<TData> : EditorUpdateAction
        where TData : IUIItemStateProvider
    {
        private TreeViewItem<TData> _item;

        public DelayedTreeViewItemRename(TreeViewItem<TData> item)
        {
            _item = item;
        }

        protected override void execute()
        {
            _item.beingDelayedRename();
        }
    }
}
#endif