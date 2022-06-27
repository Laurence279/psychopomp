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

    [SerializeField] float raycastRadius = 1f;
    public Action onNothingClick;


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

    public void DecrementDemons()
    {
        demonsSpawned--;
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

            if (InteractWithComponent()) return;

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

    private bool InteractWithComponent()
    {
        RaycastHit2D[] hits = RaycastAllSorted();
        foreach (RaycastHit2D hit in hits)
        {
            IRayCastable[] rayCastables = hit.transform.GetComponents<IRayCastable>();
            foreach (IRayCastable castable in rayCastables)
            {
                if (castable.HandleRayCast(this))
                {
                    //SetCursor(castable.GetCursorType());
                    return true;
                }
            }
        }

        return false;
    }

    RaycastHit2D[] RaycastAllSorted()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(GetMouseRay(), raycastRadius, Vector2.zero);
        float[] distances = new float[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            distances[i] = hits[i].distance;
        }
        Array.Sort(distances, hits);
        return hits;
    }

    private static Vector2 GetMouseRay()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            if (collider == null && CanAfford(selectedBuilding)) return true;
            return false;
        }

        private bool CanAfford(Building building)
        {
            return GetSouls >= building.GetCost();
        }


        public void HandleBuildingClicked(Building building)
        {
            selectedBuilding = building;
            buildingPreview = Instantiate(selectedBuilding.GetBuildingPreview());
        }




    }