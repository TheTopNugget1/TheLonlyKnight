using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

/*

TO DO:

- rename teh input script to be the player script / main script

- directional dash
Needs to have 

- add in a killable enemie 
just a game object that disapears when trigger is pressed and the player is close enough
then make it reapear when the player is killed so that the enemies repawn when you die

- add in a wepon
the sword just following the player
add a opake circle around teh player for the attack range

- add the death and respawn 
make the death have a condition (hearts to be added later)

add in the heart system 

__ add an enemies remaining list/ counter 
- have all of teh enemies on the list and as they are defeated remove them from the list 
- then i can go through the list and count up all of the remaining values that are not 0
- i can also make seperate countes for each enemie type

------ this would be a more inificient way of just using the object tags and finding objects
 that have the tag on them and then jsut counting that but it would be an advanced technique

*/

public class InputScript : MonoBehaviour
{

    public GameObject Sheild;

    public GameObject pointer;

    PlayerControls controls;

    Vector2 PlayerMove;  // defines the move vector as a vector 2 so it only carrys 2 floats (x and y)

    Vector2 PointerMove;  // defines the pointer move vector as a vector 2 so it carrys 2 floats (x and y)

    int PlayerSpeed = 5; // the speed of the player 

    int DashDistance = 1; // distance the player moves in the dash

    int MaxNumberOfDashes = 3; // the max number of dashes the player has   

    int MaxNumberOfHearts = 3; // The max number of hearts 

    double DashCooldown = 1.5; // the cooldown time of the dash

    int DashAvail;


    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => PlayerMove = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => PlayerMove = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => PointerMove = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => PointerMove = Vector2.zero;

        controls.Gameplay.LightATK.performed += ctx => LightATK(1); // right triger down
        controls.Gameplay.LightATK.canceled += ctx => LightATK(2); // right trigger released 

        controls.Gameplay.HeavyATK.performed += ctx => HeavyATK(1); // left triger down
        controls.Gameplay.HeavyATK.canceled += ctx => HeavyATK(2); // left trigger released 

        controls.Gameplay.Def.performed += ctx => Def(1); // bumper down
        controls.Gameplay.Def.canceled += ctx => Def(2); // bumper released

        controls.Gameplay.Interact.performed += ctx => Interact();
        controls.Gameplay.Dodge.performed += ctx => Dodge();

    }

    void Update()
    {
        PlayerMovement();
        
    }

    void PlayerMovement()
    {
         // Joystcik movement of the player
        Vector2 movement = PlayerMove * PlayerSpeed * Time.deltaTime;
        transform.Translate(new Vector3(movement.x, movement.y, 0), Space.World);
    
        // Joystick rotation of the player
        pointer.transform.position = transform.position + new Vector3(PointerMove.x, PointerMove.y, 0).normalized * 2.0f;
        transform.LookAt(pointer.transform.position);   
    }
    
    void LightATK(int x)
    {
        if (x == 1) // Trigger pressed
        {   
            
            Debug.Log("Light trigger pressed");
        }
        else if (x == 2) // Trigger released
        {
            Debug.Log("Light trigger released");
        }
    }

    void HeavyATK(int x)
    {
        if (x == 1) // Trigger pressed
        {
            Debug.Log("Heavy trigger pressed");
        }
        else if (x == 2) // Trigger released 
        {
            Debug.Log("Heavy trigger released");
        }   
    }

    void Def(int x)
    {
        if (x == 1 || x == 2)  // or gate
        {
            Sheild.gameObject.SetActive(true); //ensures the object is active
            Sheild.gameObject.SendMessage("TriangleRead", x == 1 ? "TriActive" : "TriDeactive");  // send the coresponding message
            // change to bumper because those are used in the game
        }
    }

    void Dodge()
    {
        // int[] DashArray = [0, 1, 2];

        // DashArray[0] = 1;
        
    }

    void Interact()
    {

    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Enable();
    }

}