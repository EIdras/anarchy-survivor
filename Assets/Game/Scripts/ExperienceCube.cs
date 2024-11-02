using System;
using UnityEngine;

public class ExperienceCube : MonoBehaviour
{
    private int experienceValue; // Valeur d'expérience du cube
    
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
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        if (distanceToPlayer <= PlayerManager.Instance.collectionRadius)
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