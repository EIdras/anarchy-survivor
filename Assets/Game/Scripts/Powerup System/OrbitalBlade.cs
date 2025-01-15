using UnityEngine;

public class OrbitalBlade : MonoBehaviour
{
    public float damage = 10f; // Dégâts infligés par la lame

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Blade hit something.");
        if (other.CompareTag("Enemy")) // Vérifie que la collision est avec un ennemi
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Inflige des dégâts à l'ennemi
                // TODO: Ajouter des effets visuels ou sonores
                Debug.Log($"Enemy hit by blade. Dealt {damage} damage.");
            }
        }
    }
    
    public void Initialize(PlayerOrbital playerOrbital)
    {
        PowerupManager powerupManager = PowerupManager.Instance;
        powerupManager.GetAllPowerUps();
        damage = 10f;
        // TODO: indexer les dégâts infligés par rapport au niveau du powerup et du multiplicateur de dégâts du joueur
    }
    
}