using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using UnityEngine;
using UnityEngine.AI;

public class AgentDestination : ActionBase
{
    public Transform Target;
    private NavMeshAgent agent;

    protected override void OnInit()
    {
        agent = Owner.GetComponent<NavMeshAgent>();
    }

    protected override TaskStatus OnUpdate()
    {
        agent.SetDestination(Target.position);
        return TaskStatus.Success;
    }
}