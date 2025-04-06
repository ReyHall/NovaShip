using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public ParticleSystem particlePrefab;
    public Transform spawnPoint; // Local onde as partículas serão instanciadas
    public float duration = 1.5f; // Duração do efeito antes de ser destruído

    public void PlayEffect()
    {
        if (particlePrefab != null && spawnPoint != null)
        {
            ParticleSystem effect = Instantiate(particlePrefab, spawnPoint.position, Quaternion.identity);
            
            var main = effect.main;
            main.maxParticles = Mathf.Clamp(main.maxParticles / 2, 5, 50);

            Destroy(effect.gameObject, duration);
        }
    }
}
