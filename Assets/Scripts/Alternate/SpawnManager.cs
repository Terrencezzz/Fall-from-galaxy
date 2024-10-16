using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject bloaterPrefab; // bloaters are fat and slow. Conversely, they deal and can take significant damage
    public GameObject stalkerPrefab; // stalkers are quiet and more persistent. they have larger 'patrol' areas and search for the player more thoroughly given their last location 
    public GameObject walkerPrefab; // walkers are the quintessential infected. slow, low damage. however, they tend to swarm
    public GameObject shriekerPrefab; // shreikers are large, fast, and loud. only known method of deterrence is to run, hide and pray. 

    private BloaterBehaviour bloaterScript;

    // Start is called before the first frame update
    void Start()
    {
        // SpawnEnemy("bloater", new Vector3 (39,0,36), new Vector3 (14,0,12));
        SpawnEnemy("bloater", new Vector3 (39,0,36), new Vector3 (39,0,28));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    // Spawns 
    void SpawnEnemy(string enemyType, Vector3 startPoint, Vector3 endPoint) {
        if (enemyType == "bloater") {
            GameObject bloaterInstance = Instantiate(bloaterPrefab, startPoint, bloaterPrefab.transform.rotation);
            bloaterScript = bloaterInstance.GetComponent<BloaterBehaviour>();
            bloaterScript.startPoint = startPoint;
            bloaterScript.endPoint = endPoint;
        } else if (enemyType == "stalker") {
            // GameObject bloaterInstance = Instantiate(bloaterPrefab, startPoint, bloaterPrefab.transform.rotation);
            // bloaterScript = bloaterInstance.GetComponent<BloaterBehaviour>();
            // bloaterScript.startPoint = startPoint;
            // bloaterScript.endPoint = endPoint;  
        }
    }
}
