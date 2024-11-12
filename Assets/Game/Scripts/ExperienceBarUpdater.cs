using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBarUpdater : MonoBehaviour
{
    private PlayerManager playerManager;
    private PowerupPanelManager powerupPanelManager;

    [SerializeField] private Image experienceBar;
    [SerializeField] private int barHeight = 10;

    private void Awake()
    {
        playerManager = PlayerManager.Instance;
        powerupPanelManager = PowerupPanelManager.Instance;
        experienceBar.rectTransform.sizeDelta = new Vector2(0, barHeight);
    }

    // S'abonner aux événements de PlayerManager lorsque le joueur récupère de l'expérience
    private void OnEnable()
    {
        playerManager.OnExperienceChanged += UpdateExperienceBar;
        playerManager.OnLevelUp += ResetBarWhenLevelUp;
    }

    // Se désabonner des événements de PlayerManager lorsque le script est désactivé
    private void OnDisable()
    {
        playerManager.OnExperienceChanged -= UpdateExperienceBar;
    }

    // Mettre à jour la barre d'expérience
    private void UpdateExperienceBar(int experience, int experienceRequiredToLevelUp)
    {
        // XP * largeur écran / XP nécessaire pour le prochain niveau
        experienceBar.rectTransform.sizeDelta = new Vector2(experience * Screen.width / experienceRequiredToLevelUp, barHeight);
    }
    
    private void ResetBarWhenLevelUp(int level)
    {
        ResetExperienceBar();
    }

    private void ResetExperienceBar()
    {
        experienceBar.rectTransform.sizeDelta = new Vector2(0, barHeight);
    }
}
