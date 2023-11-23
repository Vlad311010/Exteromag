using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Interfaces;

public class CharacterInteraction : MonoBehaviour, IDestroyable
{
    [Header("Inputs")]
    public DefaultControls control;

    // [Header("Parameters")]


    // [Header("Current State")]

    void OnEnable()
    {
        control = new DefaultControls();
        control.Enable();

        control.gameplay.RestartLevel.performed += OnRestart;
    }


    void OnDisable()
    {
        control.Disable();

        control.gameplay.RestartLevel.performed -= OnRestart;
    }


    private void OnRestart(InputAction.CallbackContext obj)
    {
        GameEvents.current.SceneLoad(SceneManager.GetActiveScene().buildIndex);
    }

    public void DestroyObject()
    {
        // 
        // throw new System.NotImplementedException();
    }
}
