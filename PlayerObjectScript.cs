using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerObjectScript : MonoBehaviour
{

    public GameObject movment;

    void Update()
    {   
        transform.position = movment.transform.position;
    }
}
