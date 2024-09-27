using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;

    public void Init(bool floorActive, bool top, bool bottom, bool left, bool right)
    {
        if (floor != null) floor.SetActive(floorActive);
        if (topWall != null) topWall.SetActive(top);
        if (bottomWall != null) bottomWall.SetActive(bottom);
        if (leftWall != null) leftWall.SetActive(left);
        if (rightWall != null) rightWall.SetActive(right);
    }
}
