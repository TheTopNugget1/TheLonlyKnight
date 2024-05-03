using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class SheildScript : MonoBehaviour
{

    public GameObject player;

    public GameObject pointer;

    public CollisionScript Collision;
    
    public Vector2 RegSize = new Vector2(1.5f, 1.5f);

    bool On = false; // initialised to off

    private Collider2D shieldCollider; // Reference to the collider component

    void Start()
    {
        // Get the Collider2D component attached to the shield GameObject
        shieldCollider = GetComponent<Collider2D>();
    }

    void DefRead(string message)  // receives the message to turn on and off
    {

        if (message == "DefActive")  // toggles on to true
        {
            Debug.Log("recieved message: " + message);
            On = true;
        }

        else if (message == "DefDeactive")  // toggles on to false
        {
            Debug.Log("recieved message: " + message);
            On = false;
            
        }
    }
    
    void Update()
    {
        transform.position = pointer.transform.position;  //sets the sheild to the pointer position 

        // All the code below just toggles the on and off of the sheild and makes it disapear when not being used ie no input for 

        if(On)  // makes the sheild viable
        {
            transform.localScale = RegSize;

            // Enable the collider when the shield is activated
            shieldCollider.enabled = true;
            
        }

        else if(On == false) // makes the shield disapear
        {
            transform.localScale = Vector2.zero;

            // Disable the collider when the shield is deactivated
            shieldCollider.enabled = false;
        }

        if(transform.position == player.transform.position)  // if the sheild is not being used no rotation input, then the sheild disapears
        {
            transform.localScale = Vector2.zero;
            
            // Disable the collider when the shield is deactivated
            shieldCollider.enabled = false;
        }

        else if(On == true)    // if the sheild is not at the player position ie it is being used, it will apear again 
        {
            transform.localScale = RegSize;
        }
    }
}
