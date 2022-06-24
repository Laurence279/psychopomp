using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InformationOverlay : MonoBehaviour
{
    private void Update()
    {
        transform.localScale = Vector3.one;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        { 
            this.gameObject.SetActive(false);
        }
    }
}
