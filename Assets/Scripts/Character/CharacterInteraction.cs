using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Interfaces;
using Structs;
using System.Collections;

public class CharacterInteraction : MonoBehaviour, IDestroyable
{
    [Header("Inputs")]
    public DefaultControls control;

    [Header("Parameters")]
    [SerializeField] private SpellScriptableObject[] spellsSO;

    [Header("References")]
    private CharacterAim aim;
    private ManaPool mana;
    // private SpellCasting cast;
    Camera camera;



    // private SpellSlotWrapper[] spells;
    private SpellContainer[] spells;
    private int selectedSpell;

    private void Start()
    {
        aim = GetComponentInChildren<CharacterAim>();
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

        control.gameplay.Action.started += OnAction;
        control.gameplay.Action.canceled += OnAction;

        control.gameplay.RestartLevel.performed += OnRestart;
    }


    void OnDisable()
    {
        control.Disable();

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
            spells[selectedSpell].holdDown = true;
        else if (obj.canceled)
            spells[selectedSpell].holdDown = false;

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
        Debug.Log("PLAYER DEATH");
    }
}
