using System.Linq;
using TMPro;
using UnityEngine;

namespace Worldreaver.UniUI
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class UniButtonTMP : UniButton, IUniTMP
    {
        [SerializeField] private TextMeshProUGUI text;

        #region Implementation of IUniTMP

        public TextMeshProUGUI Text
        {
            get
            {
#if UNITY_EDITOR
                FindChildText();
#endif
                return text;
            }
        }

        #endregion

        #region Overrides of Selectable

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            //use Invoke to call method to avoid warning "Send Message cannot be called during Awake, or OnValidate...
            Invoke(nameof(FindChildText), 0.1f);
        }

        private void CreateChildTextTMP()
        {
            var childText = new GameObject("Text");
            childText.transform.SetParent(transform, false);
            text = childText.AddComponent<TextMeshProUGUI>();
        }

        private void FindChildText()
        {
            if (text != null) return;
            text = GetComponentsInChildren<TextMeshProUGUI>(true).FirstOrDefault(_ => _.gameObject != gameObject);
            if (text != null) return;
            CreateChildTextTMP();
        }
#endif

        #endregion
    }
}