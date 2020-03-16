namespace Worldreaver.UniUI
{
    public interface IToggleAffect : IAffect
    {
        bool IsExpan { get; }
        UnityEngine.UI.LayoutElement LayoutElement { get; }
        UnityEngine.Vector2 ValueExpand { get; }
        UnityEngine.Vector2 ValueFlexible { get; }
        UnityEngine.Vector3 SelectedScale { get; }
        UnityEngine.Vector3 UnSelectedScale { get; }
    }
}