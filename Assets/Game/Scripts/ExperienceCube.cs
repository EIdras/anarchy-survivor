using UnityEngine;

public class ExperienceCube : CollectableItem
{
    private int experienceValue;

    protected override void Start()
    {
        base.Start();
    }

    public void SetExperienceValue(int value)
    {
        experienceValue = Mathf.Max(value, 1); // Assure que l'XP ne soit pas négative ou nulle
    }

    protected override void Collect()
    {
        PlayerManager.Instance.AddExperience(experienceValue);
        gameObject.SetActive(false);
    }
}
