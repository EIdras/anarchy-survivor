using UnityEngine;

public class Enemy_Simple : MonoBehaviour
{
    [SerializeField] private Animator animator;
    //TODO_Exercice_1: ajoutez un champ serialis� NavMeshAgent appel� agent;


    private PlayerController player;

    void Start()
    {
        player = PlayerController.instance;
    }


    void Update()
    {
        //TODO_Exercice_1: donnez une destination au NavMeshAgent agent

        //TODO_Exercice_1: mettez � jour le champ Speed (0 - 1) de l'animator
    }
}
