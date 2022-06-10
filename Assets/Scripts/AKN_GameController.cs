using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {Free, Combat, Hunt}

public class AKN_GameController : MonoBehaviour
{
    public static AKN_GameController instance = null;
    
    public GameState gameState;

    public AKN_PlayerController playerController;
    public AKN_DialogueController dialogueController;
    public AKN_UIController UIController;
    public AKN_InventoryController inventoryController;
    public AKN_CombatInitializer combatInitializer;
    public AKN_DatabaseController databaseController;
    [HideInInspector] public AKN_CombatController combatController;

    public Camera mainCamera;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != instance)
        {
            Destroy (gameObject);
        }
    }

    private void Start()
    {
        if (!UIController.dialoguePanel.gameObject.activeInHierarchy)
            UIController.dialoguePanel.gameObject.SetActive(true);
            
            mainCamera = Camera.main;
    }

    public IEnumerator LoadScene(string _scene)
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone)
            yield return null;

        yield return new WaitForEndOfFrame();
        mainCamera.gameObject.SetActive(false);
        UIController.interactPanel.SetActive(false);
        UIController.activityPanel.SetActive(false);
    }

    public void AKN_LOG(string _string, bool isBold)
    {
        if (isBold)
            Debug.Log("<b><color=white>" + _string + "</color></b>");
        else
            Debug.Log("<b><color=white>" + _string + "</color>");
    }

    public void AKN_LOGERROR(string _string, bool isBold)
    {
        if (isBold)
            Debug.LogError("<b><color=white>" + _string + "</color></b>");
        else
            Debug.LogError("<b><color=white>" + _string + "</color>");
    }
}
