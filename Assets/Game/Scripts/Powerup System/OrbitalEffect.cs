using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/OrbitalEffect")]
public class OrbitalEffect : PowerupEffect
{
    public GameObject bladePrefab;
    public int baseBladeCount = 3;
    public float rotationSpeed = 100f;

    public override void Activate(GameObject player, int level)
    {
        var orbital = player.GetComponent<PlayerOrbital>();
        if (orbital == null)
        {
            orbital = player.AddComponent<PlayerOrbital>();
        }

        int currentBladeCount = baseBladeCount + (level - 1); // Calcule le nombre de lames en fonction du niveau
        orbital.Activate(bladePrefab, currentBladeCount, rotationSpeed);
    }
    
}