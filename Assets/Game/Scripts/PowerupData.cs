using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerup", menuName = "Powerups/Powerup")]
public class PowerupData : ScriptableObject
{
    public string powerupName;
    public Sprite icon;
    public string description;
    public int level = 1;
    public PowerupRarity rarity;
}

public enum PowerupRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
