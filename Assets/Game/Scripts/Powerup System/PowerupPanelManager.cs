using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PowerupPanelManager : MonoBehaviour
{
    public static PowerupPanelManager Instance { get; private set; }

    public GameObject powerupCardPrefab;
    public Transform cardContainer;
    public List<PowerupData> availablePowerups;
    public AudioSettingsManager audioSettingsManager;

    private List<GameObject> activeCards = new List<GameObject>();
    private PlayerInput playerInput;
    private TimeManager timeManager;

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
        timeManager = TimeManager.Instance;
    }

    public void ShowPowerupOptions()
    {
        gameObject.SetActive(true);

        // Nettoyage des cartes existantes
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
        activeCards.Clear();

        // Sélectionner 3 power-ups aléatoires
        List<PowerupData> selectedPowerups = GetRandomPowerups(3);
        foreach (var powerupData in selectedPowerups)
        {
            GameObject card = Instantiate(powerupCardPrefab, cardContainer);
            // récupère la composante Event Trigger pour ajouter un événement de sélection
            EventTrigger trigger = card.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener((data) => { audioSettingsManager.PlayUISelectSound(); });
            trigger.triggers.Add(entry);
            var cardComponent = card.GetComponent<PowerupCard>();
            int level = PowerupManager.Instance.GetPowerUpLevel(powerupData);
            cardComponent.Setup(powerupData, level + 1); // Proposer un niveau supérieur à ce que le joueur possède
            activeCards.Add(card);
        }

        // Active la première carte pour la navigation via l'Event System
        if (activeCards.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(activeCards[0]);
        }

        // Active l'écoute de l'action "Submit" pour la validation
        playerInput.actions["Submit"].performed += OnSelect;
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        timeManager.ResumeTime();

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
            int index = Random.Range(0, copyOfAvailable.Count);
            selectedPowerups.Add(copyOfAvailable[index]);
            copyOfAvailable.RemoveAt(index);
        }

        return selectedPowerups;
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
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
                PowerupManager.Instance.AddOrUpgradePowerUp(card.powerupData); // Ajouter ou mettre à jour le power-up
                HidePanel();
            }
        }
    }
}
