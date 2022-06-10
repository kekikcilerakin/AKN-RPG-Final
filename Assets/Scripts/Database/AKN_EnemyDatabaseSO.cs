using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AKN_DB_Enemy", menuName = "AKN RPG Final/Database/Character/AKN_EnemyDatabase", order = 0)]
public class AKN_EnemyDatabaseSO : ScriptableObject
{
    public List<AKN_EnemySO> plainEnemies;
    public List<AKN_EnemySO> roadEnemies;
    public List<AKN_EnemySO> desertEnemies;
    public List<AKN_EnemySO> forestEnemies;
    public List<AKN_EnemySO> mountainEnemies;

}