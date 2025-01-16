using System;
using UnityEngine;

public class ExperienceCube : MonoBehaviour
{
    private int experienceValue; // Valeur d'expérience du cube
    private bool isAttracting = false; // Indique si le cube est en train d'être attiré
    private Transform playerTransform;
    private float attractionSpeed = 10f; // Vitesse d'attraction du cube vers le joueur
    private float minDistanceToCollect = 0.5f; // Distance minimale pour collecter le cube
    private bool isReadyToCheckDistance = false; // Indique si le cube peut vérifier la distance avec le joueur
    private float activationDelay = 0.5f; // Temps avant que le cube commence à vérifier la distance

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
    private void Start()
    {
        playerTransform = PlayerController.Instance.transform;
    }

    public void SetExperienceValue(int value)
    {
        if (value <= 0)
        {
            Debug.LogWarning("Experience value for ExperienceCube is set to zero or negative. Setting to default value of 1.");
            experienceValue = 1;
        }
        else
        {
            experienceValue = value;
        }
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

        // Déplace progressivement le cube vers le joueur
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        float step = attractionSpeed * Time.deltaTime;

        // Met à jour la position du cube
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

        // Vérifie si le cube est suffisamment proche pour être collecté
        if (Vector3.Distance(transform.position, playerTransform.position) <= minDistanceToCollect)
        {
            Collect();
        }
    }

    private void Collect()
    {
        PlayerManager.Instance.AddExperience(experienceValue);
        gameObject.SetActive(false); // TODO : Détruire le cube (plante Unity)
    }
}