using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEntranceScript : MonoBehaviour
{
    [SerializeField] int levelToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterRespawn>().Respawn();
            GameEvents.current.NextLevelLoad();
            SceneManager.LoadScene(string.Format("Level_{0}_1", levelToLoad));
            GetComponent<Collider2D>().enabled = false;
        }


    }


}
