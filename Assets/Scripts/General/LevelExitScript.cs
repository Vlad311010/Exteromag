using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameEvents.current.UpgradePickUp();
            StartCoroutine(LoadNextLevel());    
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator LoadNextLevel()
    {
        GameEvents.current.ExitTriggered();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(Extensions.GetNextLevelName());   
    }
}
