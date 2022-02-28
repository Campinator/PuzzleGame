/**
 * Date Created: Feb 16. 2022
 * Created By: Camp Steiner
 * 
 * Date Modified: Feb 16, 2022
 * Modified by: Camp Steiner
 * 
 * Description: Lines for projectile trajectory
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
   public static ProjectileLine S; //singleton line

   [Header("Set in Inspector")]
   public float minDistance = 0.1f;

   private LineRenderer line;
   private GameObject _poi;
   private List<Vector3> points;

   private void Awake()
   {
      S = this;
      line = GetComponent<LineRenderer>(); //reference to line, by default disabled
      line.enabled = false;
      points = new List<Vector3>();
   }

   public GameObject poi
   {
      get { return _poi; }
      set
      {
         _poi = value;
         if (_poi != null)
         {
            line.enabled = false;
            points = new List<Vector3>();
            AddPoint();
         }
      }
   }

   public void Clear()
   //clear the line and remove the points
   {
      _poi = null;
      line.enabled = false;
      points = new List<Vector3>();
   }

   public void AddPoint()
   {
      Vector3 pt = _poi.transform.position;
      if (points.Count > 0 && (pt - lastPoint).magnitude < minDistance) return; //if point is not far enough away from previous, don't draw
      if (points.Count == 0)
      { //if initial point
         Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
         points.Add(pt + launchPosDiff); //add additional point to enable aiming
         points.Add(pt);
         line.SetPosition(0, points[0]);
         line.SetPosition(1, points[1]);
         line.enabled = true; //enable renderer
      }
      else
      {
         points.Add(pt);
         line.positionCount = points.Count;
         line.SetPosition(points.Count - 1, lastPoint);
         line.enabled = true;
      }
   }

   public Vector3 lastPoint
   {
      get
      {
         if (points == null)
         {
            // If there are no points, returns Vector3.zero
            return (Vector3.zero);
         }
         return (points[points.Count - 1]);
      }
   }

   void FixedUpdate()
   {
      if (poi == null)
      {
         // If there is no poi, search for one
         if (FollowCam.POI != null)
         {
            if (FollowCam.POI.tag == "Projectile")
            {
               poi = FollowCam.POI;
            }
            else
            {
               return; // Return if we didn't find a poi
            }
         }
         else
         {
            return; // Return if we didn't find a poi
         }
      }
      // If there is a poi, it's loc is added every FixedUpdate
      AddPoint();
      if (FollowCam.POI == null)
      {
         // Once FollowCam.POI is null, make the local poi nulll too
         poi = null;
      }
   }

}
