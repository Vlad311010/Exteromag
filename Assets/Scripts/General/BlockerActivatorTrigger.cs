using UnityEngine;

public class BlockerActivatorTrigger : MonoBehaviour
{
    [SerializeField] int id;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameEvents.current.BlockerActivation(id);
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Color color = new Color(0, 234, 255, 0.45f);
        Gizmos.color = color;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

}
