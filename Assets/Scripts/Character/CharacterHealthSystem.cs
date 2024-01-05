using System.Collections;
using UnityEngine;
using Interfaces;
using UnityEngine.SceneManagement;

public class CharacterHealthSystem : MonoBehaviour, IHealthSystem, IResatable
{
    public int CurrentHealth { get => currentHp; }
    public int MaxHealth { get => maxHp; }

    Rigidbody2D rigidbody;
    CharacterEffects effects;
    CharacterLimitations limitations;

    [Header("Parameters")]
    [SerializeField] int maxHp;

    [Header("Options")]
    [SerializeField] bool staggerable;
    [SerializeField] float staggerTime;

    private int currentHp;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        effects = GetComponent<CharacterEffects>();
        limitations = GetComponent<CharacterLimitations>();

        currentHp = maxHp;
        ConsumeHp(0, Vector2.zero, false);
        SceneManager.sceneLoaded += UpdateUI;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= UpdateUI;
    }


    public void ConsumeHp(int amount, Vector2 staggerDirectiom, bool noDamageEffect = false)
    {
        currentHp = System.Math.Clamp(currentHp - amount, 0, maxHp);
        GameEvents.current.HealthChangePlayer(currentHp, maxHp);
        if (currentHp <= 0)
            GetComponent<IDestroyable>().DestroyObject();
        else if (!noDamageEffect && amount > 0)
        {
            effects.OnHit();

            if (staggerable)
                StartCoroutine(Stagger(staggerTime, staggerDirectiom));
        }
    }


    IEnumerator Stagger(float time, Vector2 direction)
    {
        limitations.ActivateMovementContraint();
        rigidbody.velocity = direction * 350 * Time.fixedDeltaTime;
        yield return new WaitForSeconds(time);
        rigidbody.velocity = Vector2.zero;
        limitations.DisableMovementContraint();
    }

    
    public void ResetValues()
    {
        currentHp = SceneController.characterStats.hp;
        GameEvents.current.HealthChangePlayer(currentHp, maxHp);
    }

    private void UpdateUI(Scene scene, LoadSceneMode mode)
    {
        GameEvents.current.HealthChangePlayer(currentHp, maxHp);
    }

    public override string ToString()
    {
        return string.Format("{0}/{1}", currentHp, maxHp);
    }
}
