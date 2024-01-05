using UnityEngine;

public class UIPlayer : SFXPlayer
{
    [SerializeField] AudioClip click1;
    [SerializeField] AudioClip click2;
    [SerializeField] AudioClip click3;

    [SerializeField] AudioClip drag1;
    [SerializeField] AudioClip drag2;



    public void Click1()
    {
        Play(click1);
    }

    public void Click2()
    {
        Play(click2);
    }

    public void Click3()
    {
        Play(click1);
    }

    public void Drag1()
    {
        Play(drag1);
    }

    public void Drag2()
    {
        Play(drag2);
    }


}
