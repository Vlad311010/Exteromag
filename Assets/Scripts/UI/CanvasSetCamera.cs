using UnityEngine;

public class CanvasSetCamera : MonoBehaviour
{
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
