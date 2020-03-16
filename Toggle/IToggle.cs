namespace Worldreaver.UniUI
{
    public interface IToggle : IUniDynamic
    {
        /// <summary>
        /// use change sprite when switch on off
        /// </summary>
        bool IsSwitchSprite { get; }

        /// <summary>
        /// list object active when IsOn equal true and deactive when IsOn equal false
        /// </summary>
        System.Collections.Generic.List<UnityEngine.RectTransform> ActiveObjects { get; set; }

        /// <summary>
        /// list object active when IsOn equal false and deactive when IsOn equal true
        /// </summary>
        System.Collections.Generic.List<UnityEngine.RectTransform> DeactiveObjects { get; set; }

        /// <summary>
        /// sprite of toggle when IsOn equal true
        /// </summary>
        UnityEngine.Sprite ActiveSprite { get; set; }

        /// <summary>
        /// sprite of toggle when IsOn equal false
        /// </summary>
        UnityEngine.Sprite DeactiveSprite { get; set; }

        /// <summary>
        /// target graphic image
        /// </summary>
        UnityEngine.UI.Image TargetGraphicImage { get; }

        /// <summary>
        /// graphic image
        /// </summary>
        UnityEngine.UI.Image GraphicImage { get; }
    }
}