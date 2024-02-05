using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RespawnWindow : MonoBehaviour
{
    public DefaultControls control { get; private set; }

    void Awake()
    {
        control = new DefaultControls();

        control.menu.Respawn.performed += Respawn;
        control.menu.Esc.performed += Exit;
    }

    private void OnEnable()
    {
        control.Enable();
    }

    private void OnDisable()
    {
        control.Disable();
    }

    private void Respawn(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            SceneController.ReloadScene();
        }
    }

    private void Exit(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

}
