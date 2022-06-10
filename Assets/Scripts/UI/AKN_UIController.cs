using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AKN_UIController : MonoBehaviour
{
    public Canvas mainCanvas;

    [Header("DIALOGUE")]
    public RectTransform dialoguePanel;
    public RectTransform sentencePanel;
    public RectTransform choicePanel;
    public Text sentenceText;

    [Header("TOOLTIP")]
    public AKN_Tooltip tooltip;

    [Header("TERRAIN INTERACT")]
    public GameObject interactPanel;
    public GameObject activityPanel;
    public Button interactButton;

    public List<GameObject> activityButtons = new List<GameObject>();

    public Sprite noneIcon;
    public Sprite forestIcon;
    public Sprite mountainIcon;

    [Header("DRAG from ASSETS")]
    public GameObject activityButtonPrefab;
    public AKN_TooltipSO blockedPathTooltipSO;

    private void Start()
    {
        if (activityButtonPrefab == null)
            AKN_GameController.instance.AKN_LOGERROR("Activity Button Prefab in " + gameObject.name + " is NULL", true);
    }

#region Tooltip
    public void ShowTooltip(AKN_TooltipSO tooltipSO)
    {
        tooltip.SetTooltip(tooltipSO);
        tooltip.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }
#endregion

    public void CreateActivityButtons(List<AKN_Activity> _activities)
    {
        for (int i = 0; i < _activities.Count; i++)
        {
            GameObject activityGO = Instantiate(activityButtonPrefab, transform.position, Quaternion.identity);
            activityGO.transform.SetParent(activityPanel.transform);
            activityGO.GetComponent<Button>().onClick.AddListener(_activities[i].DoActivity);
            activityGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _activities[i].SetActivityName();
            activityButtons.Add(activityGO);
        }
    }

    public void DestroyActivityButtons()
    {
        for (int i = 0; i < activityButtons.Count; i++)
        {
            Destroy(activityButtons[i]);
        }
        
        activityButtons.Clear();
    }
}
