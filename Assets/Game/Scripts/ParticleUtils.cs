using UnityEngine;

public static class ParticleUtils
{
    public static void PlayBloodParticles(Vector3 position, int min, int max)
    {
        ParticleSystem hitParticlesPrefab = ParticleManager.Instance.hitParticlesPrefab;
        if (hitParticlesPrefab != null)
        {
            // Instancie les particules à la position spécifiée
            ParticleSystem bloodParticles = Object.Instantiate(hitParticlesPrefab, position, Quaternion.identity);
            var emission = bloodParticles.emission;
            var burst = emission.GetBurst(0);
            burst.count = new ParticleSystem.MinMaxCurve(min, max); // Plus de particules pour la mort
            emission.SetBurst(0, burst);

            bloodParticles.Play();

            // Détruire automatiquement les particules une fois l'animation terminée
            Object.Destroy(bloodParticles.gameObject, bloodParticles.main.duration + bloodParticles.main.startLifetime.constant);
        }
    }
}