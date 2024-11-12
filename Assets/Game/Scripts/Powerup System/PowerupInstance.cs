using UnityEngine;

public class PowerupInstance
{
    public PowerupData data;
    public int level;
    private PowerupEffect effect;

    public PowerupInstance(PowerupData data, int level = 1)
    {
        this.data = data;
        this.level = level;
        this.effect = data.effect;
        Debug.Log("New power-up instance created : " + data.powerupName + " level " + level);
    }

    public void LevelUp()
    {
        level++;
    }

    public void ActivateEffect(GameObject player)
    {
        Debug.Log("Activating power-up effect : " + data.powerupName + " level " + level);
        if (effect != null)
        {
            effect.Activate(player, level);
        }
    }
}