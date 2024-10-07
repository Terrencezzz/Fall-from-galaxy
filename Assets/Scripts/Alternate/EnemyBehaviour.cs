using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // List of vectors for the enemy to follow 
    private List<Vector3> vectorList = new List<Vector3>(); 
    private Vector3 spawnPosition;
    private Vector3 direction;
    private int speed = 2;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
