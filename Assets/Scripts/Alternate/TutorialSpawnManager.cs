using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawnManager : MonoBehaviour
{
    public GameObject bloaterPrefab; // bloaters are fat and slow. Conversely, they deal and can take significant damage
    public GameObject stalkerPrefab; // stalkers are quiet and more persistent. they have larger 'patrol' areas and search for the player more thoroughly given their last location 
    public GameObject walkerPrefab; // walkers are the quintessential infected. slow, low damage. however, they tend to swarm
    public GameObject shriekerPrefab; // shreikers are large, fast, and loud. only known method of deterrence is to run, hide and pray. 

    private BloaterBehaviour bloaterScript;
    private StalkerBehaviour stalkerScript;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy("bloater", new Vector3 (13,0,33), new Vector3 (-9,4.5f,15));
        SpawnEnemy("stalker", new Vector3 (13,-5.5f,81), new Vector3 (13,-5.5f,81));
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
            GameObject stalkerInstance = Instantiate(stalkerPrefab, startPoint, stalkerPrefab.transform.rotation);
            stalkerScript = stalkerInstance.GetComponent<StalkerBehaviour>();
            stalkerScript.startPoint = startPoint;
            stalkerScript.endPoint = endPoint;  
        }
    }
}
