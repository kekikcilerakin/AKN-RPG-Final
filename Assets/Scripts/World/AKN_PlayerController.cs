using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AKN_PlayerController : MonoBehaviour
{
    public TerrainType currentTerrain;

    public float speed = 1;
    public GameObject playerMapIcon;
    public GameObject travelConfirmation;
    private Vector2 destination;
    public bool isTraveling = false;

    private LineRendererArrow lineRenderer;

    [Header("DRAG from ASSETS")]
    public AKN_PlayerSO player;

    private void Awake()
    {
        if (player == null)
            AKN_GameController.instance.AKN_LOGERROR("Player in " + gameObject.name + " is NULL", true);

        player = Instantiate(player);
        lineRenderer = gameObject.GetComponent<LineRendererArrow>();

    }

    private void Update()
    {
        if (AKN_GameController.instance.gameState ==  GameState.Free)
        {
            Travel();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AKN_GameController.instance.combatInitializer.InitializeCombat();
        }
    }

    private void Travel()
    {
        
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
        #endif

        #if UNITY_ANDROID && !UNITY_EDITOR
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
        #endif
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

                if (hit.collider.gameObject == travelConfirmation)
                {
                    
                    hit = Physics2D.Linecast(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1 << LayerMask.NameToLayer("Water"));
                    
                    if (hit.collider != null && hit.collider.CompareTag("Water"))
                    {
                        isTraveling = false;
                        AKN_GameController.instance.UIController.ShowTooltip(AKN_GameController.instance.databaseController.GetTooltipByID(0) as AKN_TooltipSO);
                        //AKN_GameController.instance.UIController.ShowTooltip(AKN_GameController.instance.UIController.blockedPathTooltipSO as AKN_TooltipSO);
                    }
                    else
                    {
                        isTraveling = true;
                        destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        lineRenderer.ArrowTarget = new Vector3(destination.x, destination.y, -1);
                    }


                }
                else
                {

                    /*if (!isMovementConfirmed) //? GEREKSİZ?
                    {
                        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }*/

                    travelConfirmation.transform.position = new Vector3 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);
                }
            }

        }
        
        if (transform.position == new Vector3 (destination.x, destination.y))
        {
            isTraveling = false;

        }

        if (isTraveling)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            lineRenderer.ArrowOrigin = transform.position;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
            lineRenderer.UpdateArrow();

            if (Random.value <= 0.01f)
            {
                AKN_GameController.instance.combatInitializer.InitializeCombat();
            }
        }
        else
        {
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            lineRenderer.UpdateArrow();
        }
    }
    
}
