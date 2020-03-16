using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Worldreaver.UniUI
{
    [RequireComponent(typeof(Image))]
    public class DraggableItem : MonoBehaviour, IDraggable, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeReference] private Slotable _rootSlot;
        #pragma warning disable 0649
        [SerializeField] private Image image;
        #pragma warning restore 0649

        public RectTransform CanvasTransform { get; private set; }
        public bool EnableDrag => DragAndDrop.enableDrag;
        public int Id { get; }
        public ISlot RootSlot => _rootSlot;
        public GameObject ThisGameObject => gameObject;


        public virtual void Initialized(RectTransform canvasTransform)
        {
            CanvasTransform = canvasTransform;
        }

        public void UpdateRootSlot(ISlot newRootSlot)
        {
            _rootSlot = (Slotable) newRootSlot;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (EnableDrag)
            {
                DragAndDrop.dragItem = this;
                //todo
                image.raycastTarget = false;
                DragAndDrop.InvokeOnStartDrag(this);
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            //todo move
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            Release();
        }

        protected virtual void Release()
        {
            DragAndDrop.InvokeOnEndDrag(this);
            DragAndDrop.dragItem = null;
            _rootSlot = null;
        }

        public void UpdateRaycast(bool enable)
        {
            if (image != null)
            {
                image.raycastTarget = enable;
            }
        }

        protected virtual void OnDisable()
        {
            Release();
        }
    }
}