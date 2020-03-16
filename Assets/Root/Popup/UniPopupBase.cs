#pragma warning disable 0649
using UnityEngine;

namespace UnityModule.UniPopup
{
    public class UniPopupBase : MonoBehaviour, IUniPopupHandler
    {
        [SerializeField] private Canvas canvas;

        #region Implementation of IUniPopupHandler

        public Canvas Canvas => canvas;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void UpdateSortingOrder(int sortingOrder)
        {
            canvas.sortingOrder = sortingOrder;
        }

        #endregion
    }
}