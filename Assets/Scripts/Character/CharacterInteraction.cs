using UnityEngine;
using UnityEngine.InputSystem;
using Interfaces;

public class CharacterInteraction : MonoBehaviour, IDestroyable
{
    public DefaultControls control { get; private set; }

    [Header("Parameters")]
    [SerializeField] private SpellSetSO[] spellsSets;

    [Header("References")]
    private CharacterAim aim;
    private CharacterLimitations limitations; 
    private ManaPool mana;
    private IHealthSystem health;
    Camera camera;


    public SpellContainer[] containedSpells { get; private set; }
    private int selectedSlotIdx;

    private void Awake()
    {
        aim = GetComponentInChildren<CharacterAim>();
        limitations = GetComponentInChildren<CharacterLimitations>();
        mana = GetComponent<ManaPool>();
        health = GetComponent<IHealthSystem>();
    }

    private void Start()
    {
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

        control.gameplay.RestoreMana.performed += RestoreMana;
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

        // control.gameplay.RestartLevel.performed -= OnRestart;
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

    public void DestroyObject()
    {
        limitations.DeactivatePlayer();
    }

    public void ActivatePlayer()
    {
        limitations.ActivatePlayer();
    }


    public void UpgradeSpell(int spellSlot, int upgradeIdx)
    {
        containedSpells[spellSlot].spellSet.Traverse(upgradeIdx);
    }

    private void RestoreMana(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            mana.Consume(-mana.MaxMp / 2);
            health.ConsumeHp(1, Vector2.zero, true);
        }
    }
}
