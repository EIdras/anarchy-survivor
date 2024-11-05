using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMouvement : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator animator;
    public Transform target;


    void Start()
    {
        
    }

    void Update()
    {
        agent.SetDestination(target.position);

        float actualSpeed = agent.velocity.magnitude / agent.speed;
    }

}
