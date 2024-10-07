using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewDetection : MonoBehaviour
{
    public Transform player;
    public float maxAngle = 45.0f;
    public float maxRadius = 15.0f;
    private bool isInFov = false;

    public static bool InFOV(Transform enemyObject, Transform target, float maxAngle, float maxRadius) {
        Collider[] overlaps = new Collider[100]; // returns how many objects are within enemy radius  
        int count = Physics.OverlapSphereNonAlloc(enemyObject.position, maxRadius, overlaps); // checks objects within given AI radius
        // check if objects in enemy radius are in fact the player 
        for (int i = 0; i < count + 1; i++) { 
            if (overlaps[i] != null) {
                if (overlaps[i].transform == target) {
                    Vector3 directionBetween = (target.position - enemyObject.position).normalized;
                    directionBetween.y *= 0; // ensures we ignore height as a factor in the angle
                    float angle = Vector3.Angle(enemyObject.forward, directionBetween);
                    // check if we are 'in front' of enemies fov 
                    if (angle <= maxAngle) { 
                        Ray ray = new Ray(enemyObject.position, enemyObject.position - target.position);
                        RaycastHit hit;
                        // constrain enemy view within radius 
                        if (Physics.Raycast(ray, out hit, maxRadius)) {
                            if (hit.transform == target) {
                                return true;
                            }
                        }
                    }

                }
            }
        }
        return false;
    } 

    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update() {
        isInFov = InFOV(transform, player, maxAngle, maxRadius);
        if(isInFov) {
            Debug.Log("In FOV!");
        }
    }
}
