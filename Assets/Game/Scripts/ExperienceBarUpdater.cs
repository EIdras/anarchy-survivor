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

    // S'abonner aux �v�nements de PlayerManager lorsque le joueur r�cup�re de l'exp�rience
    private void OnEnable()
    {
        playerManager.OnExperienceChanged += UpdateExperienceBar;
        powerupPanelManager.OnSelectPowerup += OnSelectPowerup;
    }

    // Se d�sabonner des �v�nements de PlayerManager lorsque le script est d�sactiv�
    private void OnDisable()
    {
        playerManager.OnExperienceChanged -= UpdateExperienceBar;
    }

    // Mettre � jour la barre d'exp�rience
    private void UpdateExperienceBar(int experience, int experienceRequiredToLevelUp)
    {
        // XP * largeur �cran / XP n�cessaire pour le prochain niveau
        experienceBar.rectTransform.sizeDelta = new Vector2(experience * Screen.width / experienceRequiredToLevelUp, barHeight);
    }

    private void ResetExperienceBar()
    {
        experienceBar.rectTransform.sizeDelta = new Vector2(0, barHeight);
    }

    private void OnSelectPowerup(PowerupCard powerupCard)
    {
        ResetExperienceBar();
    }
}
