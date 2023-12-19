using UnityEngine;

public class WindowUI : MonoBehaviour
{
    
    public bool closableByEsc = true;

    private WindowUI ancestor;

    public WindowUI OpenWindow(WindowUI activeWindow)
    {
        ancestor = activeWindow;
        ancestor?.SetActive(false);
        SceneController.Pause();
        // open window
        gameObject.SetActive(true);
        
        return this;
    }

    public WindowUI CloseWindow(bool escPress)
    {
        if (!escPress)
            return CloseWindow();
        else if (escPress && closableByEsc)
            return CloseWindow();

        return this;
    }

    private WindowUI CloseWindow()
    {
        /*if (unpauseOnClosing)
            SceneController.Resume();*/
        WindowUI activeWindow;
        if (ancestor == null)
        {
            SceneController.Resume();
            activeWindow = null;
        }
        else
        {
            ancestor.SetActive(true);
            activeWindow = ancestor;
        }

        // close window
        gameObject.SetActive(false);
        return activeWindow;
    }

    public void SetActive(bool active)
    {

        // set raycast target = false for every child (image component)
    }
}
