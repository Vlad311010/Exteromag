using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManaPool : MonoBehaviour
{
    [SerializeField] private int maxMp;
    [SerializeField] private int restoreAmount = 1;
    [SerializeField] public float restorationTime { get; private set; }
    
    private int currentMp;
    
    
    private void Start()
    {
        currentMp = maxMp;
        Consume(0);
    }

    public bool HaveEnaughtMp(int toConsume)
    {
        return currentMp >= toConsume;
    }


    public void Consume(int amount)
    {
        // StopAllCoroutines();
        currentMp = Mathf.Clamp(currentMp - amount, 0, maxMp);
        GameEvents.current.ManaChange(currentMp, maxMp);
        // StartCoroutine(Restore());
    }

    IEnumerator Restore()
    {
        yield return new WaitForSeconds(restorationTime);
        Consume(-restoreAmount);
        // currentMp = Mathf.Clamp(currentMp + restoreAmount, 0, maxMp);
        if (currentMp < maxMp)
            StartCoroutine(Restore());

    }
}
