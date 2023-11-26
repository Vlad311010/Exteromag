using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Protect : MonoBehaviour, IMoveAI
{
    public Transform protectionTargert;

    [SerializeField] Transform targert;
    [SerializeField] float innerRadius;
    [SerializeField] float outerRadius;

    private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    public void AIUpdate()
    {
    }

    public void AIReset()
    {
    }
}
