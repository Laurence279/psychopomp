using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemonDisplay : MonoBehaviour
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
        int demonCountToWin = player.GetDemonWinCount();
        int demonsSpawned = player.GetDemonsSpawned();
        SetText($"Demons: {(demonCountToWin - demonsSpawned)}");
    }
}
