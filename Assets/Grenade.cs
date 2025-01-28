using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Param�tres de la grenade")]
    public float explosionDelay = 3f; 
    public float explosionRadius = 50f; 
    public float explosionDamage = 5f; 

    [Header("Effets visuels et sonores")]
    public GameObject explosionEffect;

    public SoundManager soundManager;

    private bool hasExploded = false;

    private void Start()
    {
        soundManager = SoundManager.Instance;
        // Lancer le compte � rebours de l'explosion
        Invoke("Explode", explosionDelay);
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Effet visuel de l'explosion
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        soundManager.PlayGrenadeExplosionSound();

        // D�tecter tous les objets dans la zone d'explosion
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Appliquer les d�g�ts
                enemy.TakeDamage(explosionDamage);
            }
        }

        // D�truire la grenade apr�s l'explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Dessiner le rayon d'explosion pour visualisation dans l'�diteur
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}