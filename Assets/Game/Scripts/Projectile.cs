using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10.0f;
    public float lifetime = 2.0f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Détruire la balle après sa durée de vie
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Déplace le projectile vers l'avant
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Vérifie si le projectile touche un ennemi
        {
            // Logique d'impact : ici tu pourrais appeler une méthode sur l'ennemi, infliger des dégâts, etc.
            Debug.Log("Hit an enemy!");
            Destroy(gameObject); // Détruire la balle après l'impact
        }
    }
}