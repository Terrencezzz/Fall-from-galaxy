using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{  
    // Enemy prefabs 
    public GameObject bloaterPrefab; // bloaters are fat and slow. Conversely, they deal and can take significant damage
    public GameObject stalkerPrefab; // stalkers are quiet and more persistent. they have larger 'patrol' areas and search for the player more thoroughly given their last location 
    public GameObject shriekerPrefab; // shreikers are large, fast, and loud. only known method of deterrence is to run, hide and pray. 

    // Enemy behaviour scripts 
    private BloaterBehaviour bloaterScript;
    private StalkerBehaviour stalkerScript;
    private ShriekerBehaviour shriekerScript;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    // Spawns enemy 
    void SpawnEnemy(string enemyType, Vector3 startPoint, Vector3 endPoint) {
        if (enemyType == "bloater") {
            GameObject bloaterInstance = Instantiate(bloaterPrefab, startPoint, bloaterPrefab.transform.rotation);
            bloaterScript = bloaterInstance.GetComponent<BloaterBehaviour>();
            bloaterScript.startPoint = startPoint;
            bloaterScript.endPoint = endPoint;
        } else if (enemyType == "stalker") {
            GameObject stalkerInstance = Instantiate(stalkerPrefab, startPoint, stalkerPrefab.transform.rotation);
            stalkerScript = stalkerInstance.GetComponent<StalkerBehaviour>();
            stalkerScript.startPoint = startPoint;
            stalkerScript.endPoint = endPoint;  
        } else if (enemyType == "shrieker") {
            GameObject shriekerInstance = Instantiate(shriekerPrefab, startPoint, shriekerPrefab.transform.rotation);
            shriekerScript = shriekerInstance.GetComponent<ShriekerBehaviour>();
            shriekerScript.startPoint = startPoint;
            shriekerScript.endPoint = endPoint;  
        }
    }
}
