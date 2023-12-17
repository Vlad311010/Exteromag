using UnityEngine;
using UnityEngine.InputSystem;

public class WindowsController : MonoBehaviour
{
    [SerializeField] WindowUI pauseWindow;
    [SerializeField] WindowUI spellUpgradeWindow;

    private DefaultControls control;


    // state
    private WindowUI activeWindow = null;

    private void Awake()
    {
        control = new DefaultControls();
        control.Enable();

        control.menu.Esc.performed += EscapePressed;
    }

    void Start()
    {
        GameEvents.current.onUpgradePickUp += OpenSpellUpgradeMenu;
    }


    private void EscapePressed(InputAction.CallbackContext obj)
    {
        if (!obj.performed) return;
            
        // WindowUI activeWindow = GameObject.FindGameObjectWithTag("WindowUI").GetComponent<WindowUI>(); // mb replace with windowIsOpen
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

    public void CloseWindow(WindowUI window)
    {
        activeWindow = window.CloseWindow(false);
    }

}
