using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;        // Dégâts par projectile
    public float fireRate = 1f;       // Cadence de tir en tirs par seconde
    public float projectileSpeed = 10f; // Vitesse des projectiles
    public GameObject projectilePrefab; // Prefab du projectile
    public Transform firePoint;       // Point de départ des projectiles

    private bool isFiring = false;    // Indique si l'arme doit tirer en continu

    private void Start()
    {
        StartFiring(); // Commence à tirer en continu dès le début
    }

    private void OnEnable()
    {
        StartFiring(); // Recommence à tirer quand l'arme est activée
    }

    private void OnDisable()
    {
        StopFiring(); // Arrête de tirer quand l'arme est désactivée
    }

    private void StartFiring()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(FireContinuously());
        }
    }

    private void StopFiring()
    {
        if (isFiring)
        {
            isFiring = false;
            StopCoroutine(FireContinuously());
        }
    }

    private IEnumerator FireContinuously()
    {
        while (isFiring)
        {
            Fire();
            yield return new WaitForSeconds(1f / fireRate); // Attendre en fonction de la cadence de tir
        }
    }

    private void Fire()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Instancier le projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.SetDamage(damage);
                projScript.speed = projectileSpeed;
            }
        }
    }
}