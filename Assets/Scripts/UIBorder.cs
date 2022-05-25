using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(EventTrigger))]
public class UIBorder : MonoBehaviour
{

    Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public void ToggleBorder()
    {
        outline.enabled = !outline.enabled;
    }

}
