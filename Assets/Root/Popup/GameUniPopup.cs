#pragma warning disable 0649
using UniRx;
using UnityEngine;

namespace UnityModule.UniPopup
{
    public partial class GameUniPopup : MonoBehaviour
    {
        private UniPopup _uniPopup;
        [SerializeField] private Canvas canvas;

        /// <summary>
        /// initialize
        /// </summary>
        public void Initialized()
        {
            _uniPopup = new UniPopup();
            _uniPopup.SortingOrder.Subscribe(_ => canvas.sortingOrder = _);
        }
    }
}