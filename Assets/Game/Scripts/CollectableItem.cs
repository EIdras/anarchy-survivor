using System;
using UnityEngine;

public abstract class CollectableItem : MonoBehaviour
{
    protected Transform playerTransform;
    protected bool isAttracting = false;
    protected bool isReadyToCheckDistance = false;
    private float activationDelay = 0.5f; // Temps avant que l'objet puisse être collecté

    [SerializeField] private float attractionSpeed = 10f;
    [SerializeField] private float minDistanceToCollect = 0.5f; // Distance minimale pour collecter l'objet

    private void OnEnable()
    {
        isAttracting = false;
        isReadyToCheckDistance = false; // Empêche la détection immédiate
        Invoke(nameof(EnableDistanceCheck), activationDelay); // Active la vérification après un délai
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

        // Déplace progressivement l'objet vers le joueur
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        float step = attractionSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

        // Vérifie si l'objet est suffisamment proche pour être collecté
        if (Vector3.Distance(transform.position, playerTransform.position) <= minDistanceToCollect)
        {
            Collect();
        }
    }

    protected abstract void Collect(); // Méthode abstraite à implémenter dans les classes dérivées
}
