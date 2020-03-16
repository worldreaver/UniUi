using ExtraUniRx.Bindings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple binding to bind the text of an input field to a text field
/// </summary>
public class SetupTMP_TextBinding : MonoBehaviour
{
    public InputField InputElement;
    public TextMeshProUGUI TextElement;

    void Start()
    {
        TextElement.BindTextTo(() => InputElement.text);
    }
}
