using Cinemachine;
using System.Collections;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [SerializeField] Color color; 
    [SerializeField] GameObject splash;
    [SerializeField] GameObject onDeathEffect;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    
    private CinemachineBasicMultiChannelPerlin cameraNoise;

    private void Start()
    {
        cinemachine = GameObject.FindGameObjectWithTag("CM").GetComponent<CinemachineVirtualCamera>();
        cameraNoise = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void OnDeathEffects()
    {
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
