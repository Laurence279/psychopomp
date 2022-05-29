using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    
    private Camera cam;
    [SerializeField] private float speed;
    public int soulCount = 0;
    [SerializeField] int startingSoulCount = 50;
    GameManager gameMgr;


    [SerializeField] private LayerMask groundMask = new LayerMask();
    private Building selectedBuilding = null;
    private GameObject buildingPreview = null;

    [SerializeField] Spawner demonSpawner = null;
    [SerializeField] public int demonCountToWin = 100;
    public int demonsSpawned = 0;
    private bool demonsGone = false;

    public int GetDemonWinCount() => demonCountToWin;
    public int GetDemonsSpawned() => demonsSpawned;

    public void IncrementSouls() => soulCount++;

    public int GetSouls => soulCount;

    public void RemoveSouls(int val) => soulCount -= val;

    public static PlayerController GetPlayer()
    {
        return FindObjectOfType<PlayerController>();
    }

    public void IncrementDemons()
    {
        demonsSpawned++;
        if(demonsSpawned >= demonCountToWin)
        {
            demonsGone = true;
            demonSpawner.StopSpawns();
        }
        if(demonsSpawned % 10 == 0)
        {
            demonSpawner.IncreaseSpawnRate(1f);
        }
        if(demonsSpawned % 20 == 0)
        {
            demonSpawner.IncreaseDifficulty();
        }
    }


    private void Awake() {
        soulCount = startingSoulCount;
        cam = Camera.main;
        gameMgr = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (demonsGone)
        {
            if (FindObjectOfType<Demon>() == null)
            {
                gameMgr.WinGame();
            }
        }
        if (!buildingPreview) return;
        UpdateBuildingPreview();

        if(Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelBuildingPlacement();
        }
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlaceBuilding();
        }
        

    }

    private void CancelBuildingPlacement()
    {
        
        Destroy(buildingPreview);
        buildingPreview = null;
        selectedBuilding = null;
    }

    private void TryPlaceBuilding()
    {
        Vector3 selectedCell = GetSelectedCell();
        if (!CanPlaceBuilding(selectedCell)) return;

        // Valid
        GameObject buildingPrefab = selectedBuilding.GetBuilding();
        GameObject newBuilding = Instantiate(buildingPrefab);
        newBuilding.transform.position = selectedCell;
        RemoveSouls(selectedBuilding.GetCost());

    }

    private void UpdateBuildingPreview()
    {
        Vector3 selectedCell = GetSelectedCell();

        Color colour = CanPlaceBuilding(selectedCell) ? Color.green : Color.red;
        buildingPreview.GetComponent<SpriteRenderer>().color = colour;

        buildingPreview.transform.position = selectedCell;
    }

    private Vector3 GetSelectedCell()
    {
        var grid = FindObjectOfType<Grid>();
        var worldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int cellPos = grid.WorldToCell(worldPos);
        var cellMid = grid.GetCellCenterLocal(cellPos);
        return cellMid;
    }

    private bool CanPlaceBuilding(Vector3 selectedCell)
    {
        Collider2D collider = Physics2D.OverlapCircle(selectedCell, 1f, groundMask);
        if (collider == null && GetSouls >= selectedBuilding.GetCost()) return true;
        return false;
    }



    public void HandleBuildingClicked(Building building)
    {
        selectedBuilding = building;
        buildingPreview = Instantiate(selectedBuilding.GetBuildingPreview());

    }




    /*

    private void FixedUpdate() 
    {

        Move();
        if(movement.magnitude == 0)
        {
            animator.SetBool("isMoving", false);
        }
        timeSinceLastAttacked += Time.deltaTime;
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        animator.SetBool("isMoving", true);
        movement = ctx.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
       if (EventSystem.current.IsPointerOverGameObject()) return;
       if(ctx.performed && timeSinceLastAttacked > attackRate)
        {
            timeSinceLastAttacked = 0;
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
        }

    }

    public void Move()
    {

        Vector2 currentPos = rb.position;
        Vector2 adjustedMovement = movement * speed;
        Vector2 newPos = currentPos + adjustedMovement * Time.deltaTime;

        float direction = movement.x;
        if(direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }


        rb.MovePosition(newPos);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Soul")
        {
            //collision.gameObject.GetComponent<AIController>().FollowPlayer();
            collision.gameObject.GetComponent<AIController>().SetTargetObj(FindObjectOfType<SoulBank>().gameObject);
        }
    }

    public void Hit()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(weaponOrigin.transform.position, 0.5f, enemyLayer);
        foreach(var collider in colliders)
        {
            collider.GetComponent<Health>().Damage(weaponDamage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(weaponOrigin.transform.position, 0.5f);
    }

    */


}
