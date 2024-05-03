using UnityEditor.Timeline.Actions; // Importing necessary libraries
using UnityEngine;

public class CollisionScript : MonoBehaviour
{   
    float IsColiding1 = 1f; // Variable to track collision status, initialized to 1

    // Method to return the collision status
    public float ColidingReturnMethod()
    {
        return IsColiding1; // Returning the collision status
    }

    void Start()
    {
        // Start method (currently empty)
    }

    // Method called when a collision is detected
    void OnCollisionEnter2D(Collision2D collision)
    {   
        // Checking collision tags and broadcasting messages accordingly
        if (collision.gameObject.CompareTag("OBS") && gameObject.CompareTag("Player"))
        {
            Debug.Log("Obstacle Hit");
        }

        if (collision.gameObject.CompareTag("Wepon") && gameObject.CompareTag("Player"))
        {
            BroadcastMessage("PlayerHit");
            Debug.Log("Player Hit");
        }

        if (collision.gameObject.CompareTag("Heart") && gameObject.CompareTag("Player"))
        {
            BroadcastMessage("HeartPickedUp");
            Debug.Log("Heart Picked Up");
        }

        if (collision.gameObject.CompareTag("Wepon") && gameObject.CompareTag("Shield"))
        {
            Debug.Log("Hit Shield");
        }
    }

    // Method called when a collision persists
    void OnCollisionStay2D(Collision2D collision)
    {
        IsColiding1 = -1; // Set collision status to -1 when colliding
    }

    // Method called when a collision ends
    void OnCollisionExit2D(Collision2D collision)
    {
        IsColiding1 = 1; // Set collision status to 1 when collision ends
    }
}
