using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private float health;
    private float speed;
    private float damageToPlayer = 10f;

    private List<Color> originalColor_head;
    private List<Color> originalColor_body;
    public Color damageColor = Color.red;
    public float damageColorDuration = 0.2f;
    public GameObject experienceCubePrefab;
    public float dropChance = 0.1f; // 10% de chance de drop par défaut
    public int expAmount;

    GameObject body;
    GameObject head;
    SkinnedMeshRenderer bodyRenderer;
    SkinnedMeshRenderer headRenderer;

    public void Initialize(int health, float speed, float damage, float dropChance, int expAmount, Transform player)
    {
        this.health = health;
        this.speed = speed;
        this.player = player;
        this.dropChance = dropChance;
        this.expAmount = expAmount;
        this.damageToPlayer = damage;

        InitializeColors();
    }

    private void InitializeColors()
    {
        body = gameObject.transform.GetChild(0).gameObject;
        head = gameObject.transform.GetChild(1).gameObject;
        bodyRenderer = body.GetComponent<SkinnedMeshRenderer>();
        headRenderer = head.GetComponent<SkinnedMeshRenderer>();

        originalColor_body = new List<Color>();
        originalColor_head = new List<Color>();

        // Ajout de couleurs à la liste
        foreach (Material mat in bodyRenderer.materials)
        {
            originalColor_body.Add(mat.color);
        }
        foreach (Material mat in headRenderer.materials)
        {
            originalColor_head.Add(mat.color);
        }
    }

    public void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > 0.1f)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                // Regarde le joueur
                transform.LookAt(player);
            }
        }
    }

    // Méthode pour réduire la santé et gérer la destruction de l'ennemi
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
           HandleDeath();
        }
        else
        {
            // Animation de dégat   
            StartCoroutine(ShowDamageEffect());
        }
    }

    private void HandleDeath()
    {
        gameObject.SetActive(false); // Ajoute l'ennemi au pool pour réutilisation

        // Calcul de la probabilité d'apparition d'un cube rare
        float randomChance = UnityEngine.Random.value;
        if (randomChance <= dropChance)
        {
            ExperienceCubeSpawner.Instance.SpawnRareExperienceCube(transform.position, expAmount); // Valeur d'XP du cube rare
        }
    }
    
    private System.Collections.IEnumerator ShowDamageEffect()
    {
        bodyRenderer.material.color = damageColor;
        headRenderer.material.color = damageColor;

        //change la couleur de tous des materiaux des SkinnedMeshRenderer
        foreach (Material mat in bodyRenderer.materials)
        {
            mat.color = damageColor;
        }
        foreach (Material mat in headRenderer.materials)
        {
            mat.color = damageColor;
        }

        yield return new WaitForSeconds(damageColorDuration);

        // Restaurer la couleur originale
        for (int i = 0; i < bodyRenderer.materials.Length; i++)
        {
            bodyRenderer.materials[i].color = originalColor_body[i];
        }
        for (int i = 0; i < headRenderer.materials.Length; i++)
        {
            headRenderer.materials[i].color = originalColor_head[i];
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assurez-vous que le joueur a le tag "Player"
        {
            PlayerManager.Instance.TakeDamage(damageToPlayer);
            Debug.Log($"Enemy hit the player, dealt {damageToPlayer} damage.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Si l'ennemi reste en collision avec le joueur, il continue à lui infliger des dégâts
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.TakeDamage(damageToPlayer * Time.deltaTime);
        }
    }


    // Gizmos pour visualiser la portée de spawn des ennemis
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}