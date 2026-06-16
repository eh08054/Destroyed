using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public void PlayPaticle()
    {
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();

        if (particleSystem && !particleSystem.GetComponent<ProjectileEffect>())
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.GetComponent<ProjectileEffect>().Fire();
        }
    }
}
