namespace Worldreaver.UniUI
{
    public interface ISlot
    {
        SlotType SlotType { get; }
        bool UseSelf { get; }
        IDraggable Item { get; }
        void OnStartDragItem(IDraggable item);
        void OnEndDragItem(IDraggable item);
        void PlaceItem(IDraggable item);
        void AddItem(IDraggable item);
        void RemoveItem();
        void SwapItems(ISlot slotA, ISlot slotB);
    }
}