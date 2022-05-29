using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingIcon : MonoBehaviour
{

    [SerializeField] private Building building;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        GetComponentInChildren<TMP_Text>().text = building.GetCost().ToString();
    }
    public void HandleClick()
    {
        playerController.HandleBuildingClicked(building);
    }

}
