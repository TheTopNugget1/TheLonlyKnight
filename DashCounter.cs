using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCounter : MonoBehaviour
{

    public Text DashCountText;

    // Update is called once per frame
    void Update()
    {
        DashCountText.text = "Dashes : " + 3 ;
    }
}


/// tbc