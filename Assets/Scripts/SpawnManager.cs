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
        SpawnEnemy("bloater", new Vector3(46,0,42), new Vector3(31,0,28));
        SpawnEnemy("bloater", new Vector3(39,0,-6), new Vector3(21,0,-36));
        SpawnEnemy("bloater", new Vector3(51,0,-36), new Vector3(69,0,-54));
        SpawnEnemy("bloater", new Vector3(58,0,-24), new Vector3(45,0,-18));
        SpawnEnemy("bloater", new Vector3(15,0,-36), new Vector3(11,0,-43));
        SpawnEnemy("bloater", new Vector3(3,0,-25), new Vector3(11,0,-17));
        SpawnEnemy("bloater", new Vector3(-21,0,6), new Vector3(-39,0,-24));
        SpawnEnemy("bloater", new Vector3(-21,0,18), new Vector3(-21,0,60));
        SpawnEnemy("bloater", new Vector3(-57,0,5), new Vector3(-57,0,18));
        SpawnEnemy("bloater", new Vector3(-69,0,6), new Vector3(-81,0,19));
        SpawnEnemy("bloater", new Vector3(-3,0,30), new Vector3(15,0,48));
        SpawnEnemy("bloater", new Vector3(9,0,84), new Vector3(-27,0,78));
        SpawnEnemy("bloater", new Vector3(-45,0,60), new Vector3(-63,0,101));
        SpawnEnemy("bloater", new Vector3(-42,0,49), new Vector3(-48,0,35));

        SpawnEnemy("bloater", new Vector3(-39,12,18), new Vector3(-3,12,0));
        SpawnEnemy("bloater", new Vector3(21,12,25), new Vector3(39,12,36));
        SpawnEnemy("bloater", new Vector3(21,6,-36), new Vector3(39,6,36));
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
            // GameObject shriekerInstance = Instantiate(shriekerPrefab, startPoint, shriekerPrefab.transform.rotation);
            // shriekerScript = shriekerInstance.GetComponent<ShriekerBehaviour>();
            // shriekerScript.startPoint = centrePoint;
            // shriekerScript.endPoint = maxRadius;  
        }
    }
}