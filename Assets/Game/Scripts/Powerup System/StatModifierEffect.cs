using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/StatModifierEffect")]
public class StatModifierEffect : PowerupEffect
{
    public enum StatType { Speed, CollectionRadius, FireRate, PassiveRegen }
    public StatType stat;
    public float modifier;

    public override void Activate(GameObject player, int level)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();

        switch (stat)
        {
            case StatType.Speed:
                playerManager.moveSpeed += modifier * level;
                break;
            case StatType.CollectionRadius:
                playerManager.collectionRadius += modifier * level;
                break;
            case StatType.FireRate:
                playerManager.weapon.fireRate += modifier * level;
                break;
            case StatType.PassiveRegen:
                playerManager.passiveRegen += modifier * level;
                break;
        }
    }
    
}