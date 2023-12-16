using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshController : MonoBehaviour
{
    NavMeshSurface navMesh; 
    
    void Start()
    {
        navMesh = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            navMesh.BuildNavMeshAsync();
        }
    }
}
