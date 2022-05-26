using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHitPoints = 1;
    public float hitPoints = 0;

    public float GetHitPoints() => hitPoints;
    public float GetMaxHitPoints() => maxHitPoints;
    private void Awake()
    {
        hitPoints = maxHitPoints;
    }

    public void Damage(float val)
    {
        hitPoints = Mathf.Max(0, hitPoints - val);
        if(hitPoints <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        hitPoints = 0;
        if(gameObject.tag == "Player")
        {
            FindObjectOfType<GameManager>().GameOver();
            PlayerController.GetPlayer().GetComponent<SpriteRenderer>().enabled = false;
            PlayerController.GetPlayer().GetComponent<CapsuleCollider2D>().enabled = false;
            PlayerController.GetPlayer().GetComponent<PlayerController>().enabled = false;
            return;
        }
        Destroy(gameObject, 0.2f);
    }

    
}
