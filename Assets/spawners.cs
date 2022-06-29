using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawners : MonoBehaviour
{

    [SerializeField] GameObject soulBank;
    public void Attack()
    {
        foreach(var spawner in GetComponentsInChildren<Spawner>())
        {
            spawner.AttackPlayer(soulBank);
        }
    }
}
