using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance { get; private set; }

    private Dictionary<PowerupData, PowerupInstance> playerPowerUps = new Dictionary<PowerupData, PowerupInstance>();
    private GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void AddOrUpgradePowerUp(PowerupData newPowerupData)
    {
        if (playerPowerUps.ContainsKey(newPowerupData))
        {
            // Si le power-up est déjà possédé, on augmente son niveau
            playerPowerUps[newPowerupData].LevelUp();
        }
        else
        {
            // Sinon, on l'ajoute à la liste des power-ups du joueur avec un niveau de 1
            PowerupInstance newInstance = new PowerupInstance(newPowerupData);
            playerPowerUps[newPowerupData] = newInstance;
        }
        playerPowerUps[newPowerupData].ActivateEffect(player);
    }

    public int GetPowerUpLevel(PowerupData powerUpData)
    {
        return playerPowerUps.TryGetValue(powerUpData, out PowerupInstance instance) ? instance.level : 0;
    }

    public List<PowerupInstance> GetAllPowerUps()
    {
        return new List<PowerupInstance>(playerPowerUps.Values);
    }
}