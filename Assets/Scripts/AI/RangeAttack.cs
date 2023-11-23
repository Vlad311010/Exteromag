using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour, IAttackAI
{
    public float attackRange;
    public Vector2 attackBreak;
    public LayerMask layerMask;

    [SerializeField] Transform target;
    [SerializeField] GameObject projectile;

    private Coroutine attackCoroutine = null;

    public void AIUpdate()
    {
        bool targetIsVisible = AIGeneral.IsVisible(transform.position, target, attackRange, layerMask);
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackCoroutine());
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

    }

    void OnDrawGizmosSelected()
    {
        bool targetIsVisible = AIGeneral.IsVisible(transform.position, target, attackRange, layerMask);
        Gizmos.color = targetIsVisible ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
