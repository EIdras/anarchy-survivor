using UnityEngine;

public abstract class PowerupEffect : ScriptableObject
{
    public abstract void Activate(GameObject player, int level);
}