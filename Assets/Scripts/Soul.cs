using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{

    public void BankSoul()
    {
        Destroy(gameObject);
        PlayerController.GetPlayer().IncrementSouls();
    }

}
