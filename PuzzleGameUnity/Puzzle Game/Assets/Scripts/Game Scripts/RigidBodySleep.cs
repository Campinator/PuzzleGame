/**
 * Date Created: Feb 16. 2022
 * Created By: Camp Steiner
 * 
 * Date Modified: Feb 16, 2022
 * Modified by: Camp Steiner
 * 
 * Description: Rigid body go sleep
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class RigidBodySleep : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.Sleep();
        }
    }
}
