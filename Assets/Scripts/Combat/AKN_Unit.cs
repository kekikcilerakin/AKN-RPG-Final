using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team{None, Friendly, Enemy}

public class AKN_Unit : MonoBehaviour
{
    public AKN_CharacterSO unit;
    public Team team;

    public GameObject moveGO;
    public GameObject attackGO;
    public SpriteRenderer combatIcon;

    public List<GameObject> moveableTiles;
    public List<GameObject> attackableTiles;

    private bool isTargetDead = false;
    private bool isKillUnitCompleted = false;
    private bool isHPBarActive = true;

    //AI
    public GameObject nearestUnit_AI;
    public List<GameObject> unitsInCombat_AI;
    public List<GameObject> tilesInPath_AI;
    public GameObject tileToMove_AI;
    public List<GameObject> hitList_AI;

    public Gradient healthGradient, armorGradient;  // Health red to green, Armor gray to gray

    [SerializeField] Image healthBar, armorBar, damagedArmorBar, damagedHealthBar;

    private GameObject projectileToSpawn;

    [Header("DRAG from ASSETS")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject fireballPrefab;

    private void Start()
    {
        if (arrowPrefab == null)
            AKN_GameController.instance.AKN_LOGERROR("Arrow Prefab in " + gameObject.name + " is NULL", true);

        combatIcon.sprite = unit.icon; // Set Combat Icon to unit's Combat Icon
        tag = team.ToString(); // Set gameObject's tag according to unit's Team

        GetComponent<CircleCollider2D>().enabled = true; // Enable gameObject's collider so Tile can detect CollisionEnter2D

        UpdateHPBar();
    }

    private IEnumerator UpdateHPBar() 
    {
        float shrinkDelay = 0.5f;

        healthBar.fillAmount = unit.currentHealth / unit.maximumHealth; // Set Health Bar amount according to the Current Health
        armorBar.fillAmount = unit.currentArmor / unit.maximumArmor; // Set Armor Bar amount according to the Current Armor

        healthBar.color = healthGradient.Evaluate(healthBar.fillAmount); // Set Health Bar color according to the Current Health
        armorBar.color = armorGradient.Evaluate(armorBar.fillAmount); // Set Armor Bar color according to the Current Armor

        while (armorBar.fillAmount < damagedArmorBar.fillAmount || healthBar.fillAmount < damagedHealthBar.fillAmount) // HP Decrease Animation
        {
            shrinkDelay -= Time.deltaTime;
            if (shrinkDelay < 0)
            {
                float shrinkSpeed = 0.75f;

                if (armorBar.fillAmount < damagedArmorBar.fillAmount) // If Armor Bar Amount is less than Damaged Armor Bar
                {
                    damagedArmorBar.fillAmount -= shrinkSpeed * Time.deltaTime; // Decrease Damaged Armor Bar over time
                }

                if (healthBar.fillAmount < damagedHealthBar.fillAmount) // If Health Bar Amount is less than Damaged Health Bar
                {
                    damagedHealthBar.fillAmount -= shrinkSpeed * Time.deltaTime; // Decrease Damaged Health Bar over time
                }
            }
            yield return null;
        }

    }

    public bool TakeDamage(float _damage)
    {
        float _exceedDamage;

        if (unit.currentArmor > 0) // If unit has Armor
        {
            if (_damage > unit.currentArmor) // If Damage is greater than the Current Armor
            {
                _exceedDamage = _damage - unit.currentArmor; // Calculate Exceed Damage by subtracting the Current Armor from the Damage
                unit.currentArmor -= _damage - _exceedDamage; // Same thing with unit.currentArmor = 0;
                unit.currentHealth -= _exceedDamage; // Subtract Exceed Damage from the Current Health
            }
            else // If Damage is less than the Current Armor
            {
                unit.currentArmor -= _damage; // Subtract Damage from the Current Armor
            }
        }
        else // If unit has no Armor
        {
            unit.currentHealth -= _damage; // Subtract Damage from the Current Health
        }

        StartCoroutine(UpdateHPBar()); // Update unit's HP Bar


        //Debug.Log(AKN_GameController.instance.combatController.selectedUnit.unit.name + " has attacked to " + unit.name + " and hit for " + _damage);

        if (unit.currentHealth <= 0) // If unit's Current Health is less than or equal to 0
            return true; // Return is dead
        else // If unit's Current Health is more than 0
            return false; // Return is not dead
    }

    public void ToggleHPBar()
    {
        healthBar.gameObject.SetActive(!isHPBarActive);
        armorBar.gameObject.SetActive(!isHPBarActive);
        damagedArmorBar.gameObject.SetActive(!isHPBarActive);
        damagedHealthBar.gameObject.SetActive(!isHPBarActive);
        isHPBarActive = !isHPBarActive;

    }

    public IEnumerator MoveUnit(GameObject _tileToMove)
    {
        if (AKN_GameController.instance.combatController.combatState == CombatState.FriendlyTurnStarted) // If Friendly Turn Started
        {
            moveGO.SetActive(false); // Doesn't work when Combat State is FriendlyMoving;
            attackGO.SetActive(false); // Doesn't work when Combat State is FriendlyMoving;

            AKN_GameController.instance.combatController.combatState = CombatState.FriendlyMoveStarted; // Set Combat State to Friendly Move Started

            ToggleHPBar(); // Disables HP Bar

            while (transform.position != _tileToMove.transform.position) // While Unit gameObject has not reached it's destination
            {
                transform.position = Vector2.MoveTowards(transform.position, _tileToMove.transform.position, 10 * Time.deltaTime); // Move Unit gameObject towards it's destination
                yield return null;
            }

            ToggleHPBar(); // Enables HP Bar

            AKN_GameController.instance.combatController.combatState = CombatState.FriendlyMoveEnded; // Set Combat State to Friendly Move Ended

            StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
        }
        else if (AKN_GameController.instance.combatController.combatState == CombatState.EnemyPrepareForMoveEnded) // If Enemy Prepare for Move Ended
        {
            moveGO.SetActive(false); // Doesn't work when Combat State is EnemyMoving;
            attackGO.SetActive(false); // Doesn't work when Combat State is EnemyMoving;

            AKN_GameController.instance.combatController.combatState = CombatState.EnemyMoveStarted; // Set Combat State to Enemy Move Started

            ToggleHPBar(); // Disables HP Bar

            while (transform.position != _tileToMove.transform.position) // While Unit gameObject has not reached it's destination
            {
                transform.position = Vector2.MoveTowards(transform.position, _tileToMove.transform.position, 10 * Time.deltaTime); // Move Unit gameObject towards it's destination
                yield return null;
            }

            ToggleHPBar(); // Enables HP Bar

            AKN_GameController.instance.combatController.combatState = CombatState.EnemyMoveEnded; // Set Combat State to Enemy Move Ended

            StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
        }


    }

    public IEnumerator AttackUnit(GameObject _tileToAttack)
    {
        moveGO.SetActive(false);
        attackGO.SetActive(false);
        
        if (AKN_GameController.instance.combatController.combatState == CombatState.FriendlyTurnStarted) // If Friendly Turn Started
        {
            AKN_GameController.instance.combatController.combatState = CombatState.FriendlyAttackStarted; // Set Game State to Friendly Attack Started
            //Debug.Log(unit.name + " attacked " + _tileToAttack);
            AKN_Unit _unitToAttack = _tileToAttack.GetComponent<AKN_Tile>().unitOnTileGO.GetComponent<AKN_Unit>(); // Set Unit to Attack based on Tile to Attack

            AttackAnimation(_tileToAttack); // Play Attack Animation

            float distance =  Vector2.Distance(_tileToAttack.transform.position, transform.position); // Get distance between Unit and Unit to Attack
            if (unit.attackRadius == 2 && distance >= 2.2f && distance <= 2.5f) // If Selected Unit's Attack Radius is 2 and Target is next to the Selected Unit
                isTargetDead = _unitToAttack.TakeDamage(unit.damage / 2); // Deal half Damage
            else // If Selected Unit's Attack Radius is 1 or Target is not next to the Selected Unit
                isTargetDead = _unitToAttack.TakeDamage(unit.damage); // Deal full damage

            if (isTargetDead) // If target is dead after the attack
            {
                //Debug.Log(unit.name + " has killed " + _unit.name);
                StartCoroutine(KillUnit(_tileToAttack)); // Kill the Unit
                yield return new WaitUntil(() => isKillUnitCompleted); // Wait Until Unit death animation ends
                AKN_GameController.instance.combatController.combatState = CombatState.FriendlyAttackEnded; // Set Game State to Friendly Attack Ended
                StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
            }
            else // If target is still alive after the attack
            {
                StartCoroutine(_unitToAttack.UpdateHPBar()); // Update Attacked Unit's HP Bar
                AKN_GameController.instance.combatController.combatState = CombatState.FriendlyAttackEnded; // Set Game State to Friendly Attack Ended
                StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
            }
        }
        else if (AKN_GameController.instance.combatController.combatState == CombatState.EnemyTurnStarted) // If Enemy Turn Started
        {
            AKN_GameController.instance.combatController.combatState = CombatState.EnemyAttackStarted; // Set Game State to Enemy Attack Started
            //Debug.Log(unit.name + " attacked " + _tileToAttack);
            AKN_Unit _unitToAttack = _tileToAttack.GetComponent<AKN_Tile>().unitOnTileGO.GetComponent<AKN_Unit>(); // Set Unit to Attack based on Tile to Attack

            AttackAnimation(_tileToAttack); // Play Attack Animation

            float distance =  Vector2.Distance(_tileToAttack.transform.position, transform.position); // Get distance between Unit and Unit to Attack
            if (unit.attackRadius == 2 && distance >= 2.2f && distance <= 2.5f) // If Selected Unit's Attack Radius is 2 and Target is next to the Selected Unit
                isTargetDead = _unitToAttack.TakeDamage(unit.damage / 2); // Deal half Damage
            else // If Selected Unit's Attack Radius is 1 or Target is not next to the Selected Unit
                isTargetDead = _unitToAttack.TakeDamage(unit.damage); // Deal full damage

            if (isTargetDead) // If target is dead after the attack
            {
                //Debug.Log(unit.name + " has killed " + _unit.name);
                StartCoroutine(KillUnit(_tileToAttack)); // Kill the Unit
                yield return new WaitUntil(() => isKillUnitCompleted); // Wait Until Unit death animation ends
                AKN_GameController.instance.combatController.combatState = CombatState.EnemyAttackEnded; // Set Game State to Enemy Attack Ended
                StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
            }
            else // If target is still alive after the attack
            {
                StartCoroutine(_unitToAttack.UpdateHPBar()); // Update Attacked Unit's HP Bar
                AKN_GameController.instance.combatController.combatState = CombatState.EnemyAttackEnded; // Set Game State to Enemy Attack Ended
                StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
            }

            
        }
    }
    
    private void AttackAnimation(GameObject _tileToAttack)
    {
        if (unit.attackType is AttackType.Melee) // If Attacker is a Melee Unit
        {
            StartCoroutine(MeleeAttackAnimation(_tileToAttack)); // Play Fighter Attack Animation
        }
        else if (unit.attackType is AttackType.Ranged) // If Attacker is a Ranged Unit
        {
            float distance =  Vector2.Distance(_tileToAttack.transform.position, transform.position); // Get distance between Unit and Unit to Attack
            if (unit.attackRadius == 2 && distance >= 2.2f && distance <= 2.5f) // If Selected Unit's Attack Radius is 2 and Target is next to the Selected Unit
                StartCoroutine(MeleeAttackAnimation(_tileToAttack)); // Play Melee Animation
            else // If Selected Unit's Attack Radius is 1 or Target is not next to the Selected Unit
            {
                if (unit.characterClass is CharacterClass.Ranger) // If Unit is Ranger
                    projectileToSpawn = arrowPrefab; // Set projectile to Arrow
                else if (unit.characterClass is CharacterClass.Sorcerer) // If Unit is Sorcerer
                    projectileToSpawn = fireballPrefab; // Set projectile to Fireball

                GameObject _projectileGO = Instantiate(projectileToSpawn, transform); // Spawn Projectile
                StartCoroutine(RangedAttackAnimation(_tileToAttack, _projectileGO)); // Play Ranged Animation
            }

        }
    }

    private IEnumerator MeleeAttackAnimation(GameObject _tileToAttack)
    {
        float startTime = Time.time;
        var targetPos = (combatIcon.transform.position + _tileToAttack.transform.position) / 2f; // Set Target's position to be middle of Selected Unit and the Target Unit

        while (true)
        {
            var currentTime = Time.time;
            if (startTime + 0.25f < currentTime)
                break;
            combatIcon.transform.position = Vector2.MoveTowards(combatIcon.transform.position, targetPos, ((startTime + 0.25f) - currentTime)); // Move Selected Unit's icon to middle
            yield return null;
        }
        startTime = Time.time;
        while (true)
        {
            var currentTime = Time.time;
            if (startTime + 0.25f < currentTime)
                break;
            combatIcon.transform.position = Vector2.MoveTowards(combatIcon.transform.position, transform.position, ((startTime + 0.25f) - currentTime)); // Move Selected Unit's icon back to it's position
            yield return null;
        }
        combatIcon.transform.position = transform.position; // Set Selected Unit's icon's position to Unit's position
    }

    private IEnumerator RangedAttackAnimation(GameObject _tileToAttack, GameObject _projectileGO)
    {
        float startTime = Time.time;

        var targetPos = _tileToAttack.transform.position; // Set Target's position

        while (true)
        {
            var currentTime = Time.time;
            if (startTime + 0.5f < currentTime)
                break;
            _projectileGO.transform.right = _tileToAttack.transform.position - transform.position; // Turn projectile to Target
            _projectileGO.transform.position = Vector2.MoveTowards(_projectileGO.transform.position, targetPos,  ((startTime + 0.5f) - currentTime)); // Move Projectile to Target
            yield return 0;
        }
        Destroy(_projectileGO); // Destroy the Projectile
    }

    public IEnumerator KillUnit(GameObject _tileToKill)
    {
        isKillUnitCompleted = false;

        AKN_Unit _unitToKill = _tileToKill.GetComponent<AKN_Tile>().unitOnTileGO.GetComponent<AKN_Unit>(); // Set Unit to Kill based on Tile to Kill

        _unitToKill.ToggleHPBar(); // Disable Unit to Kill's HP Bar
        _unitToKill.combatIcon.sprite = AKN_GameController.instance.combatController.combatUIController.unitDeadIcon; // Set Unit to Kill's Combat Icon to Dead icon
        yield return new WaitForSeconds(0.5f); // Wait 0.5 seconds to show Dead icon

        if (_unitToKill.team is Team.Friendly) // If Unit to Kill's team is Friendly
        {
            AKN_GameController.instance.combatController.friendlyUnitsInCombat.Remove(_unitToKill.gameObject); // Remove Unit to Kill from Friendly Units in Combat
            AKN_GameController.instance.combatController.friendlyCount--; // Decrease Friendly Count by 1
        }
        else if (_unitToKill.team is Team.Enemy) // If Unit to Kill's team is Enemy
        {
            AKN_GameController.instance.combatController.enemyUnitsInCombat.Remove(_unitToKill.gameObject); // Remove Unit to Kill from Enemy Units in Combat
            AKN_GameController.instance.combatController.enemyCount--; // Decrease Enemy Count by 1
        }

        Destroy(_tileToKill.GetComponent<AKN_Tile>().unitOnTileGO); // Destroy Unit to Kill's gameObject

        if (AKN_GameController.instance.combatController.friendlyCount == 0) // If there are no more Friendly Unit left
        {
            AKN_GameController.instance.combatController.combatState = CombatState.Lost; // Set Combat State to Lost
            AKN_GameController.instance.combatController.EndCombat(); // End Combat
        }
        else if (AKN_GameController.instance.combatController.enemyCount == 0) // If there are no more Enemy Unit left
        {
            AKN_GameController.instance.combatController.combatState = CombatState.Won; // Set Combat State to Won
            AKN_GameController.instance.combatController.EndCombat(); // End Combat
        }

        isKillUnitCompleted = true;
    }

    public void FindNearestUnit_AI()
    {
        if (AKN_GameController.instance.combatController.combatState == CombatState.EnemyTurnStarted) // If Enemy Turn Started
        {
            AKN_GameController.instance.combatController.combatState = CombatState.EnemySearchUnitStarted; // Set Combat State to Enemy Search Unit Started
            
            unitsInCombat_AI.Clear(); // Clear Units in Combat List
            tilesInPath_AI.Clear(); // Clear Tiles in Path to Target List

            float _radius = 30f; // Set a radius that covers the Combat Scene
            
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _radius); // Add every object in radius to hitColliders 
            float minSqrDistance = Mathf.Infinity;

            foreach (var hitCollider in hitColliders)
            {
                // If Object's Tag is "Tile", Tile has an Unit on it and Unit is Player or Teammate
                if (hitCollider.CompareTag("Tile") && hitCollider.GetComponent<AKN_Tile>().unitOnTileGO != null && (hitCollider.GetComponent<AKN_Tile>().unitOnTileGO.GetComponent<AKN_Unit>().unit is AKN_PlayerSO || hitCollider.GetComponent<AKN_Tile>().unitOnTileGO.GetComponent<AKN_Unit>().unit is AKN_TeammateSO))
                {
                    float distanceToSelectedUnit = (transform.position - hitCollider.transform.position).sqrMagnitude; // Calculate the distance
                    if (distanceToSelectedUnit < minSqrDistance)
                    {
                        minSqrDistance = distanceToSelectedUnit;
                        nearestUnit_AI = hitCollider.gameObject; // Get nearest Unit to Selected Unit
                    }
                    unitsInCombat_AI.Add(hitCollider.GetComponent<AKN_Tile>().unitOnTileGO); // Add every unit in the radius to Units in Combat List
                }
            }

            RaycastHit2D[] hits2D;
            hits2D = Physics2D.LinecastAll(transform.position, nearestUnit_AI.transform.position);
            
            if (hits2D.Length > 0)
            {
                //Color rndColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); //! delete after
                foreach (var _hit in hits2D)
                {
                    if (_hit.collider.tag == "Tile" && _hit.collider.GetComponent<AKN_Tile>().unitOnTileGO == null)
                    {
                        //_hit.collider.gameObject.GetComponent<SpriteRenderer>().color = rndColor; //! delete after
                        tilesInPath_AI.Add(_hit.collider.gameObject);
                    }
                }
            }

            AKN_GameController.instance.combatController.combatState = CombatState.EnemySearchUnitEnded;
        }
    }

