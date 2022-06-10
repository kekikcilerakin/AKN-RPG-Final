using UnityEngine;

[CreateAssetMenu(fileName = "AKN_Tooltip_", menuName = "AKN RPG Final/Tooltip", order = 3)]
public class AKN_TooltipSO : ScriptableObject
{
    public int ID;

    public string header;
    public string content;

    public bool button;
    public string buttonText;
}