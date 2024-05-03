using System;
using System.Linq; // Importing LINQ library for Sum method
using UnityEngine;
using UnityEngine.UI;

public class DashboardScript : MonoBehaviour
{
    // Variables for UI elements
    public Text DashCountText; // Reference to the text displaying dash count
    public GameObject Player; // Reference to the player object

    private int[] dashboardDashArray; // Array to store dash counts   

    // Strings for displaying dashboard information
    string words1 = "Dashes :"; // Label for dash count
    string words2 = "Health :"; // Label for player health
    string words3 = "Weapon :"; // Label for selected weapon

    float PlayerHealth = 100.0f; // Placeholder for health system output
    string WeaponSelected = "Potato"; // Placeholder for the weapon selection script 

    // Update is called once per frame
    void Update()
    {
        // Update the UI text with current dashboard information
        DashCountText.text = (
            "\n" + words1 +  dashboardDashArray.Sum() + // Display dash count
            "\n" + words2 + PlayerHealth + // Display player health
            "\n" + words3 + WeaponSelected); // Display selected weapon
    }

    // Method to update the dash array
    public void UpdateDashArray(int[] newDashArray)
    {
        dashboardDashArray = newDashArray; // Update the dashboard dash array
    }

    // Method to handle dash event (currently commented out)
    void DashEvent()
    {
        //Player.InputScript.Dodge(); // Uncomment this line once Player.InputScript.Dodge() is implemented
    }
}
