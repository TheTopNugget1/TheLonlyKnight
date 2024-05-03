using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Mono.Cecil.Cil;
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



public class InputScript : MonoBehaviour
{
    // Public variables
    public GameObject Sheild;
    public GameObject pointer;
    public GameObject pauseUI;

    public DashboardScript Dash; // Assuming DashboardScript is a custom script

    public CollisionScript Collision;

    // Changable variables
    public int NumOfDash = 4;  // changable
    public float DashDistance = 2f; // changable
    public float CoolDownTime = 3f; // changable
    public float PlayerSpeed = 2.5f;  // changable
    public float PlayerSize = 1f;   // changable
    public float PointerSize = 0.5f;  // changable 
    public float PointerRadiusFromPlayer = 2.0f; // changable
 


    // Player controls
    PlayerControls controls;

    // Player movement variables
    Vector2 PlayerMove;
    Vector2 PointerMove;
    Vector2 PlayerLastPos;

    // Dash variables
    int[] DashArray;
    bool AbleToDash = true;
    public Vector3 PlayerVelocity = Vector3.zero;

    // Rigidbody and movement tracking variables
    private Rigidbody2D rb2D;
    private Vector2 lastPosition;
    private float lastTime;

    // Pause Screen
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component attached to the game object and store it in rb2D
        rb2D = GetComponent<Rigidbody2D>();

        // Record the initial position of the Rigidbody2D as the last position
        lastPosition = rb2D.position;

        // Record the current time as the last time
        lastTime = Time.time;

        // Initialize the DashArray
        DashInitializer();

    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Initialize player controls
        controls = new PlayerControls();

        // Assign actions to control events
        controls.Gameplay.Move.performed += ctx => PlayerMove = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => PlayerMove = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => PointerMove = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => PointerMove = Vector2.zero;

        controls.Gameplay.LightATK.performed += ctx => LightATK(1);
        controls.Gameplay.LightATK.canceled += ctx => LightATK(2);

        controls.Gameplay.HeavyATK.performed += ctx => HeavyATK(1);
        controls.Gameplay.HeavyATK.canceled += ctx => HeavyATK(2);

        controls.Gameplay.Def.performed += ctx => Def(1);
        controls.Gameplay.Def.canceled += ctx => Def(2);

