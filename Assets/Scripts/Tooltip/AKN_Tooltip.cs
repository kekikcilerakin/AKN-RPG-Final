using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class AKN_Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public Button button;
    public TextMeshProUGUI buttonField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public void SetTooltip(AKN_TooltipSO tooltipSO)
    {
        if (string.IsNullOrEmpty(tooltipSO.header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = tooltipSO.header;
        }

        contentField.text = tooltipSO.content;

        if (!tooltipSO.button)
        {
            button.gameObject.SetActive(false);
        }
        else
        {
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() => AKN_GameController.instance.UIController.HideTooltip());
            buttonField.text = tooltipSO.buttonText;

        }

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

}
