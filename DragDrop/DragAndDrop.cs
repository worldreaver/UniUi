namespace Worldreaver.UniUI
{
    public delegate void DragEvent(IDraggable item);

    public static class DragAndDrop
    {
        public static IDraggable dragItem;
        public static bool enableDrag = true;
        public static event DragEvent OnStartDragItem; // drag start event
        public static event DragEvent OnEndDragItem; // drag end event

        public static void InvokeOnStartDrag(IDraggable item)
        {
            OnStartDragItem?.Invoke(item);
        }

        public static void InvokeOnEndDrag(IDraggable item)
        {
            OnEndDragItem?.Invoke(item);
        }
    }
}