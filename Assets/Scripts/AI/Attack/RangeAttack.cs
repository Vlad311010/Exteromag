using Interfaces;
using Structs;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttack : MonoBehaviour, IAttackAI
{
    public SpellScriptableObject spell;
    
    public float attackRange;
    public Vector2 attackBreak;
    public AICore core { get; set; }
    

    [SerializeField] Transform target;
    [SerializeField] GameObject projectile;


    private bool targetIsVisible;
    private Coroutine attackCoroutine = null;


    public void AIUpdate()
    {
        targetIsVisible = AIGeneral.TargetIsVisible(transform.position, target, attackRange, core.playerObstaclesLayerMask);
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }
        else if (!targetIsVisible && attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }


    }

    IEnumerator AttackCoroutine()
    {
        float timer = Random.Range(attackBreak.x, attackBreak.y);
        yield return new WaitForSeconds(timer);
        Attack();
        attackCoroutine = null;
    }

    private void Attack()
    {
        Vector2 castDirection = (target.transform.position - transform.position).normalized;
        AimSnapshot aimSnapshot = new AimSnapshot(target.transform.position, castDirection);
        SpellCasting.Cast(transform, spell, aimSnapshot);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = targetIsVisible ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
