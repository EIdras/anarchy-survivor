using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class PowerupPanelManager : MonoBehaviour
{
    public static PowerupPanelManager Instance { get; private set; }

    public GameObject powerupCardPrefab;
    public Transform cardContainer;
    public List<PowerupData> availablePowerups;

    private List<GameObject> activeCards = new List<GameObject>();
    private PlayerInput playerInput;

    public event Action<PowerupCard> OnSelectPowerup;

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
        gameObject.SetActive(false);
        playerInput = PlayerController.Instance.GetPlayerInput();
    }

    public void ShowPowerupOptions()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);

        PlayerController.Instance.EnableControl(false);

        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
        activeCards.Clear();

        List<PowerupData> selectedPowerups = GetRandomPowerups(3);
        
        foreach (var powerup in selectedPowerups)
        {
            GameObject card = Instantiate(powerupCardPrefab, cardContainer);
            card.GetComponent<PowerupCard>().Setup(powerup);
            activeCards.Add(card);
        }

        // Active la première carte pour la navigation via l'Event System
        EventSystem.current.SetSelectedGameObject(activeCards[0]);

        // Active l'écoute de l'action "Submit" pour la validation
        playerInput.actions["Submit"].performed += OnSelect;
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        PlayerController.Instance.EnableControl(true);

        // Désactive l'écoute de l'action "Submit"
        playerInput.actions["Submit"].performed -= OnSelect;
    }

    private List<PowerupData> GetRandomPowerups(int count)
    {
        List<PowerupData> selectedPowerups = new List<PowerupData>();
        List<PowerupData> copyOfAvailable = new List<PowerupData>(availablePowerups);

        for (int i = 0; i < count; i++)
        {
            if (copyOfAvailable.Count == 0) break;
            int index = UnityEngine.Random.Range(0, copyOfAvailable.Count);
            selectedPowerups.Add(copyOfAvailable[index]);
            copyOfAvailable.RemoveAt(index);
        }

        return selectedPowerups;
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        // Valide la carte sélectionnée dans l'Event System
        SelectPowerup();
    }

    private void SelectPowerup()
    {
        GameObject selectedCard = EventSystem.current.currentSelectedGameObject;
        if (selectedCard != null)
        {
            PowerupCard card = selectedCard.GetComponent<PowerupCard>();
            if (card != null)
            {
                Debug.Log("Power-up selected: " + card.nameText.text);
                HidePanel();
                OnSelectPowerup?.Invoke(card);
            }
        }
    }
}
