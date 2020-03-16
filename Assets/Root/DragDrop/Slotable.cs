using UnityEngine;
using UnityEngine.EventSystems;

namespace Worldreaver.UniUI
{
    public enum SlotType
    {
        Swap, // Items will be swapped between any slots
        DropOnly, // Item will be dropped into slot
        DragOnly // Item will be dragged from this slot
    }

    public enum TriggerType
    {
        DropRequest, // Request for item drop from one slot to another
        DropEventEnd, // Drop event completed
        ItemAdded, // Item manualy added into slot
        ItemWillBeDestroyed // Called just before item will be destroyed
    }

    public class Slotable : MonoBehaviour, ISlot, IDropHandler
    {
        [SerializeField] private SlotType slotType = SlotType.Swap;
        [SerializeField] private bool useSelf = false; // equal false item form slot will be cloned on drag start
        public SlotType SlotType => slotType;
        public bool UseSelf => useSelf;
        public IDraggable Item { get; private set; }

        protected virtual void OnEnable()
        {
            DragAndDrop.OnStartDragItem += OnStartDragItem;
            DragAndDrop.OnEndDragItem += OnEndDragItem;
            Item = GetComponentInChildren<IDraggable>();
        }

        protected virtual void OnDisable()
        {
            DragAndDrop.OnStartDragItem -= OnStartDragItem;
            DragAndDrop.OnEndDragItem -= OnEndDragItem;
        }

        public virtual void OnStartDragItem(IDraggable item)
        {
            if (Item != null)
            {
                Item.UpdateRaycast(false);
//                if (Item == item)
//                {
//                    switch (slotType)
//                    {
//                        case SlotType.DropOnly:
//                            //todo
//                            break;
//                    }
//                }
            }
        }

        public virtual void OnEndDragItem(IDraggable item)
        {
            Item?.UpdateRaycast(true);
        }

        public virtual void PlaceItem(IDraggable item)
        {
            if (item != null)
            {
                DestroyItem();
                if (item.RootSlot.UseSelf)
                {
                    
                }
            }
        }

        protected virtual void DestroyItem()
        {
            if (Item != null)
            {
                Destroy(Item.ThisGameObject);
            }

            Item = null;
        }

        public virtual void AddItem(IDraggable item)
        {
            if (item != null)
            {
                PlaceItem(item);
            }
        }

        public virtual void RemoveItem()
        {
            DestroyItem();
        }

        public virtual void SwapItems(ISlot slotA, ISlot slotB)
        {
            if (slotA != null && slotB != null)
            {
                IDraggable itemA = slotB.Item;
                IDraggable itemB = slotB.Item;
            }
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
        }
    }
}