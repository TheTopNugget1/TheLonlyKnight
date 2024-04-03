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

    Vector2 RegSize = new Vector2(1.5F, 1.5F); //default size of the sheild

    bool On = false; // initialised to off

    void TriangleRead(string message)  // receives the message to turn on and off
    {

        if (message == "TriActive")  // toggles on to true
        {
            Debug.Log("recieved message: " + message);
            On = true;
        }

        else if (message == "TriDeactive")  // toggles on to false
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
        }

        else if(On == false) // makes the shield disapear
        {
            transform.localScale = Vector2.zero;
        }

        if(transform.position == player.transform.position)  // if the sheild is not being used no rotation input, then the sheild disapears
        {
            transform.localScale = Vector2.zero;
        }

        else if(On == true)    // if the sheild is not at the player position ie it is being used, it will apear again 
        {
            transform.localScale = RegSize;
        }
    }
}
