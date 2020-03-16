using TMPro;
using UnityEngine;


namespace Worldreaver.UniUI
{
    public class UniProgressBarTMP : UniProgressBar
    {
#pragma warning disable 0649
        [SerializeField] private TextMeshProUGUI text;
#pragma warning restore 0649
        public TextMeshProUGUI Text
        {
            get => text;
            set => text = value;
        }
    }
}