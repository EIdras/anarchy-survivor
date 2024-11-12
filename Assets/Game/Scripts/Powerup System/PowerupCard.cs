using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerupCard : MonoBehaviour
{
    public Image iconImage;          // L'icône du power-up
    public TMP_Text nameText;        // Le nom du power-up
    public TMP_Text descriptionText; // La description du power-up
    public TMP_Text levelText;       // Le niveau actuel du power-up
    public Image rarityBorder;       // Bordure pour indiquer la rareté du power-up

    [HideInInspector]
    public PowerupData powerupData;  // Référence aux données du power-up (définie lors de la configuration)

    // Configuration de la carte avec les données du power-up et le niveau proposé
    public void Setup(PowerupData powerupData, int level)
    {
        this.powerupData = powerupData;  // Assigne le PowerupData transmis à l'instance actuelle

        // Remplit les informations visuelles de la carte avec les données du PowerupData
        iconImage.sprite = powerupData.icon;
        nameText.text = powerupData.powerupName;
        descriptionText.text = powerupData.description;
        levelText.text = "Lvl " + level;

        // Définit la couleur de la bordure en fonction de la rareté
        switch (powerupData.rarity)
        {
            case PowerUpRarity.Common:
                rarityBorder.color = Color.gray;
                break;
            case PowerUpRarity.Rare:
                rarityBorder.color = Color.blue;
                break;
            case PowerUpRarity.Epic:
                rarityBorder.color = Color.magenta;
                break;
            case PowerUpRarity.Legendary:
                rarityBorder.color = Color.yellow;
                break;
        }
    }
}