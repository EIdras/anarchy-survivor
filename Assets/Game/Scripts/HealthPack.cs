using UnityEngine;

public class HealthPack : CollectableItem
{
    [SerializeField] private float healAmount = 20f;

    protected override void Collect()
    {
        PlayerManager.Instance.Heal(healAmount);
        gameObject.SetActive(false);
    }
}
