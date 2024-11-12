using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/ShieldEffect")]
public class ShieldEffect : PowerupEffect
{
    public int initialLayers = 0;           // Couches initiales pour le niveau 1
    public float regenerationTime = 5f;     // Temps de régénération par couche

    public override void Activate(GameObject player, int level)
    {
        var shield = player.GetComponent<PlayerShield>();
        if (shield == null)
        {
            shield = player.AddComponent<PlayerShield>();
            shield.shieldPrefab = Resources.Load<GameObject>("ShieldPrefab");
        }
        
        shield.regenerationTime = regenerationTime;
        shield.InitializeShield(level); // Niveau affecte le nombre de couches
    }
}