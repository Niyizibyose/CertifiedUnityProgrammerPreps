using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UiTextModifier: MonoBehaviour
{
    [SerializeField] private Text text;

    /// <summary>
    /// <para>
    /// Changes the text of the variable of type Text.
    /// </para>
    /// </summary>
    /// <param name="newNumber"></param>
    public void UpdateText(float newNumber)
    {
        text.text = newNumber.ToString(CultureInfo.InvariantCulture);
    }
}
