using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHitPoints = 1;
    public float hitPoints = 0;

    [SerializeField] private GameObject healthBar = null;

    public float GetHitPoints() => hitPoints;
    public float GetMaxHitPoints() => maxHitPoints;

    public bool isDead() => hitPoints <= 0;
    private void Awake()
    {
        hitPoints = maxHitPoints;
    }

    public void Heal(float val)
    {
        hitPoints = Mathf.Min(hitPoints + val, maxHitPoints);
    }

    public void Damage(float val)
    {
        hitPoints = Mathf.Max(0, hitPoints - val);
        if(hitPoints <= 0)
        {
            Death();
        }
    }

    private void Update()
    {
        if(healthBar != null)
        {
            healthBar.GetComponent<Slider>().value = (100 / maxHitPoints) * hitPoints;
        }
    }

    private void Death()
    {
        hitPoints = 0;
        if(gameObject.tag == "Player")
        {
            FindObjectOfType<GameManager>().GameOver();
            return;
        }
        Destroy(gameObject, 0.2f);
    }

    
}
