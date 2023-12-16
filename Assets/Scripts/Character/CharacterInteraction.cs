using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Interfaces;

public class CharacterInteraction : MonoBehaviour, IDestroyable
{
    public DefaultControls control { get; private set; }

    [Header("Parameters")]
    // [SerializeField] private SpellScriptableObject[] spellsSO;
    [SerializeField] private SpellSetSO[] spellsSets;

    [Header("References")]
    private CharacterAim aim;
    private CharacterEffects effects; 
    private CharacterLimitations limitations; 
    private ManaPool mana;
    Camera camera;


    private SpellContainer[] containedSpells;
    private int selectedSpell;

    private void Start()
    {
        aim = GetComponentInChildren<CharacterAim>();
        effects = GetComponentInChildren<CharacterEffects>();
        limitations = GetComponentInChildren<CharacterLimitations>();
        mana = GetComponent<ManaPool>();
        camera = Camera.main;

        containedSpells = new SpellContainer[spellsSets.Length];
        for (int i = 0; i < spellsSets.Length; i++)
        {
            containedSpells[i] = new SpellContainer(spellsSets[i], i);
        }
    }

    void OnEnable()
    {
        control = new DefaultControls();
        control.Enable();


        if (aim != null)
            aim.enabled = true;

        control.gameplay.Cast0.started += OnCast0;
        control.gameplay.Cast0.canceled += OnCast0;
        control.gameplay.Cast1.started += OnCast1;
        control.gameplay.Cast1.canceled += OnCast1;
        control.gameplay.Cast2.started += OnCast2;
        control.gameplay.Cast2.canceled += OnCast2;

        control.menu.Esc.performed += EscapePressed;

        control.gameplay.RestartLevel.performed += OnRestart;
    }


    void OnDisable()
    {
        control.Disable();
        
        if (aim != null)
            aim.enabled = false;

        control.gameplay.Cast0.started -= OnCast0;
        control.gameplay.Cast0.canceled -= OnCast0;
        control.gameplay.Cast1.started -= OnCast1;
        control.gameplay.Cast1.canceled -= OnCast1;
        control.gameplay.Cast2.started -= OnCast2;
        control.gameplay.Cast2.canceled -= OnCast2;

        control.gameplay.RestartLevel.performed -= OnRestart;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedSpell = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedSpell = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedSpell = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            selectedSpell = 3;*/

        if (!containedSpells[selectedSpell].IsEmpty() && containedSpells[selectedSpell].holdDown && !containedSpells[selectedSpell].isInCooldown)
        {
            if (containedSpells[selectedSpell].spell.preventConstantCasting)
                containedSpells[selectedSpell].holdDown = false;

            if (mana.HaveEnaughtMp(containedSpells[selectedSpell].spell.castCost))
            {
                mana.Consume(containedSpells[selectedSpell].spell.castCost);
                StartCoroutine(containedSpells[selectedSpell].SpellCooldown());
                SpellCasting.Cast(transform, containedSpells[selectedSpell].spell, aim.TakeSnapshot());
            }
        }
    }

    private void ProcessCastInput(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            GetComponentInChildren<Animator>().SetBool("Casting", true);
            containedSpells[selectedSpell].holdDown = true;
            GameEvents.current.SpellCastButtonHold(true, selectedSpell);

        }
        else if (obj.canceled)
        {
            GetComponentInChildren<Animator>().SetBool("Casting", false);
            containedSpells[selectedSpell].holdDown = false;
            GameEvents.current.SpellCastButtonHold(false, selectedSpell);
        }
    }

    private void OnCast0(InputAction.CallbackContext obj)
    {
        GameEvents.current.SpellCastButtonHold(false, selectedSpell);
        selectedSpell = 0;
        ProcessCastInput(obj);

    }

    private void OnCast1(InputAction.CallbackContext obj)
    {
        GameEvents.current.SpellCastButtonHold(false, selectedSpell);
        selectedSpell = 1;
        ProcessCastInput(obj);
    }
    
    private void OnCast2(InputAction.CallbackContext obj)
    {
        GameEvents.current.SpellCastButtonHold(false, selectedSpell);
        selectedSpell = 2;
        ProcessCastInput(obj);
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

    private void EscapePressed(InputAction.CallbackContext obj)
    {
        if (obj.performed)
            GameEvents.current.EscapePressed();
    }

}
