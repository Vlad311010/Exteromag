using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Interfaces;

public class CharacterInteraction : MonoBehaviour, IDestroyable
{
    [Header("Inputs")]
    public DefaultControls control;

    [Header("Parameters")]
    [SerializeField] private SpellScriptableObject[] spellsSO;

    [Header("References")]
    private CharacterAim aim;
    private CharacterEffects effects; 
    private CharacterLimitations limitations; 
    private ManaPool mana;
    Camera camera;


    private SpellContainer[] spells;
    private int selectedSpell;

    private void Start()
    {
        aim = GetComponentInChildren<CharacterAim>();
        effects = GetComponentInChildren<CharacterEffects>();
        limitations = GetComponentInChildren<CharacterLimitations>();
        mana = GetComponent<ManaPool>();
        camera = Camera.main;

        spells = new SpellContainer[spellsSO.Length];
        for (int i = 0; i < spellsSO.Length; i++)
        {
            spells[i] = new SpellContainer(spellsSO[i], i);
        }
    }

    void OnEnable()
    {
        control = new DefaultControls();
        control.Enable();
        
        if (aim != null)
            aim.enabled = true;

        control.gameplay.Action.started += OnAction;
        control.gameplay.Action.canceled += OnAction;

        control.gameplay.RestartLevel.performed += OnRestart;
    }


    void OnDisable()
    {
        control.Disable();
        
        if (aim != null)
            aim.enabled = false;

        control.gameplay.Action.started -= OnAction;
        control.gameplay.Action.canceled -= OnAction;

        control.gameplay.RestartLevel.performed -= OnRestart;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedSpell = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedSpell = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedSpell = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            selectedSpell = 3;

        if (!spells[selectedSpell].IsEmpty() && spells[selectedSpell].holdDown && !spells[selectedSpell].isInCooldown)
        {
            if (spells[selectedSpell].spell.preventConstantCasting)
                spells[selectedSpell].holdDown = false;

            if (mana.HaveEnaughtMp(spells[selectedSpell].spell.castCost))
            {
                mana.Consume(spells[selectedSpell].spell.castCost);
                StartCoroutine(spells[selectedSpell].SpellCooldown());
                SpellCasting.Cast(transform, spells[selectedSpell].spell, aim.TakeSnapshot());
            }
        }
    }

    private void OnAction(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            GetComponentInChildren<Animator>().SetBool("Casting", true);
            spells[selectedSpell].holdDown = true;
            GameEvents.current.SpellCastButtonHold(true, selectedSpell);
            
        }
        else if (obj.canceled)
        {
            GetComponentInChildren<Animator>().SetBool("Casting", false);
            spells[selectedSpell].holdDown = false;
            GameEvents.current.SpellCastButtonHold(false, selectedSpell);
        }

    }

    private void OnActionAlt(InputAction.CallbackContext obj)
    {

    }

    private void OnRestart(InputAction.CallbackContext obj)
    {
        GameEvents.current.SceneLoad(SceneManager.GetActiveScene().buildIndex);
    }

    public void DestroyObject()
    {
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.enabled = false;
        }
        limitations.DisableActions();
        effects.CreateSplashEffect();
        GameEvents.current.PlayerDied();
        Debug.Log("PLAYER DEATH");
    }
}
