using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapColorRestore : MonoBehaviour
{
    [SerializeField] Color color = Color.white;

    void Awake()
    {
        GetComponent<Tilemap>().color = color;
    }
}
