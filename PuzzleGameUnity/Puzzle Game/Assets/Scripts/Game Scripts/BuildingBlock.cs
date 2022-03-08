/**
 * Date Created: Feb 16. 2022
 * Created By: Camp Steiner
 * 
 * Date Modified: March 7, 2022
 * Modified by: Camp Steiner
 * 
 * Description: Turns off and on the rigid body of the blocks while building
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingBlock : MonoBehaviour
{
    // Start is called before the first frame update
    //when the object spawns, don't want it interacting with anything
    void Awake()
    {
        Rigidbody currentRB = GetComponent<Rigidbody>();
        currentRB.useGravity = false;
        currentRB.detectCollisions = false;
    }

    //after game starts, enable physics for all the blocks
    public void EnablePhysics(){ 
        Rigidbody currentRB = GetComponent<Rigidbody>();
        currentRB.useGravity = true;
        currentRB.detectCollisions = true;
    }
}
