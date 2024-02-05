using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WindowsControllerMainMenu : MonoBehaviour
{
    [SerializeField] private WindowUI mainMenu;
    [SerializeField] private WindowUI options;
    [SerializeField] private WindowUI credits;

    private DefaultControls control;


    // state
    public static SettingsSO gameSettings;
    private WindowUI activeWindow = null;

    private void Awake()
    {
        control = new DefaultControls();
        control.Enable();

        control.menu.Esc.performed += EscapePressed;

        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        control.menu.Esc.performed -= EscapePressed;
    }

    void Start()
    {
        activeWindow = mainMenu;
        gameSettings = ScriptableObject.CreateInstance("SettingsSO") as SettingsSO;
    }


    private void EscapePressed(InputAction.CallbackContext obj)
    {
        if (!obj.performed) return;

        activeWindow = activeWindow.CloseWindow(true);

    }

    public void OpenSettingsWindow()
    {
        activeWindow = options.OpenWindow(activeWindow);
    }

    public void OpenCreditsMenu()
    {
        activeWindow = credits.OpenWindow(activeWindow);
    }

    public void Play()
    {
        SceneManager.LoadScene("Hub");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LanguageSwitch()
    {
        if (Translator.selectedLangage == SystemLanguage.English)
            Translator.selectedLangage = SystemLanguage.Ukrainian;
        else
            Translator.selectedLangage = SystemLanguage.English;
    }

    public void CloseWindow(WindowUI window)
    {
        activeWindow = window.CloseWindow(false);
    }
}