    public void MoveTowardsNearestUnit_AI()
    {
        if (AKN_GameController.instance.combatController.combatState == CombatState.EnemySearchUnitEnded) // If Enemy Search Unit Ended
        {
            AKN_GameController.instance.combatController.combatState = CombatState.EnemyPrepareForMoveStarted; // Set Game State to Enemy Prepare for Move Started

            if (tilesInPath_AI.Count == 0) // If there is no tile in path to the Target
            {
                if (moveableTiles.Count > 0) // But there are Moveable Tiles
                {
                    // No tile to target but moveable tiles, moving random inside moveable tiles
                    int rnd = UnityEngine.Random.Range(0, moveableTiles.Count); // Get a random Tile inside Moveable Tiles
                    tileToMove_AI = moveableTiles[rnd]; // Set Tile to Move to this Tile
                    AKN_GameController.instance.combatController.combatState = CombatState.EnemyPrepareForMoveEnded; // Set Game State to Enemy Prepare For Move Ended
                    StartCoroutine(MoveUnit(tileToMove_AI)); // Move Unit to Tile to Move
                }
                else // If there are no Moveable Tiles
                {
                    // No tile in path and no moveable tile, skipping turn
                    AKN_GameController.instance.combatController.combatState = CombatState.EnemyTurnSkipped; // Set Game State to Enemy Turn Skipped
                    StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
                }
                return;
            }
            else // If there are Tiles in path to the Target
            {
                foreach (var _tile in tilesInPath_AI)
                {
                    if (moveableTiles.Contains(_tile)) // If Moveable Tiles contains this Tile
                    {
                        tileToMove_AI = _tile; // Set Tile to Move to this tile
                        if (tileToMove_AI.GetComponent<AKN_Tile>().unitOnTileGO != null) // If Tile to move is full
                        {
                            // There is a tile in path but it is full, finding new tile
                            AKN_GameController.instance.combatController.combatState = CombatState.EnemySearchUnitEnded; // Set Game State to Enemy Search Unit Ended
                            MoveTowardsNearestUnit_AI(); // Start from beginning
                        }
                        else // If Tile to move is empty
                        {
                            // Tile in path and moving to tile
                            AKN_GameController.instance.combatController.combatState = CombatState.EnemyPrepareForMoveEnded; // Set Game State to Enemy Prepare For Move Ended
                            StartCoroutine(MoveUnit(tileToMove_AI)); // Move Unit to Tile to Move
                            
                        }
                        return;
                    }
                    else // If Moveable Tiles does not contain this Tile
                    {
                        if (moveableTiles.Count > 0) // If there are Moveable Tiles
                        {
                            // Path is not in moveable tiles but there are moveable tiles, randomly walking towards target
                            RandomMove_AI(); // Move randomly prioritizing front first and sides next
                            
                        }
                        else // If there are no Moveable Tiles
                        {
                            // Path is not in moveable tiles and no moveable tiles, skipping turn
                            AKN_GameController.instance.combatController.combatState = CombatState.EnemyTurnSkipped; // Set Game State to Enemy Turn Skipped
                            StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
                        }
                        return;
                    }
                }
            }
        }
    }

