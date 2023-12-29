using UnityEngine;

public class StatsChangeZone : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int mp;
    
    [SerializeField] bool deastroy = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneController.characterStats.hp = hp;
            SceneController.characterStats.mana = mp;
            collision.GetComponent<CharacterRespawn>().ResetStats();
            if (deastroy)
                Destroy(this.gameObject);
        }
    }
}
