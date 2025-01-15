using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/OrbitalEffect")]
public class OrbitalEffect : PowerupEffect
{
    public GameObject bladePrefab;
    public float rotationSpeed = 100f;

    public override void Activate(GameObject player, int level)
    {
        var orbital = player.GetComponent<PlayerOrbital>();
        if (orbital == null)
        {
            orbital = player.AddComponent<PlayerOrbital>();
        }
        
        orbital.Activate(bladePrefab, level, rotationSpeed);
    }
    
}