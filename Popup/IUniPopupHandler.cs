namespace Worldreaver.UniUI
{
    public interface IUniPopupHandler
    {
        /// <summary>
        /// game object
        /// </summary>
        UnityEngine.GameObject ThisGameObject { get; }

        /// <summary>
        /// canvas contains popup
        /// </summary>
        UnityEngine.Canvas Canvas { get; }

        /// <summary>
        /// active popup
        /// </summary>
        void Show();

        /// <summary>
        /// deactive popup
        /// </summary>
        void Close();

        /// <summary>
        /// update sorting order of cavas contains popup
        /// </summary>
        /// <param name="sortingOrder"></param>
        void UpdateSortingOrder(int sortingOrder);
    }
}