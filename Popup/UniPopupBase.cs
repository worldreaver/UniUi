#pragma warning disable 0649
using UnityEngine;

namespace Worldreaver.UniUI
{
    public class UniPopupBase : MonoBehaviour, IUniPopupHandler
    {
        [SerializeField] private Canvas canvas;

        #region Implementation of IUniPopupHandler

        public GameObject ThisGameObject => gameObject;
        public Canvas Canvas => canvas;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
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