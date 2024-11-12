using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerup", menuName = "Powerups/Powerup Data")]
public class PowerupData : ScriptableObject
{
    public string powerupName;
    public Sprite icon;
    public string description;
    public PowerupEffect effect; // Effet associé au power-up
    public PowerUpRarity rarity;
}

public enum PowerUpRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}