/**
 * Date Created: Feb 9. 2022
 * Created By: Camp Steiner
 * 
 * Date Modified: March 1, 2022
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
    public float inaccuracy = 35f;

    [Header("Don't Touch")]
    static private Slingshot S;  
    public GameObject launchPoint;
    public GameObject projectile; //instance of projectile
    public Vector3 launchPos; //position of projectile
    public Rigidbody projRB; //useful for later

    public GameObject target;
    static private GameObject t;
    GameManager gm; //reference to game manager
    public static bool shouldFire = false;

    static public Vector3 LAUNCH_POS {                                       
            get {
                if (S == null ) return Vector3.zero;
                return S.launchPos;
            }
        }
    static public Vector3 TARGET_POS {
        get{
            if(t == null) return Vector3.zero;
            return (Vector3)t.transform.position;
        }
    }

    private void Awake()
    {
        S = this;  
        gm = GameManager.GM;  
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject; //find child obj called LaunchPoint
        launchPos = launchPointTrans.position;

        target = GameObject.FindGameObjectWithTag("Goal");
        t=target;
    }

    void Update()
    {
        if (shouldFire)
        {
            Launch();
        }
    }


    public void Launch()
    {
        /**
         * Vector3 for position of slingshot
         * Slingshot knows position of target
         * Need to generate launch vector such that it creates a parabola that ends at target
         */
        projectile = Instantiate(projPrefab) as GameObject; //create projectile at launch point
        projectile.transform.position = launchPos;

        projRB = projectile.GetComponent<Rigidbody>();

        float angle = Random.Range(30, 60) * Mathf.Deg2Rad; //more or less arc
        //2D kinematics equations whooo
        float range = Mathf.Abs(launchPos.x - target.transform.position.x) * Random.Range((1-inaccuracy/100), (1+inaccuracy/100)); //fuzz accuracy some so its not just sniping the player
        float v = Mathf.Sqrt((range * Mathf.Abs(Physics.gravity.y)) / (Mathf.Sin(2 * angle)));
        Vector3 launchV = new Vector3(v * Mathf.Cos(angle), v * Mathf.Sin(angle), 0);
        projRB.velocity = launchV; 
        FollowCam.POI = projectile; //set point of interest for follow cam
        ProjectileLine.S.poi = projectile;

        //set up stuff for next shot
        gm.Shots = gm.Shots - 1;
        shouldFire = false;
    }

    //so other scripts can cause the slingshot to fire
    public static void Fire(){
        shouldFire = true;
    }

}