    private void RandomMove_AI()
    {
        if (AKN_GameController.instance.combatController.combatState == CombatState.EnemyPrepareForMoveStarted) // If Enemy Prepare For Move Started
        {
            hitList_AI.Clear(); // Clear Hit List

            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, new Vector2(-1,-1), 2.5f); //Get Bottom Left Tile to inside Hit Array
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider != null && 
                    hit[i].collider.CompareTag("Tile") && 
                    hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO != this.gameObject && 
                    hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO == null)
                {
                    hitList_AI.Add(hit[i].collider.gameObject);
                }
            }

            hit = Physics2D.RaycastAll(transform.position, new Vector2(1,-1), 2.5f); //Get Bottom Right Tile to inside Hit Array
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider != null && 
                    hit[i].collider.CompareTag("Tile") && 
                    hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO != this.gameObject && 
                    hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO == null)
                {
                    hitList_AI.Add(hit[i].collider.gameObject);
                }
            }

            if (hitList_AI.Count > 0)
            {
                tileToMove_AI = hitList_AI[UnityEngine.Random.Range(0, hitList_AI.Count)];
                //Debug.Log("rnd moving to: " + goToTile_AI);
                AKN_GameController.instance.combatController.combatState = CombatState.EnemyPrepareForMoveEnded;
                StartCoroutine(MoveUnit(tileToMove_AI));
            }
            else
            {
                Debug.Log("rnd Sol alt ve Sağ alt dolu");
                hitList_AI.Clear();

                hit = Physics2D.RaycastAll(transform.position, Vector2.left, 2.5f); //Get Left Tile to inside Hit Array
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider != null && 
                        hit[i].collider.CompareTag("Tile") && 
                        hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO != this.gameObject &&
                        hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO == null)
                    {
                        hitList_AI.Add(hit[i].collider.gameObject);
                    }
                }

                hit = Physics2D.RaycastAll(transform.position, Vector2.right, 2.5f); //Get Right Tile to inside Hit Array
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider != null &&
                        hit[i].collider.CompareTag("Tile") && 
                        hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO != this.gameObject &&
                        hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO == null)
                    {
                        hitList_AI.Add(hit[i].collider.gameObject);
                    }
                }

                if (hitList_AI.Count > 0)
                {
                    tileToMove_AI = hitList_AI[UnityEngine.Random.Range(0, hitList_AI.Count)];
                    AKN_GameController.instance.combatController.combatState = CombatState.EnemyPrepareForMoveEnded;
                    StartCoroutine(MoveUnit(tileToMove_AI));
                }
                else
                {
                    Debug.Log("rnd Sol ve Sağ dolu!");
                    hitList_AI.Clear();

                    hit = Physics2D.RaycastAll(transform.position, new Vector2(-1,1), 2.5f); //Get Top Left Tile to inside Hit Array
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (hit[i].collider != null && 
                            hit[i].collider.CompareTag("Tile") && 
                            hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO != this.gameObject &&
                            hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO == null)
                        {
                            hitList_AI.Add(hit[i].collider.gameObject);
                        }
                    }

                    hit = Physics2D.RaycastAll(transform.position, new Vector2(1,1), 2.5f); //Get Top Right Tile to inside Hit Array
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (hit[i].collider != null &&
                            hit[i].collider.CompareTag("Tile") && 
                            hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO != this.gameObject &&
                            hit[i].collider.gameObject.GetComponent<AKN_Tile>().unitOnTileGO == null)
                        {
                            hitList_AI.Add(hit[i].collider.gameObject);
                        }
                    }

                    if (hitList_AI.Count > 0)
                    {
                        tileToMove_AI = hitList_AI[UnityEngine.Random.Range(0, hitList_AI.Count)];
                        AKN_GameController.instance.combatController.combatState = CombatState.EnemyPrepareForMoveEnded;
                        StartCoroutine(MoveUnit(tileToMove_AI));
                    }
                    else
                    {
                        Debug.Log("Nowhere to go! skipping turn");
                        AKN_GameController.instance.combatController.combatState = CombatState.EnemyTurnSkipped; // Set Game State to Enemy Turn Skipped
                        StartCoroutine(AKN_GameController.instance.combatController.NextTurn()); // Begin Next Turn
                    }

                }
            }
        }
        
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, 30);
    }
    */
}