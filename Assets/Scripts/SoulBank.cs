using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBank : MonoBehaviour
{

    private GameObject door;

    private void Awake()
    {
        door = FindObjectOfType<Door>().gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Soul")
        {
            collision.gameObject.GetComponent<AIController>().SetTargetObj(door);
        }
    }

    

}
