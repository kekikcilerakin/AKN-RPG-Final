using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AKN_DatabaseController : MonoBehaviour
{
    public AKN_EnemyDatabaseSO enemyDatabase;
    public AKN_TeammateDatabaseSO teammateDatabase;
    public AKN_TooltipDatabaseSO tooltipDatabase;

    private void Start()
    {
        if (enemyDatabase == null)
            AKN_GameController.instance.AKN_LOGERROR("Enemy Database in " + gameObject.name + " is NULL", true);
            
        if (teammateDatabase == null)
            AKN_GameController.instance.AKN_LOGERROR("Teammate Database in " + gameObject.name + " is NULL", true);

        if (tooltipDatabase == null)
            AKN_GameController.instance.AKN_LOGERROR("Tooltip Database in " + gameObject.name + " is NULL", true);
    }

    public AKN_EnemySO GetRandomEnemy(TerrainType _terrainType)
    {
        if (_terrainType == TerrainType.Plains)
            return enemyDatabase.plainEnemies[Random.Range(0, enemyDatabase.plainEnemies.Count())];

        else if (_terrainType == TerrainType.Mountain)
            return enemyDatabase.mountainEnemies[Random.Range(0, enemyDatabase.mountainEnemies.Count())];

        else if (_terrainType == TerrainType.Forest)
            return enemyDatabase.forestEnemies[Random.Range(0, enemyDatabase.forestEnemies.Count())];

        else if (_terrainType == TerrainType.Road)
            return enemyDatabase.roadEnemies[Random.Range(0, enemyDatabase.roadEnemies.Count())];

        else if (_terrainType == TerrainType.Desert)
            return enemyDatabase.desertEnemies[Random.Range(0, enemyDatabase.desertEnemies.Count())];

        else
            return null;
    }

    public AKN_TeammateSO GetRandomTeammate()
    {
        return teammateDatabase.teammates[Random.Range(0, teammateDatabase.teammates.Count())];
    }

    public AKN_TooltipSO GetTooltipByID(int ID)
    {
        return tooltipDatabase.tooltips.FirstOrDefault(tooltip => tooltip.ID == ID);
    }
}
