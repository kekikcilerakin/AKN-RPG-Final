using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TileType{None, Border, FriendlySpawn, EnemySpawn, Obstacle}

public class AKN_Tile : MonoBehaviour, IPointerDownHandler
{
    public GameObject unitOnTileGO; // Unit gameObject currently on this Tile

    public TileType tileType;

    private Color defaultColor;

    [Header("DRAG from ASSETS")]
    public Sprite borderSprite; // Black Sprite to assign as borders
    public Sprite withTreeSprite; // With Tree Sprite to assign as obstacle

    public void InitializeTile()
    {   
        if (borderSprite == null)
            AKN_GameController.instance.AKN_LOGERROR("Border Sprite in " + gameObject.name + " is NULL", true);
        
        defaultColor = GetComponent<SpriteRenderer>().color;

        if (tileType == TileType.FriendlySpawn) // If this Tile is Friendly Spawn Point
            AKN_GameController.instance.combatController.friendlySpawnPoints.Add(this.transform); // Set Friendly Spawn Point in Combat Controller

        else if (tileType == TileType.EnemySpawn) // If this Tile is Enemy Spawn Point
            AKN_GameController.instance.combatController.enemySpawnPoints.Add(this.transform); // Set Enemy Spawn Point in Combat Controller

        else if (tileType == TileType.Border) // If this Tile is Border
        {
            GetComponent<SpriteRenderer>().sprite = borderSprite; // Set Tile's sprite as border sprite
            GetComponent<SpriteRenderer>().color = Color.black; // Set Tile's color to black
            Destroy(GetComponent<CircleCollider2D>()); // Remove Circle Collider 2d from this Tile
            Destroy(GetComponent<AKN_Tile>()); // Remove AKN_Tile from this Tile
            Destroy(GetComponent<Rigidbody2D>()); // Remove Rigidbody 2D from this Tile
        }
        else if (tileType == TileType.Obstacle)
        {
            gameObject.tag = "TileObstacle";
            GetComponent<SpriteRenderer>().sprite = withTreeSprite; // Set Tile's sprite as border sprite
            Destroy(GetComponent<CircleCollider2D>()); // Remove Circle Collider 2d from this Tile
            Destroy(GetComponent<AKN_Tile>()); // Remove AKN_Tile from this Tile
            Destroy(GetComponent<Rigidbody2D>()); // Remove Rigidbody 2D from this Tile
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.collider.CompareTag("Friendly") || other.collider.CompareTag("Enemy")) && unitOnTileGO == null) // If Friendly or Enemy entered this Tile and if this Tile is empty
        {
            unitOnTileGO = other.gameObject; // Set entered gameObject to Unit on this Tile
        }

