/**
 * Date Created: March 2. 2022
 * Created By: Camp Steiner
 * 
 * Date Modified: March 7, 2022
 * Modified by: Camp Steiner
 * 
 * Description: The main handler for users clicking on the build area. Places blocks where the user clicks
 * 
 */
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; //libraries for accessing scenes
using UnityEngine.UI;

public class BuildArea : MonoBehaviour
{
   [Header("Set in Inspector")]
   public GameObject[] constructionMaterials;
   public int[] amounts;
   public Text[] txts;
   public Material doneMaterial;
   public Material tempMaterial;
   public Material ghostMaterial;

   private int curMaterialIdx = 0;
   private GameObject silhouette;

   [HideInInspector]
   GameManager gm; //reference to game manager
   private bool buildMode = true;

   // Start is called before the first frame update
   void Start()
   {
      gm = GameManager.GM;
      UpdateGUI();
      SetActiveMaterial(curMaterialIdx);
   }

   void UpdateGUI()
   {
      for (int i = 0; i < txts.Length; i++)
      {
         txts[i].text = "x" + amounts[i];
      }
      int money = 0;
      foreach (int i in amounts)
      {
         money += i;
      }
      gm.Score = money * 100;
   }

   public void SetActiveMaterial(int i)
   {
      Destroy(silhouette);
      curMaterialIdx = i;
      silhouette = Instantiate(constructionMaterials[curMaterialIdx]) as GameObject;
      silhouette.GetComponent<Renderer>().material = ghostMaterial;
      silhouette.SetActive(false);
   }

   //rounds a given float to the nearest 0.5
   void OnMouseUpAsButton()
   {
      if (!buildMode || amounts[curMaterialIdx] == 0) return;
      Vector3 mousePos = GetMouseCoords();

      GameObject block = Instantiate(constructionMaterials[curMaterialIdx]) as GameObject;
      block.transform.position = mousePos;
      block.GetComponent<Renderer>().material = tempMaterial;
      amounts[curMaterialIdx]--;
      UpdateGUI();
   }
   void OnMouseOver()
   {
      if (buildMode) silhouette.SetActive(true);
   }
   void OnMouseExit()
   {
      silhouette.SetActive(false);
   }
   void Update()
   {
      if (!silhouette) return;
      silhouette.transform.position = GetMouseCoords();
      int layerObject = 50;
      Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
      RaycastHit2D hit = Physics2D.Raycast(ray, ray, layerObject);
      if (hit.collider != null)
      {
         Debug.Log(hit.collider.gameObject.name);
      }
   }

   private float r(float input)
   {
      return Mathf.Round(input * 2f) / 2f + 0.5f;
   }
   private Vector3 GetMouseCoords()
   {
      Vector3 mousePos2D = Input.mousePosition; //grab mouse position
      mousePos2D.z = -Camera.main.transform.position.z; //fix z coordinate
      Vector3 mp = Camera.main.ScreenToWorldPoint(mousePos2D);
      //round mouse position to the nearest 0.5 position 
      //variables shortened for ease of looking at the line, not for readability
      Vector3 mousePos = new Vector3(r(mp.x), r(mp.y), r(mp.z));
      return mousePos;
   }

   public void ResetBuild()
   {
      SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); //reload the current level
   }
   public void FinishBuilding()
   {
      //make the blocks real and not just construction items
      foreach (GameObject block in GameObject.FindGameObjectsWithTag("BuildingBlock"))
      {
         block.GetComponent<BuildingBlock>().EnablePhysics();
         block.GetComponent<Renderer>().material = doneMaterial;
      }
      //hide the buttons
      foreach (GameObject btn in GameObject.FindGameObjectsWithTag("GameButton"))
      {
         btn.SetActive(false);
      }
      //don't let user place any more block
      buildMode = false;
      //need to shoot bullets with a delay in between them
      //don't feel like doing a bunch of waiting and time comparison
      StartCoroutine(FireShots());
   }

   IEnumerator FireShots()
   {
      yield return new WaitForSeconds(2);
      Slingshot.Fire();
      yield return new WaitForSeconds(2);
      while (gm.Shots > 0)
      {
         //holy hell
         yield return new WaitUntil(() => !GameObject.FindGameObjectWithTag("Slingshot").GetComponent<Slingshot>().projectile.GetComponent<Projectile>().IsActive());
         Slingshot.Fire();
         yield return new WaitForSeconds(3);
      }
   }

}
