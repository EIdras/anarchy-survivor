using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;    // Vitesse du projectile
    public float lifetime = 5f;  // Durée de vie du projectile en secondes
    private float damage;        // Dégâts infligés par le projectile

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime); // Détruire le projectile après sa durée de vie
    }

    private void Update()
    {
        // Déplace le projectile vers l'avant
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le projectile touche un ennemi
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // Détruire le projectile après l'impact
        }
    }
}