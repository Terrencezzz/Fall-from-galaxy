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
        SpawnEnemy("bloater", new Vector3(63,0,77), new Vector3(49,0,77));
        SpawnEnemy("bloater", new Vector3(58,0,63), new Vector3(40,0,60));
        SpawnEnemy("bloater", new Vector3(47,0,65), new Vector3(47,0,89));
        SpawnEnemy("bloater", new Vector3(-39,12,18), new Vector3(-3,12,0));
        SpawnEnemy("bloater", new Vector3(21,12,25), new Vector3(39,12,-36));
        SpawnEnemy("bloater", new Vector3(21,6,-36), new Vector3(39,6,-49));
        SpawnEnemy("bloater", new Vector3(48,-12,17), new Vector3(39,-12,18));
        SpawnEnemy("bloater", new Vector3(39,-12,20), new Vector3(40,-12,26));
        SpawnEnemy("bloater", new Vector3(38,-12,27), new Vector3(40,-12,37));
        SpawnEnemy("bloater", new Vector3(41,-12,47), new Vector3(47,-12,46));
        SpawnEnemy("bloater", new Vector3(59,-12,46), new Vector3(67,-12,48));
        SpawnEnemy("bloater", new Vector3(67,-12,41), new Vector3(69,-12,34));
        SpawnEnemy("bloater", new Vector3(69,-12,32), new Vector3(57,-12,27));
        SpawnEnemy("bloater", new Vector3(68,-12,17), new Vector3(59,-12,19));

        SpawnEnemy("stalker", new Vector3(-57,0,42), new Vector3(-10,0,83));
        SpawnEnemy("stalker", new Vector3(26,0,65), new Vector3(-21,0,23));
        SpawnEnemy("stalker", new Vector3(-63,0,11), new Vector3(-32,0,-25));
        SpawnEnemy("stalker", new Vector3(74,0,-6), new Vector3(68,0,-37));
        SpawnEnemy("stalker", new Vector3(39,6,-54), new Vector3(-4,12,36));
        SpawnEnemy("stalker", new Vector3(47,9,90), new Vector3(47,9,67));
        SpawnEnemy("stalker", new Vector3(32,4.6f,59), new Vector3(15,0,66));
        SpawnEnemy("stalker", new Vector3(69,-12,19), new Vector3(58,-12,48));
        SpawnEnemy("stalker", new Vector3(45,-12,48), new Vector3(47,-12,18));

        SpawnEnemy("shrieker", new Vector3(88,0,-8), new Vector3(-62,0,42));
        SpawnEnemy("shrieker", new Vector3(25,0,66), new Vector3(68,0,-54));
        SpawnEnemy("shrieker", new Vector3(40,-32,18), new Vector3(68,-32,47));
        SpawnEnemy("shrieker", new Vector3(-47,-19.7f,88), new Vector3(-44,-19.7f,91));
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
            stalkerScript.startPoint = startPoint;
            stalkerScript.endPoint = endPoint;  
        }
    }
}