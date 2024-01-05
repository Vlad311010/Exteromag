using UnityEngine;

public class UISoundDeligetion : MonoBehaviour
{
    UIPlayer sound;

    void Start()
    {
        sound = GetComponentInParent<UIPlayer>();
    }

    public void Click1()
    {
        sound?.Click1();
    }

    public void Click2()
    {
        sound?.Click2();
    }

    public void Click3()
    {
        sound?.Click3();
    }

    public void Drag1()
    {
        sound?.Drag1();
    }

    public void Drag2()
    {
        sound?.Drag2();
    }

}
