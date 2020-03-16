namespace Worldreaver.UniUI
{
    public interface IAffect
    {
        /// <summary>
        /// default scale
        /// </summary>
        UnityEngine.Vector3 DefaultScale { get; set; }

        /// <summary>
        /// is affect to self
        /// </summary>
        bool IsAffectToSelf { get; }

        /// <summary>
        /// object affect if IsAffectToSelf equal false.
        /// </summary>
        UnityEngine.RectTransform AffectObject { get; }
    }
}