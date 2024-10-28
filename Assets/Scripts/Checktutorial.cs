using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checktutorial : MonoBehaviour
{
    public InteractionController interactionController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionController.robotCount == 1)
        {
            SceneManager.LoadScene(2);
        }
    }
}
