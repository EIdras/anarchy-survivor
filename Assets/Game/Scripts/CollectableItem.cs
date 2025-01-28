using System;
using UnityEngine;

public abstract class CollectableItem : MonoBehaviour
{
    protected Transform playerTransform;
    protected bool isAttracting = false;
    protected bool isReadyToCheckDistance = false;
    private float activationDelay = 0.5f; // Temps avant que l'objet puisse �tre collect�

    [SerializeField] private float attractionSpeed = 10f;
    [SerializeField] private float minDistanceToCollect = 0.5f; // Distance minimale pour collecter l'objet

    private void OnEnable()
    {
        isAttracting = false;
        isReadyToCheckDistance = false; // Emp�che la d�tection imm�diate
        Invoke(nameof(EnableDistanceCheck), activationDelay); // Active la v�rification apr�s un d�lai
    }

    private void EnableDistanceCheck()
    {
        isReadyToCheckDistance = true;
    }

    protected virtual void Start()
    {
        playerTransform = PlayerController.Instance.transform;
    }

    private void Update()
    {
        if (isAttracting)
        {
            MoveTowardsPlayer();
        }
        else if (isReadyToCheckDistance)
        {
            CheckPlayerDistance();
        }
    }

    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= PlayerManager.Instance.collectionRadius)
        {
            StartAttraction();
        }
    }

    private void StartAttraction()
    {
        isAttracting = true;
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player transform is null.");
            return;
        }

        // D�place progressivement l'objet vers le joueur
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        float step = attractionSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

        // V�rifie si l'objet est suffisamment proche pour �tre collect�
        if (Vector3.Distance(transform.position, playerTransform.position) <= minDistanceToCollect)
        {
            Collect();
        }
    }

    protected abstract void Collect(); // M�thode abstraite � impl�menter dans les classes d�riv�es
}
