using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AKN_DB_Tooltip", menuName = "AKN RPG Final/Database/Tooltip/AKN_TooltipDatabase", order = 0)]
public class AKN_TooltipDatabaseSO : ScriptableObject
{
    public List<AKN_TooltipSO> tooltips = new List<AKN_TooltipSO>();
}