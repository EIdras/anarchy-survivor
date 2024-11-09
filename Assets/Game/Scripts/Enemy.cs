using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private float health;
    private float speed;

    public void Initialize(int health, float speed, Transform player)
    {
        this.health = health;
        this.speed = speed;
        this.player = player;
    }

    public void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            // Regarde le joueur
            transform.LookAt(player);
        }
    }

    // Méthode pour réduire la santé et gérer la destruction de l'ennemi
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false); // Ajoute l'ennemi au pool pour réutilisation
        }
    }
    
    // Gizmos pour visualiser la portée de spawn des ennemis
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}