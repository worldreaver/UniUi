using UnityEngine;

namespace Worldreaver.UniUI
{
    public interface IDraggable
    {
        RectTransform CanvasTransform { get; }
        bool EnableDrag { get; }
        int Id { get; }
        ISlot RootSlot { get; }
        void UpdateRaycast(bool enable);
        GameObject ThisGameObject { get; }
    }
}