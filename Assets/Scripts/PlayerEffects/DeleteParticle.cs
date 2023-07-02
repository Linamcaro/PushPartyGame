using UnityEngine;

public class DeleteParticle : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
