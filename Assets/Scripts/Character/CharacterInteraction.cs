using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Interfaces;

public class CharacterInteraction : MonoBehaviour, IDestroyable
{
    public DefaultControls control { get; private set; }

    [Header("Parameters")]
    [SerializeField] private SpellSetSO[] spellsSets;

    [Header("References")]
    private CharacterAim aim;
    private CharacterEffects effects; 
    private CharacterLimitations limitations; 
    private ManaPool mana;
    Camera camera;


    public SpellContainer[] containedSpells { get; private set; }
    private int selectedSlotIdx;

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

        control.gameplay.Cast0.started  += ctx => OnCast(ctx, 0);
        control.gameplay.Cast0.canceled += ctx => OnCast(ctx, 0);
        control.gameplay.Cast1.started  += ctx => OnCast(ctx, 1);
        control.gameplay.Cast1.canceled += ctx => OnCast(ctx, 1);
        control.gameplay.Cast2.started  += ctx => OnCast(ctx, 2);
        control.gameplay.Cast2.canceled += ctx => OnCast(ctx, 2);

        control.gameplay.RestartLevel.performed += OnRestart;
    }


    void OnDisable()
    {
        control.Disable();
        
        if (aim != null)
            aim.enabled = false;

        // control.gameplay.Cast0.started  -= ctx => OnCast(ctx, 0);
        // control.gameplay.Cast0.canceled -= OnCast;
        // control.gameplay.Cast1.started  -= OnCast;
        // control.gameplay.Cast1.canceled -= OnCast;
        // control.gameplay.Cast2.started  -= OnCast;
        // control.gameplay.Cast2.canceled -= OnCast;

        control.gameplay.RestartLevel.performed -= OnRestart;
    }

    private void Update()
    {
        if (!containedSpells[selectedSlotIdx].IsEmpty() && containedSpells[selectedSlotIdx].holdDown && !containedSpells[selectedSlotIdx].isInCooldown)
        {
            if (containedSpells[selectedSlotIdx].spellSet.spell.preventConstantCasting)
                containedSpells[selectedSlotIdx].holdDown = false;

            if (mana.HaveEnaughtMp(containedSpells[selectedSlotIdx].castCost))
            {
                mana.Consume(containedSpells[selectedSlotIdx].castCost);
                StartCoroutine(containedSpells[selectedSlotIdx].SpellCooldown());
                SpellCasting.Cast(transform, containedSpells[selectedSlotIdx].spell, aim.TakeSnapshot());
            }
        }
    }

    private void ProcessCastInput(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            GetComponentInChildren<Animator>().SetBool("Casting", true);
            containedSpells[selectedSlotIdx].holdDown = true;
            GameEvents.current.SpellCastButtonHold(true, selectedSlotIdx);

        }
        else if (obj.canceled)
        {
            GetComponentInChildren<Animator>().SetBool("Casting", false);
            containedSpells[selectedSlotIdx].holdDown = false;
            GameEvents.current.SpellCastButtonHold(false, selectedSlotIdx);
        }
    }

    private void OnCast(InputAction.CallbackContext obj, int slotIdx)
    {
        if (containedSpells[selectedSlotIdx].spell.interuptOnSpellChange)
            GameEvents.current.SpellCastButtonHold(false, selectedSlotIdx);

        selectedSlotIdx = slotIdx;
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

    public void UpgradeSpell(int spellSlot, int upgradeIdx)
    {
        containedSpells[spellSlot].spellSet.Traverse(upgradeIdx);
    }

}
