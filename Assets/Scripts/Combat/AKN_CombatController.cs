using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CombatState
{
    CombatStarting,

    Won,
    Lost,

    FriendlyTurnStarted,
    FriendlyTurnEnded,
    FriendlyAttackStarted,
    FriendlyAttackEnded,
    FriendlyMoveStarted,
    FriendlyMoveEnded,

    EnemyTurnStarted,
    EnemyTurnSkipped,
    EnemyTurnEnded,
    EnemyAttackStarted,
    EnemyAttackEnded,
    EnemyMoveStarted, 
    EnemyMoveEnded, 
    EnemyPrepareForMoveStarted,
    EnemyPrepareForMoveEnded,
    EnemySearchUnitStarted,
    EnemySearchUnitEnded
}

public class AKN_CombatController : MonoBehaviour
{
    private AKN_PlayerSO player; // Getting Player scriptableObject so we can spawn it
    public CombatState combatState; // Defining Combat States
    public int friendlyCount, enemyCount; // Unit count to spawn as many

    private GameObject unitMoveGO, unitAttackGO; // Selected Unit's Move and Attack gameObjects
    private float unitMoveRadius, attackRadius; // Determine Unit's Move and Attack Radius as in Hex Tile Size
    private float hexSize = 12.5f; // Hex Tile Size

    public AKN_Unit selectedUnit; // Selected Unit (current turn)
    public GameObject selectedUnitGO; // Selected Unit's gameObject (current turn)

    private int friendlyTurnOrder = 0, enemyTurnOrder = 0; // Defining Turn Order

    [HideInInspector] public List<GameObject> friendlyUnitsInCombat = new List<GameObject>(); // List that holds Friendly Units currently in Combat
    [HideInInspector] public List<GameObject> enemyUnitsInCombat = new List<GameObject>(); // List that holds Enemy Units currently in Combat

    [HideInInspector] public List<Transform> friendlySpawnPoints = new List<Transform>(); // List that holds Friendly Spawn Points
    [HideInInspector] public List<Transform> enemySpawnPoints = new List<Transform>(); // List that holds Enemy Spawn Points
    
    private List<int> usedFriendlySpawnPoints = new List<int>(); // List that holds full Friendly Spawn Points
    private List<int> usedEnemySpawnPoints = new List<int>(); // List that holds full Enemy Spawn Points
    
    private List<AKN_TeammateSO> teammates = new List<AKN_TeammateSO>(); // List that holds selected Teammates for Combat
    private List<AKN_EnemySO> enemies = new List<AKN_EnemySO>(); // List that holds selected Enemies for Combat

    [Header("DRAG from SCENE")]
    public AKN_TileGenerator tileGenerator; // To generate combat tiles
    public AKN_CombatUIController combatUIController; // To edit combat UI

    [Header("DRAG from ASSETS")]
    [SerializeField] private GameObject unitPrefab; // To spawn Units
    
    private void Start()
    {
        if (unitPrefab == null)
            AKN_GameController.instance.AKN_LOGERROR("Unit Prefab in " + gameObject.name + " is NULL", true);

        combatState = CombatState.CombatStarting;

        player = AKN_GameController.instance.combatInitializer.player;
        AKN_GameController.instance.combatController = this; // Set CombatController in GameController to this

        enemyCount = AKN_GameController.instance.combatInitializer.enemyCountToSend; // Set Enemy Count

        tileGenerator.GenerateTiles(); // Generate Tiles

        AssignUnits(AKN_GameController.instance.combatInitializer.currentTerrain); // Assign Units

        SpawnPlayer(); //Spawn Player
        SpawnTeammates(); // Spawn Teammates
        SpawnEnemies(); // Spawn Enemies

        StartCombat(); // Start Combat
    }

