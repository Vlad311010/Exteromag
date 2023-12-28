using UnityEngine;
using UnityEngine.VFX;

public class VFXDestroy : MonoBehaviour
{
    VisualEffect effect;
    private float minimalLifeTime = 0.5f;

    void Start()
    {
        effect = GetComponent<VisualEffect>();
    }

    void Update()
    {
        
        if (minimalLifeTime <= 0 && effect.aliveParticleCount == 0)
        {
            Destroy(effect.gameObject);
        }
        else
        {
            minimalLifeTime -= Time.deltaTime;
        }
    }
}
