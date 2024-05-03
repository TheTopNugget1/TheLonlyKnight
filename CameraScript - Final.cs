using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class CameraScript : MonoBehaviour
{
    
    public GameObject player;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = player.transform.position;  //sets the camera to the player position 

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10); // offsets the camera 

    }
}
