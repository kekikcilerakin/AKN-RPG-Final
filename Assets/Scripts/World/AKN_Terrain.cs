using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TerrainType{None, Forest, Mountain, Plains, Desert, Road}

public class AKN_Terrain : MonoBehaviour
{
    public TerrainType terrainType;
    public UnityEvent activityEvent = new UnityEvent();

    public List<AKN_Activity> activities = new List<AKN_Activity>();


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player entered " + terrainType);
        AKN_GameController.instance.playerController.currentTerrain = terrainType;
        AKN_GameController.instance.UIController.CreateActivityButtons(activities);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player left " + terrainType);
        AKN_GameController.instance.playerController.currentTerrain = TerrainType.None;
        AKN_GameController.instance.UIController.DestroyActivityButtons();
    }

}