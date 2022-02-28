/**
 * Date Created: Feb 9. 2022
 * Created By: Camp Steiner
 * 
 * Date Modified: Feb 14, 2022
 * Modified by: Camp Steiner
 * 
 * Description: Controls the action of the slingshot
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Drop Prefab Here")]
    public GameObject projPrefab;
    [Header("Change Values Around")]
    public float velocityMultiplier = 10f;

    [Header("Don't Touch")]
    static private Slingshot S;  
    public GameObject launchPoint;
    public GameObject projectile; //instance of projectile
    public Vector3 launchPos; //position of projectile
    public bool aimingMode; //is player aiming
    public Rigidbody projRB; //useful for later

    private GameObject target;

    static public Vector3 LAUNCH_POS {                                       
            get {
                if (S == null ) return Vector3.zero;
                return S.launchPos;
            }
        }

    private void Awake()
    {
        S = this;    
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject; //find child obj called LaunchPoint
        launchPos = launchPointTrans.position;

        target = GameObject.FindGameObjectWithTag("Goal");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }


    private void Launch()
    {
        /**
         * Vector3 for position of slingshot
         * Slingshot knows position of target
         * Need to generate launch vector such that it creates a parabola that ends at target
         */
        projectile = Instantiate(projPrefab) as GameObject; //create projectile at launch point
        projectile.transform.position = launchPos;

        projRB = projectile.GetComponent<Rigidbody>();
        projRB.velocity = (launchPos - target.transform.position) * velocityMultiplier;
        FollowCam.POI = projectile; //set point of interest for follow cam
        projectile = null;
        ProjectileLine.S.poi = projectile;
    }

}