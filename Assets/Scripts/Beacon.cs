using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{

    private GameObject soulBank;
    [SerializeField] private float radius = 5f;

    private void Start()
    {
        StartCoroutine(BeaconEmitter());
    }

    private void Awake()
    {
        soulBank = FindObjectOfType<SoulBank>().gameObject;
    }

    IEnumerator BeaconEmitter()
    {
        Collider2D[] arr = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Soul"));
        foreach (Collider2D c in arr)
        {
            if (c.gameObject.CompareTag("Soul") == true)
            {

                var soul = c.gameObject.GetComponent<Soul>();
                if (soul.GetBeacon() == null)
                {
                    soul.SetBeacon(gameObject);
                    continue;
                }

                var currentBeacon = soul.GetBeacon();
                float currentBeaconDistance = GetDistanceToBank(currentBeacon);
                float thisBeaconDistance = GetDistanceToBank(gameObject);

                if (thisBeaconDistance > currentBeaconDistance) continue;

                soul.SetBeacon(gameObject);

            }

        }

        yield return new WaitForSeconds(Random.Range(1f, 3f));
        yield return BeaconEmitter();
    }



    private float GetDistanceToBank(GameObject beacon)
    {
        return Vector2.Distance(beacon.transform.position, soulBank.transform.position);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(transform.position, radius);
    }




}
