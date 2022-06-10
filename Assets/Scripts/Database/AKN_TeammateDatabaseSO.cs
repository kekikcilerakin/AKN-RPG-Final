using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AKN_DB_Teammate", menuName = "AKN RPG Final/Database/Character/AKN_TeammateDatabase", order = 0)]
public class AKN_TeammateDatabaseSO : ScriptableObject
{
    public List<AKN_TeammateSO> teammates;
}