    private void AssignUnits(TerrainType _currentTerrain)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            enemies.Add(AKN_GameController.instance.databaseController.GetRandomEnemy(_currentTerrain));
        }

        for (int i = 0; i < friendlyCount - 1; i++)
        {
            teammates.Add(AKN_GameController.instance.databaseController.GetRandomTeammate());
        }
    }

    private void SpawnPlayer()
    {
        int rnd = Random.Range(0, friendlySpawnPoints.Count);
        usedFriendlySpawnPoints.Add(rnd);

        GameObject playerGO = Instantiate(unitPrefab, friendlySpawnPoints[rnd].transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(playerGO, SceneManager.GetSceneByName("CombatScene"));
        playerGO.transform.SetParent(GameObject.Find("Friendly Units").transform);
        playerGO.name = player.name;
        playerGO.GetComponent<AKN_Unit>().unit = player;
        playerGO.GetComponent<AKN_Unit>().team = Team.Friendly;
        friendlyUnitsInCombat.Add(playerGO);
    }

    private void SpawnTeammates()
    {
        if (friendlyCount > 1)
        {
            int counter = 0;
            while (usedFriendlySpawnPoints.Count != friendlyCount)
            {
                int rnd = Random.Range(0, friendlySpawnPoints.Count);
                if (!usedFriendlySpawnPoints.Contains(rnd))
                {
                    usedFriendlySpawnPoints.Add(rnd);
                    AKN_TeammateSO teammate = Instantiate(teammates[counter]);
                    counter++;
                    GameObject teammateGO = Instantiate(unitPrefab, friendlySpawnPoints[rnd].transform.position, Quaternion.identity);
                    SceneManager.MoveGameObjectToScene(teammateGO, SceneManager.GetSceneByName("CombatScene"));
                    teammateGO.transform.SetParent(GameObject.Find("Friendly Units").transform);
                    teammateGO.name = teammate.name;
                    teammateGO.GetComponent<AKN_Unit>().unit = teammate;
                    teammateGO.GetComponent<AKN_Unit>().team = Team.Friendly;
                    friendlyUnitsInCombat.Add(teammateGO);
                }
            }
        }
    }

    private void SpawnEnemies()
    {
        int counter = 0;
        while (usedEnemySpawnPoints.Count != enemyCount)
        {
            int rnd = Random.Range(0, enemySpawnPoints.Count);
            
            if (!usedEnemySpawnPoints.Contains(rnd))
            {
                usedEnemySpawnPoints.Add(rnd);
                AKN_EnemySO enemy = Instantiate(enemies[counter]);
                counter++;
                GameObject enemyGO = Instantiate(unitPrefab, enemySpawnPoints[rnd].transform.position, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(enemyGO, SceneManager.GetSceneByName("CombatScene"));
                enemyGO.transform.SetParent(GameObject.Find("Enemy Units").transform);
                enemyGO.name = enemy.name;
                enemyGO.GetComponent<AKN_Unit>().unit = enemy;
                enemyGO.GetComponent<AKN_Unit>().team = Team.Enemy;
                enemyUnitsInCombat.Add(enemyGO);
            }
        }
    }

    private void StartCombat()
    {
        StartCoroutine(FriendlyTurn());
    }

    public void EndCombat()
    {
        if (combatState == CombatState.Won)
        {
            Debug.Log("You won.");
            AKN_GameController.instance.gameState = GameState.Free;
            SceneManager.UnloadSceneAsync("CombatScene");
            AKN_GameController.instance.mainCamera.gameObject.SetActive(true);
            AKN_GameController.instance.UIController.interactPanel.SetActive(true);
            AKN_GameController.instance.UIController.activityPanel.SetActive(true);
        }
        else if (combatState == CombatState.Lost)
        {
            Debug.Log("You lost.");
            AKN_GameController.instance.gameState = GameState.Free;
            SceneManager.UnloadSceneAsync("CombatScene");
            AKN_GameController.instance.mainCamera.gameObject.SetActive(true);
        }
    }

    public IEnumerator NextTurn()
    {
        if (combatState == CombatState.FriendlyMoveEnded || combatState == CombatState.FriendlyAttackEnded)
        {
            combatState = CombatState.FriendlyTurnEnded;
        }
        else if (combatState == CombatState.EnemyMoveEnded || combatState == CombatState.EnemyAttackEnded || combatState == CombatState.EnemyTurnSkipped)
        {
            combatState = CombatState.EnemyTurnEnded;
        }

        selectedUnit.moveableTiles.Clear();

        //Debug.Log("Turn ended for " + selectedUnitGO);

        if (combatState == CombatState.FriendlyTurnEnded)
        {
            enemyTurnOrder = 0;
            friendlyTurnOrder += 1;
            selectedUnit.combatIcon.GetComponent<SpriteRenderer>().color = Color.white;
            unitMoveGO.SetActive(false);
            unitAttackGO.SetActive(false);

            yield return null;

            if (friendlyTurnOrder >= friendlyCount)
            {
                StartCoroutine(EnemyTurn());
            }
            else
            {
                StartCoroutine(FriendlyTurn());
            }
        }
        else if (combatState == CombatState.EnemyTurnEnded)
        {
            friendlyTurnOrder = 0;
            enemyTurnOrder += 1;
            selectedUnit.combatIcon.GetComponent<SpriteRenderer>().color = Color.white;
            unitMoveGO.SetActive(false);
            unitAttackGO.SetActive(false);

            yield return null;

            if (enemyTurnOrder >= enemyCount)
            {
                StartCoroutine(FriendlyTurn());
            }
            else
            {
                StartCoroutine(EnemyTurn());
            }
        }
    }

    private IEnumerator FriendlyTurn()
    {
        combatState = CombatState.FriendlyTurnStarted; // Combat State is Friendly Turn

        yield return new WaitForSeconds(0.25f); // Turn start delay
        selectedUnitGO = friendlyUnitsInCombat[friendlyTurnOrder]; // Set Selected Unit gameObject according to Friendly Turn Order
        selectedUnit = selectedUnitGO.GetComponent<AKN_Unit>(); // Set Selected Unit according to Selected Unit gameObject
        
        //Debug.Log("Turn started for " + selectedUnitGO);

        combatUIController.SetUnitPanel(selectedUnit.unit);

        selectedUnitGO.GetComponent<AKN_Unit>().combatIcon.GetComponent<SpriteRenderer>().color = Color.yellow; // Change Selected Unit's color so it indicates selected

        unitMoveRadius = selectedUnit.unit.moveRadius * hexSize; // Set Unit's Move Radius as Hex Size
        attackRadius = selectedUnit.unit.attackRadius * hexSize; // Set Unit's Attack Radius as Hex Size

        unitMoveGO = selectedUnit.moveGO; // Set Unit Move GO as Selected Unit's Move gameObject.
        unitAttackGO = selectedUnit.attackGO; // Set Unit Attack GO as Selected Unit's Attack gameObject.

        unitMoveGO.transform.localScale = new Vector2(unitMoveRadius, unitMoveRadius); // Set Unit Move GO's size depending on it's Radius
        unitAttackGO.transform.localScale = new Vector2(attackRadius, attackRadius); // Set Unit Attack GO's size depending on it's Radius

        //selectedUnit.CheckForUnitsInRadius();

        yield return null; // Wait a frame so Selected Unit's Moveable Tiles list does not detect full tiles

        unitMoveGO.SetActive(true); // Enable Unit's Move GO
        unitAttackGO.SetActive(true); // Enable Unit's Attack GO

        /*if (selectedUnit.moveableTiles.Count == 0 || selectedUnit)
        {
            //StartCoroutine(NextTurn());
        }*/
    }

    private IEnumerator EnemyTurn()
    {
        combatState = CombatState.EnemyTurnStarted; // Combat State is Enemy Turn

        yield return new WaitForSeconds(0.75f); // Turn start delay
        selectedUnitGO = enemyUnitsInCombat[enemyTurnOrder]; // Set Selected Unit gameObject according to Enemy Turn Order
        selectedUnit = selectedUnitGO.GetComponent<AKN_Unit>(); // Set Selected Unit according to Selected Unit gameObject

        //Debug.Log("Turn started for " + selectedUnitGO);

        selectedUnitGO.GetComponent<AKN_Unit>().combatIcon.GetComponent<SpriteRenderer>().color = Color.yellow; // Change Selected Unit's color so it indicates selected

        unitMoveRadius = selectedUnit.unit.moveRadius * hexSize; // Set Unit's Move Radius as Hex Size
        attackRadius = selectedUnit.unit.attackRadius * hexSize; // Set Unit's Attack Radius as Hex Size

        unitMoveGO = selectedUnit.moveGO; // Set Unit Move GO as Selected Unit's Move gameObject.
        unitAttackGO = selectedUnit.attackGO; // Set Unit Attack GO as Selected Unit's Attack gameObject.

        unitMoveGO.transform.localScale = new Vector2(unitMoveRadius, unitMoveRadius); // Set Unit Move GO's size depending on it's Radius
        unitAttackGO.transform.localScale = new Vector2(attackRadius, attackRadius); // Set Unit Attack GO's size depending on it's Radius

        yield return null; // Wait a frame so Selected Unit's Moveable Tiles list does not detect full tiles

        unitMoveGO.SetActive(true); // Enable Unit's Move GO
        unitAttackGO.SetActive(true); // Enable Unit's Attack GO

        yield return new WaitForSeconds(0.1f); // Wait for second so Selected Unit can detect Attackable Tiles

        if (selectedUnit.attackableTiles.Count > 0)
        {
            int _rndTarget = Random.Range(0, selectedUnit.attackableTiles.Count);
            StartCoroutine(selectedUnit.AttackUnit(selectedUnit.attackableTiles[_rndTarget]));
        }
        else
        {
            selectedUnit.FindNearestUnit_AI();
            yield return null;
            selectedUnit.MoveTowardsNearestUnit_AI();
            //StartCoroutine(NextTurn());
        }
    }
}