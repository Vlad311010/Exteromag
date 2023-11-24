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



    private SpellSlotWrapper[] spells;

    private void Start()
    {
        aim = GetComponentInChildren<CharacterAim>();
        mana = GetComponent<ManaPool>();
        // cast = GetComponent<SpellCasting>();
        camera = Camera.main;

        spells = new SpellSlotWrapper[spellsSO.Length];
        for (int i = 0; i < spellsSO.Length; i++)
        {
            spells[i] = new SpellSlotWrapper(spellsSO[i]);
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
        if (!spells[0].IsEmpty() && spells[0].holdDown && !spells[0].isInCooldown)
        {
            if (mana.HaveEnaughtMp(spells[0].spell.castCost))
            {
                mana.Consume(spells[0].spell.castCost);
                StartCoroutine(SpellCoolDown(0));
                SpellCasting.Cast(transform, spells[0].spell, aim.TakeSnapshot());
            }
        }
    }

    private void OnAction(InputAction.CallbackContext obj)
    {
        if (obj.started)
            spells[0].holdDown = true;
        else if (obj.canceled)
            spells[0].holdDown = false;

    }

    private void OnActionAlt(InputAction.CallbackContext obj)
    {

    }

    private IEnumerator SpellCoolDown(int slotIdx)
    {
        spells[slotIdx].isInCooldown = true;
        yield return new WaitForSeconds(spells[slotIdx].spell.cooldown);
        spells[slotIdx].isInCooldown = false;
    }


    private void OnRestart(InputAction.CallbackContext obj)
    {
        GameEvents.current.SceneLoad(SceneManager.GetActiveScene().buildIndex);
    }

    public void DestroyObject()
    {
        // 
        // throw new System.NotImplementedException();
    }
}
