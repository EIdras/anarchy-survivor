using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_With_BehaviorTree : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BehaviorTree tree;
    [SerializeField] private NavMeshAgent agent;

    private PlayerController player;


    void Start()
    {
        player = PlayerController.Instance;

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector("Enemy BT")
                //TODO_Exercice_3: créez une nouvelle séquence "Attack"
                    //TODO_Exercice_3: créez une condition "Is Player Close", qui vérifie si la distance entre le joueur et l'enemy est faible (< 10m)
                    //TODO_Exercice_3: si oui, jouer l'anim "Shoot", à l'aide de SetTrigger, puis attendre à l'aide de WaitTime
                    //TODO_Exercice_3: clore la séquence
                //TODO_Exercice_3: Sinon, créez une Action vide "Idle"
            .Build();

    }
            
    void Update()
    {
        tree.Tick();
    }
}