        controls.Gameplay.Interact.performed += ctx => Interact();
        controls.Gameplay.Dodge.performed += ctx => Dodge();
        controls.Gameplay.Pause.performed += ctx => Pause();
    }

    // Update is called once per frame
    void Update()
    {  

        float IsColiding = Collision.ColidingReturnMethod();

        // Handle player movement based on input
        PlayerMovement(1);

        // Update the dashboard display with the current dash array information
        UpdateDashArray();

        // Calculate and update the player's velocity based on its displacement since the last frame
        PlayerVelocity = VelocityCalculator();

        // sets the chagnable variables of player and pointer size 
        transform.localScale = new Vector3(PlayerSize,PlayerSize,PlayerSize);
        pointer.transform.localScale = new Vector2(PointerSize, PointerSize);
    }

    void DashInitializer()
    {
        // Initialize the DashArray
        DashArray = new int[NumOfDash];

        for (int i = 0; i < NumOfDash; i++)
        {
            DashArray[i] = 1; // Set all dashes to available
        }
    }

    // Calculates player velocity
    // no longer needed for the player dash or motion so instead could use for a magnitual attack, the heavy attack deals more dammage the faster you travel
    Vector3 VelocityCalculator()
    {
        // Calculate the displacement vector by subtracting the previous position from the current position
        Vector2 displacement = rb2D.position - lastPosition;

        // Calculate the time elapsed since the last frame
        float deltaTime = Time.time - lastTime;

        // Check if deltaTime is zero to avoid division by zero
        if (deltaTime != 0)
        {
            // Calculate the velocity vector by dividing the displacement by the time elapsed
            // The resulting velocity is a Vector3 with the x and y components representing the velocity in the x and y directions,
            // and the z component set to 0 as this is a 2D game and there's no velocity in the z direction.
            Vector3 velocity = new Vector3(displacement.x, displacement.y, 0) / deltaTime;

            // Update the lastPosition and lastTime variables for the next frame
            lastPosition = rb2D.position;
            lastTime = Time.time;

            // Return the calculated velocity
            return velocity;
        }
        else
        {
            // If deltaTime is zero, return zero velocity
            return Vector3.zero;
        }
    }

    // Handles the dash cooldown
    IEnumerator Cooldown()
    {   
        // Wait for the cooldown duration specified by CoolDownTime
        yield return new WaitForSeconds(CoolDownTime);
    
        // Reset the DashArray to indicate that all dashes are available (1) again
        DashInitializer();
    
        // Set the AbleToDash flag to true to enable the player to dash again
        AbleToDash = true;
    }

    // Updates dash array
    void UpdateDashArray()
    {           
        // Send the DashArray data to the Dashboard script
        Dash.UpdateDashArray(DashArray);
    }

    // Handles player movement
    void PlayerMovement(float Reverse)
    {
        // Calculate the movement vector by multiplying the input vector (PlayerMove) 
        // by the player's speed and the time since the last frame (Time.deltaTime)
        Vector2 movement = PlayerMove * PlayerSpeed * Time.deltaTime * Reverse;
    
        // Move the player in the world space based on the calculated movement vector
        transform.Translate(new Vector3(movement.x, movement.y, 0), Space.World);
    
        // Calculate the position of the pointer by adding the normalized direction of 
        // PointerMove multiplied by 2 units to the player's position
        pointer.transform.position = transform.position + new Vector3(PointerMove.x, PointerMove.y, 0).normalized * PointerRadiusFromPlayer;
    
        // Make the player face the direction of the pointer by using Transform.LookAt
        transform.LookAt(new Vector3(pointer.transform.position.x, pointer.transform.position.y, 0));
    }

    // Handles light attack
    void LightATK(int x)
    {
        if (x == 1)
        {   
            Debug.Log("Light trigger pressed");
        }
        else if (x == 2)
        {
            Debug.Log("Light trigger released");
        }
    }

    // Handles heavy attack
    void HeavyATK(int x)
    {
        if (x == 1)
        {
            Debug.Log("Heavy trigger pressed");
        }
        else if (x == 2)
        {
            Debug.Log("Heavy trigger released");
        }   
    }

    // Handles defense action
    void Def(int x)
    {
        if (x == 1 || x == 2)
        {
            Sheild.gameObject.SetActive(true);
            Sheild.gameObject.SendMessage("DefRead", x == 1 ? "DefActive" : "DefDeactive"); // this line was done by chat gpt i could do it like the others but it looks cool so i kept it
        }
    }

    // Method responsible for performing the dodge action
    void Dodge()
    {      
        // Check if the player is able to dash
        if (AbleToDash == true)
        {
            // Sort the DashArray in ascending order
            Array.Sort(DashArray);

            // Set the last element of DashArray to 0, indicating that one dash has been used
            DashArray[NumOfDash - 1] = 0;

            // Calculate the new position after the dodge based on player velocity and dash distance
            Vector3 newPosition = transform.position + PlayerVelocity.normalized * DashDistance;

            // Move the player to the new position
            transform.position = newPosition;
        }

        // Check if the sum of all elements in DashArray equals 0 (indicating all dashes have been used)
        if (DashArray.Sum() == 0)
        {
            // Check if the player is currently able to dash
            if (AbleToDash)
            {
                // Start the cooldown coroutine if the player is able to dash
                StartCoroutine(Cooldown());
            }

            // Set AbleToDash to false, indicating that the player cannot dash until the cooldown period is over
            AbleToDash = false;
        }
    }

    // Handles interaction action
    void Interact()
    {
        // Placeholder for interaction logic
    }

    void Pause()
    {
        TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            pauseUI.SetActive(true); // Show pause UI
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            pauseUI.SetActive(false); // Hide pause UI
        }
    }

    // Enables player controls
    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    // Disables player controls
    void OnDisable()
    {
        controls.Gameplay.Enable();
    }
}
