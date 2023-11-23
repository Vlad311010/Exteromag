using Structs;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCasting : MonoBehaviour
{
    [Header("Inputs")]
    DefaultControls control;

    [Header("Parameters")]
    [SerializeField] private SpellScriptableObject[] spellsSO;

    [Header("References")]
    private CharacterAim aim;
    private ManaPool mana;
    Camera camera;



    private SpellSlotWrapper[] spells;


    void OnEnable()
    {
        control = new DefaultControls();
        control.Enable();

        control.gameplay.Action.started += OnAction;
        control.gameplay.Action.canceled += OnAction;
        // control.gameplay.ActionAlt.performed += OnActionAlt;
    }

    void OnDisable()
    {
        control.Disable();
        
        // control.gameplay.Action.performed -= OnAction;
        // control.gameplay.ActionAlt.performed -= OnActionAlt;
    }

    private void Start()
    {
        aim = GetComponentInChildren<CharacterAim>();
        mana = GetComponent<ManaPool>();
        camera = Camera.main;

        spells = new SpellSlotWrapper[spellsSO.Length];
        for(int i = 0; i < spellsSO.Length; i++)
        {
            spells[i] = new SpellSlotWrapper(spellsSO[i]);
        }
    }

    private void Update()
    {
        if (!spells[0].IsEmpty() && spells[0].holdDown && !spells[0].isInCooldown)
        {
            if (mana.HaveEnaughtMp(spells[0].spell.castCost))
            {
                mana.Consume(spells[0].spell.castCost);
                StartCoroutine(SpellCoolDown(0));
                Cast(spells[0].spell);
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


    public void Cast(SpellScriptableObject spell)
    {
        spell = Instantiate(spell);
        AimSnapshot aimSnapshot = aim.TakeSnapshot();

        SpellSpawnData spellSpawnData = new SpellSpawnData(transform.position, aimSnapshot.castPoint, aimSnapshot.castDirection);

        spellSpawnData = AttributesResolver.ResolveSpawnAttributes(spellSpawnData, spell);
        SpawnSpell(spell, spellSpawnData);
    }

    private void SpawnSpell(SpellScriptableObject spell, SpellSpawnData spellSpawnData)
    {
        List<SpellBase> spellObjects = new List<SpellBase>();

        if (spellSpawnData.projectilesPosition.Count != 0)
        {
            for (int i = 0; i < spellSpawnData.projectilesPosition.Count; i++)
            {
                SpellBase spellBase = Instantiate(spell.projectile, spellSpawnData.projectilesPosition[i], spellSpawnData.projectilesRotation[i]).GetComponent<SpellBase>();
                spellObjects.Add(spellBase);
            }
        }
        else
        {
            Vector2 direction = (spellSpawnData.castPoint - spellSpawnData.origin).normalized;
            Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), direction);
            SpellBase spellBase = Instantiate(spell.projectile, spellSpawnData.origin, lookDirection).GetComponent<SpellBase>();
            spellObjects.Add(spellBase);
        }

        foreach (SpellBase spellObject in spellObjects)
        {
            spell = Instantiate(spell);
            spell.casterPosition = spellSpawnData.casterPosition;
            spell.castPoint = spellSpawnData.castPoint;
            spell.owner = transform;

            AttributesResolver.ResolveSpellAttributes(spellObject.gameObject, spell);
            spellObject.Init(spell);

            //add velocity
            if (spellSpawnData.forceMode == Enums.ForceApplyMode.LookDirection)
            {
                /*Vector2 direction = (spellSpawnData.castPoint - (Vector2)spellObject.transform.position).normalized;
                Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), direction);
                spellObject.transform.rotation = Quaternion.Euler(0, 0, lookDirection.eulerAngles.z);*/
                spellObject.GetComponent<ProjectileMovement>().Init(spellObject.transform.up, spell.speed, spell.acceleration, spell.decceleration, spell.useDecceleration, spell.deccelerationStart);
                // spellObject.GetComponent<Rigidbody2D>().AddForce(spellObject.transform.up * spell.speed, ForceMode2D.Impulse);
            }
            else if (spellSpawnData.forceMode == Enums.ForceApplyMode.CastPoint)
            {
                Vector2 direction = (spellSpawnData.castPoint - (Vector2)spellObject.transform.position).normalized;
                spellObject.GetComponent<ProjectileMovement>().Init(direction, spell.speed, spell.acceleration, spell.decceleration, spell.useDecceleration, spell.deccelerationStart);
                // spellObject.GetComponent<Rigidbody2D>().AddForce(direction * spell.speed);
            }

        }
    }

}
