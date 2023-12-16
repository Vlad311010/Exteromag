using UnityEngine;

public class UpgradePickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameEvents.current.UpgradePickUp();
            Destroy(this.gameObject);
        }
    }
}
