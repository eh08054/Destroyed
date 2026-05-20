using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public void PlayPaticle()
    {
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();

        if (particleSystem)
        {
            particleSystem.Play();
        }
    }
}
