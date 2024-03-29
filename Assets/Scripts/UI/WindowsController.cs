using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WindowsController : MonoBehaviour
{
    [SerializeField] private WindowUI pauseWindow;
    [SerializeField] private WindowUI spellUpgradeWindow;
    [SerializeField] private WindowUI respawnWindow;

    private DefaultControls control;


    // state
    private WindowUI activeWindow = null;

    private void Awake()
    {
        control = new DefaultControls();
        control.Enable();

        control.menu.Esc.performed += EscapePressed;
    }
    private void OnDestroy()
    {
        control.menu.Esc.performed -= EscapePressed;
    }

    void Start()
    {
        GameEvents.current.onUpgradePickUp += OpenSpellUpgradeMenu;
        GameEvents.current.onPlayersDeath += OpenRespawnWindow;
        GameEvents.current.onExitTriggered += DisablePause;
    }

    private void DisablePause()
    {
        control.menu.Disable();
    }

    private void EscapePressed(InputAction.CallbackContext obj)
    {
        if (!obj.performed) return;
            
        if (activeWindow == null)
        {
            activeWindow = pauseWindow.OpenWindow(activeWindow);
        }
        else
        {
            activeWindow = activeWindow.CloseWindow(true);
        }
    }

    private void OpenSpellUpgradeMenu()
    {
        activeWindow = spellUpgradeWindow.OpenWindow(activeWindow);
    }

    private void OpenRespawnWindow()
    {
        activeWindow = respawnWindow.OpenWindow(activeWindow);
    }

    public void CloseWindow(WindowUI window)
    {
        activeWindow = window.CloseWindow(false);
    }

    public void OpenWindow(WindowUI window)
    {
        activeWindow = window.OpenWindow(activeWindow);
    }

    public void LoadMainMenu()
    {
        activeWindow.CloseWindow(false);
        activeWindow = null;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadeScene()
    {
        activeWindow.CloseWindow(false);
        activeWindow = null;
        SceneController.ReloadScene();
    }

    public void ReloadeLevel()
    {
        activeWindow.CloseWindow(false);
        activeWindow = null;
        SceneController.ReloadLevel();
    }

}