        if (AKN_GameController.instance.combatController.combatState == CombatState.FriendlyTurnStarted) // If Friendly Turn Started
        {
            if (other.collider.name == "Move GO" && unitOnTileGO == null) // If Move GO is entered this Tile and if this Tile is empty
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); // Set this Tile's color to transparent white
                AKN_GameController.instance.combatController.selectedUnit.moveableTiles.Add(gameObject); // Add this Tile to Moveable Tiles of Move GO's Unit
            }
            else if (other.gameObject.name == "Attack GO" && unitOnTileGO != null) // If Attack GO is entered this Tile and if there is a Unit on this Tile
            {
                if (unitOnTileGO.GetComponent<AKN_Unit>().unit is AKN_EnemySO && other.transform.parent.GetComponent<AKN_Unit>().team == Team.Friendly) // If Unit in this tile is Enemy and Attack GO's Unit is Friendly
                {
                    GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f); // Set this Tile's color to transparent red
                    AKN_GameController.instance.combatController.selectedUnit.attackableTiles.Add(gameObject); // Add this Tile to Attackable Tiles of Attack GO's Unit
                }
            }
        }
        else if (AKN_GameController.instance.combatController.combatState == CombatState.EnemyTurnStarted) // If Enemy Turn Started
        {
            if (other.collider.name == "Move GO" && unitOnTileGO == null) // If Move GO is entered this Tile and if this Tile is empty
            {
                AKN_GameController.instance.combatController.selectedUnit.moveableTiles.Add(gameObject); // Add this Tile to Moveable Tiles of Move GO's Unit
            }
            else if (other.gameObject.name == "Attack GO" && unitOnTileGO != null) // If Attack GO is entered this Tile and if there is a Unit on this Tile
            {
                if ((unitOnTileGO.GetComponent<AKN_Unit>().unit is AKN_PlayerSO || unitOnTileGO.GetComponent<AKN_Unit>().unit is AKN_TeammateSO) && other.transform.parent.GetComponent<AKN_Unit>().team == Team.Enemy) // If Unit in this tile is Player or Teammate and Attack GO's Unit is Enemy
                {
                    AKN_GameController.instance.combatController.selectedUnit.attackableTiles.Add(gameObject); // Add this Tile to Attackable Tiles of Attack GO's Unit
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if ((other.collider.CompareTag("Friendly") || other.collider.CompareTag("Enemy") && other.gameObject == unitOnTileGO)) // If Friendly or Enemy exited this Tile and if this Unit in tile is the one exited
        {
            unitOnTileGO = null; // Set Unit on Tile gameObject to null
            
        }

        if (AKN_GameController.instance.combatController.combatState == CombatState.FriendlyTurnStarted || AKN_GameController.instance.combatController.combatState == CombatState.EnemyTurnStarted) // If Friendly or Enemy Turn Started
        {
            if (other.gameObject.name == "Move GO") // If exited gameObject is Move GO
            {
                GetComponent<SpriteRenderer>().color = defaultColor; // Set this Tile's color to it's default (empty) color
                AKN_GameController.instance.combatController.selectedUnit.moveableTiles.Remove(gameObject); // Remove this tile from Move GO's Unit's Moveable Tiles
            }
            else if (other.gameObject.name == "Attack GO") // If exited gameObject is Attack GO
            {
                GetComponent<SpriteRenderer>().color = defaultColor; // Set this Tile's color to it's default (empty) color
                AKN_GameController.instance.combatController.selectedUnit.attackableTiles.Remove(gameObject); // Remove this tile from Move GO's Unit's Attackable Tiles
            }
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData) // If we click on this Tile
    {
        if (AKN_GameController.instance.combatController.combatState == CombatState.FriendlyTurnStarted) // If Friendly Turn Started
        {
            if (AKN_GameController.instance.combatController.selectedUnit.moveableTiles.Contains(gameObject)) // If this tile is in Selected Unit's Moveable Tiles
            {
                StartCoroutine(AKN_GameController.instance.combatController.selectedUnit.MoveUnit(gameObject)); // Move the Selected Unit to this Tile
            }
            else if (unitOnTileGO != null && unitOnTileGO.GetComponent<AKN_Unit>().unit is AKN_EnemySO && AKN_GameController.instance.combatController.selectedUnit.attackableTiles.Contains(gameObject)) // If this tile has an Unit on it and Unit is Enemy and This tile is in Selected Unit's Attackable Tiles
            {
                StartCoroutine(AKN_GameController.instance.combatController.selectedUnit.AttackUnit(gameObject)); // Make Selected Unit Attack this Tile
            }
        }
    }

}

/*

    private void OnMouseEnter()
    {
        if (unitOnTileGO == null) return;

        if (AKN_GameController.instance.combatController.combatState == CombatState.FriendlyTurn && unitOnTileGO.GetComponent<AKN_Unit>().unit is AKN_Enemy)
        {
            AKN_GameController.instance.combatController.combatUIController.SetUnitPanel(unitOnTileGO.GetComponent<AKN_Unit>().unit);
        }
    }
*/