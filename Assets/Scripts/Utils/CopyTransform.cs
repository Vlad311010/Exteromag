using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public Transform copyFrom;
    public bool x, y, rotation;

    void Update()
    {
        Vector3 pos = new Vector3(x ? copyFrom.position.x : transform.position.x, y ? copyFrom.position.y : transform.position.y);
        transform.position = pos;
        transform.rotation = rotation ? copyFrom.rotation : transform.rotation;
    }
}
