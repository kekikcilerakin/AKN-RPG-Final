using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKN_CombatInitializer : MonoBehaviour
{
    public AKN_PlayerSO player;
    public TerrainType currentTerrain;
    
    public int friendlyCountToSend, enemyCountToSend;

    private void Start()
    {
        player = AKN_GameController.instance.playerController.player;
        currentTerrain = AKN_GameController.instance.playerController.currentTerrain;
    }

    public void InitializeCombat()
    {   
        
        AKN_GameController.instance.playerController.isTraveling = false;
        AKN_GameController.instance.gameState = GameState.Combat;

        
        
        enemyCountToSend = 3;
        //!enemyCountToSend = Random.Range(1, 6);
        StartCoroutine(AKN_GameController.instance.LoadScene("CombatScene"));
    }
}
