using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HealthDisplay : MonoBehaviour
{

    TMP_Text text = null;
    PlayerController player = null;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        player = PlayerController.GetPlayer();
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    private void Update()
    {
        float health = (100 / player.GetComponent<Health>().GetMaxHitPoints()) * player.GetComponent<Health>().GetHitPoints();
        SetText(String.Format("Health: {0}%", health));
    }



}
