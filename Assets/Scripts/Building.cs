using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Psychopomp/New Building")]
public class Building : ScriptableObject
{
    [SerializeField] private int ID;

    [SerializeField] private string Name;

    [TextArea]
    [SerializeField] private string Description;

    [SerializeField] private int cost = 50;

    [SerializeField] GameObject buildingPreview;
    [SerializeField] GameObject building;
    public GameObject GetBuildingPreview() => buildingPreview;

    public GameObject GetBuilding() => building;

    public int GetCost() => cost;


}
