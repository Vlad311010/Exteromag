using Cinemachine;
using System.Collections;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    // visual 
    // health sacrifice
    [SerializeField] GameObject manaParticles;

    // hit
    [SerializeField] Material hitMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] float hitHighlightTime;

      // death
    [SerializeField] Color color; 
    [SerializeField] GameObject splash;
    [SerializeField] GameObject onDeathEffect;



    // sound
    [SerializeField] AudioClip CastFailedSound;
    [SerializeField] AudioClip healtSacrificeSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip deathSound;

    // camera 
    [SerializeField] CinemachineVirtualCamera cinemachine;
    private CinemachineBasicMultiChannelPerlin cameraNoise;

    private SFXPlayer sound;
    private SpriteRenderer[] sprites;
    private Coroutine hitHighlightCoroutine;

    private void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        cinemachine = GameObject.FindGameObjectWithTag("CM").GetComponent<CinemachineVirtualCamera>();
        cameraNoise = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        sound = GetComponentInChildren<SFXPlayer>();
    }

    public void OnHealthSacrifice()
    {
        Instantiate(manaParticles, transform.position, Quaternion.identity);
        sound.Play(healtSacrificeSound);
    } 

    public void CastFailded()
    {
        if (sound.Clip != CastFailedSound || !sound.IsPlaying) 
            sound.Play(CastFailedSound);
    }

    public void OnHit()
    {
        CameraShake(0.4f);
        if (hitHighlightCoroutine != null)
        {
            StopCoroutine(hitHighlightCoroutine);
        }
        hitHighlightCoroutine = StartCoroutine(HitHighlight());
        sound.Play(hitSound);
    }

    IEnumerator HitHighlight()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material = hitMaterial;
        }

        yield return new WaitForSeconds(hitHighlightTime);

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material = defaultMaterial;
        }
    }

    public void OnDeathEffects()
    {
        sound.Play(deathSound);
        Instantiate(onDeathEffect, transform.position, transform.rotation).GetComponent<SpriteRenderer>();
        // Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 359));
        // SpriteRenderer splashSprite = Instantiate(splash, transform.position, rotation).GetComponent<SpriteRenderer>();
        // splashSprite.color = color;
    }

    public void CameraShake(float duration, float magnitude = 11, float frequency = 0.07f)
    {
        cameraNoise.m_AmplitudeGain = magnitude;
        cameraNoise.m_FrequencyGain = frequency;
        StartCoroutine(CameraShakeCoroutine(magnitude, duration, duration));
    }

    IEnumerator CameraShakeCoroutine(float originalMagnitude, float duration, float timer)
    {
        cameraNoise.m_AmplitudeGain = originalMagnitude * (timer / duration);
        timer -= Time.deltaTime;
        yield return new WaitForEndOfFrame();
        if (timer > 0)
            StartCoroutine(CameraShakeCoroutine(originalMagnitude, duration, timer));
        else
        {
            cameraNoise.m_AmplitudeGain = 0f;
            cameraNoise.m_FrequencyGain = 0f;
        }
    }


}
