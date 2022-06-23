using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SoulsDisplay : MonoBehaviour
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
        int souls = player.GetSouls;
        SetText(String.Format("{0}", souls));
    }



}
