using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class InformationOverlay : MonoBehaviour, IRayCastable
{
    [System.Serializable]
    struct DataTextMapping
    {
        [SerializeField] public string title;
        [SerializeField] public TextAsset data;
        [SerializeField] public TMP_Text text;
    }

    [SerializeField] DataTextMapping[] dataTextMappings = null;


    public GameObject informationOverlay = null;
    private bool isHovering;

    private void Start()
    {
        foreach (var mapping in dataTextMappings)
        {
            string[] dataLines = mapping.data.text.Split('\n');
            string randomLine = dataLines[UnityEngine.Random.Range(0, dataLines.Length)];
            TMP_Text test = mapping.text;
            test.text = $"<b>{mapping.title}:</b>\n<i>{randomLine}</i>";
        }
    }

    public bool HandleRayCast(PlayerController callingController)
    {
        isHovering = true;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            informationOverlay.SetActive(true);
        }
        return true;
    }
    
    private void Update()
    {
        transform.localScale = Vector3.one;

        if (!isHovering)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                informationOverlay.SetActive(false);
            }
        }
        isHovering = false;
        
    }

}
