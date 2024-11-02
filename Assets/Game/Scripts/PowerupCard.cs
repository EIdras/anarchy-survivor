using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupCard : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text levelText;
    public Image rarityBorder;
    public Image selectionBorder;

    public void Setup(PowerupData powerupData)
    {
        iconImage.sprite = powerupData.icon;
        nameText.text = powerupData.powerupName;
        descriptionText.text = powerupData.description;
        levelText.text = "Lvl " + powerupData.level;
        
        // Définir la couleur en fonction de la rareté
        switch (powerupData.rarity)
        {
            case PowerupRarity.Common:
                rarityBorder.color = Color.gray;
                break;
            case PowerupRarity.Rare:
                rarityBorder.color = Color.blue;
                break;
            case PowerupRarity.Epic:
                rarityBorder.color = Color.magenta;
                break;
            case PowerupRarity.Legendary:
                rarityBorder.color = Color.yellow;
                break;
        }

        SetSelected(false); // Par défaut, la carte n'est pas sélectionnée
    }

    public void SetSelected(bool isSelected)
    {
        if (selectionBorder != null)
        {
            selectionBorder.gameObject.SetActive(isSelected); // Montre ou cache la bordure de sélection
        }
    }
}