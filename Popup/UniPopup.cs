using System.Collections.Generic;
using UniRx;

namespace UnityModule.UniPopup
{
    public class UniPopup
    {
        /// <summary>
        /// stack contains all popup (LIFO)
        /// </summary>
        private readonly Stack<IUniPopupHandler> _stacks = new Stack<IUniPopupHandler>();

        /// <summary>
        /// subjectproperty control sorting order of root canvas popup
        /// </summary>
        public SubjectProperty<int> SortingOrder { get; } = new SubjectProperty<int>();

        /// <summary>
        /// hide popup in top stack
        /// </summary>
        public void Hide()
        {
            _stacks.Pop().Hide();
            var orderOfBoard = 0;
            if (_stacks.Count > 1)
            {
                var stop = _stacks.Peek();
                orderOfBoard = stop.Canvas.sortingOrder - 10;
            }

            SortingOrder.OnNext(orderOfBoard);
        }

        /// <summary>
        /// hide all popup in top stack
        /// </summary>
        public void HideAll()
        {
            var count = _stacks.Count;
            for (int i = 0; i < count; i++)
            {
                _stacks.Pop().Hide();
            }

            SortingOrder.OnNext(0);
        }

        /// <summary>
        /// show popup
        /// </summary>
        /// <param name="uniPopupHandler">popup wanna show</param>
        public void Show(IUniPopupHandler uniPopupHandler)
        {
            var lastOrder = 0;
            if (_stacks.Count > 0)
            {
                var top = _stacks.Peek();
                lastOrder = top.Canvas.sortingOrder;
            }

            uniPopupHandler.UpdateSortingOrder(lastOrder + 10);
            SortingOrder.OnNext(lastOrder);
            _stacks.Push(uniPopupHandler);
        }

        /// <summary>
        /// show popup and hide previous popup
        /// </summary>
        /// <param name="uniPopupHandler">popup wanna show</param>
        /// <param name="number">number previous popup wanna hide</param>
        public void Show(IUniPopupHandler uniPopupHandler,
            int number)
        {
            if (number > _stacks.Count)
            {
                number = _stacks.Count;
            }

            for (int i = 0; i < number; i++)
            {
                var p = _stacks.Pop();
                p.Hide();
            }

            Show(uniPopupHandler);
        }

        /// <summary>
        /// show popup and hide all previous popup
        /// </summary>
        /// <param name="uniPopupHandler">popup wanna show</param>
        public void ShowAndHideAll(IUniPopupHandler uniPopupHandler)
        {
            Show(uniPopupHandler, _stacks.Count);
        }
    }
}