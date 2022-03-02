/**
 * Date Created: Feb 14. 2022
 * Created By: Camp Steiner
 * 
 * Date Modified: Feb 14, 2022
 * Modified by: Camp Steiner
 * 
 * Description: Camera to follow projectile
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static GameObject POI; //what to follow

    public float camZ; //z index of camera currently

    [Header("Set in Inspector")]
    public float easing = 0.5f; //smoothing on camera
    public Vector2 minXY = Vector2.zero; //clamping camera

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // if (POI == null) return;
        // Vector3 dest = POI.transform.position;

        Vector3 dest;
        if(POI == null)
        {
            dest = Vector3.zero;
        }
        else
        {
            dest = POI.transform.position;
            if(POI.tag == "Projectile")
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }

        //clamp x and y 
        dest.x = Mathf.Max(minXY.x, dest.x);
        dest.y = Mathf.Max(minXY.y, dest.y);
        //interpolate from current position to target object
        dest = Vector3.Lerp(this.transform.position, dest, easing);
        dest.z = camZ;
        this.transform.position = dest;

        Camera.main.orthographicSize = dest.y + 10;

    }

    public void SwitchView(string eView = "")
    {
        /*if (eView == "")
        {
            eView = HUDCanvas.viewButtonText.text;
        }
        string showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                HUDCanvas.viewButtonText.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                HUDCanvas.viewButtonText.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                HUDCanvas.viewButtonText.text = "Show Slingshot";
                break;
        }*/
    }
}
