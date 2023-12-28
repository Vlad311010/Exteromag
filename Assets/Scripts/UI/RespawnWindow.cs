using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnWindow : MonoBehaviour
{
    public DefaultControls control { get; private set; }

    void Awake()
    {
        control = new DefaultControls();

        control.menu.Respawn.performed += Respawn;
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

}
