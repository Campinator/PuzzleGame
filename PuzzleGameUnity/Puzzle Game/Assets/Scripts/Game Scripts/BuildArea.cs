using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //libraries for accessing scenes

public class BuildArea : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] constructionMaterials;
    public int[] amounts;
    public Material doneMaterial;
    public Material tempMaterial;
    [HideInInspector]
    GameManager gm; //reference to game manager

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GM;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //rounds a given float to the nearest 0.5
    private float r(float input){
        return Mathf.Round(input * 2f) / 2f;
    }
    void OnMouseUpAsButton() {
        Vector3 mousePos2D = Input.mousePosition; //grab mouse position
        mousePos2D.z = -Camera.main.transform.position.z; //fix z coordinate
        Vector3 mp = Camera.main.ScreenToWorldPoint(mousePos2D);
        //round mouse position to the nearest 0.5 position 
        //variables shortened for ease of looking at the line, not for readability
        Vector3 mousePos = new Vector3(r(mp.x), r(mp.y), r(mp.z));
        GameObject block = Instantiate(constructionMaterials[0]) as GameObject;
        block.transform.position = mousePos;
        block.GetComponent<Renderer>().material = tempMaterial;
    }

    public void ResetBuild(){
        SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); //reload the current level
    }
    public void FinishBuilding(){
        foreach(GameObject block in GameObject.FindGameObjectsWithTag("BuildingBlock")){
            block.GetComponent<BuildingBlock>().EnablePhysics();
        }
        foreach(GameObject btn in GameObject.FindGameObjectsWithTag("GameButton")){
            btn.SetActive(false); 
        }
        //need to shoot bullets with a delay in between them
        //don't feel like doing a bunch of waiting and time comparison
        StartCoroutine(FireShots());
    }

    IEnumerator FireShots(){
        yield return new WaitForSeconds(2);
        Slingshot.Fire();
        yield return new WaitForSeconds(2);
        while(gm.Shots > 0){ 
            //holy hell
            while(GameObject.FindGameObjectWithTag("Slingshot").GetComponent<Slingshot>().projectile.GetComponent<Projectile>().IsActive()) yield return null;
            Slingshot.Fire();
        }
    }
}
