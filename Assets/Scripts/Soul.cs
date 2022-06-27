using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Soul : MonoBehaviour
{

    private GameObject boundBeacon = null;
    private GameObject soulBank;

    private bool isSpecial = false;



    private void Awake()
    {
        soulBank = FindObjectOfType<Door>().gameObject;
    }
    private void Start()
    {
        isSpecial = Random.Range(1, 101) > 70;
    }

    private void FixedUpdate()
    {
        if (!isSpecial) return;
        GetComponent<AIController>().SetTargetObj(soulBank);
    }


    public void SetBeacon(GameObject beacon)
    {
        boundBeacon = beacon;
        GetComponent<AIController>().SetWanderArea(beacon.transform.position);
    }

    public GameObject GetBeacon() => boundBeacon;

    public void BankSoul()
    {
        Destroy(gameObject);
        PlayerController.GetPlayer().IncrementSouls();
    }

}
