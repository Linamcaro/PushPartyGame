using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownAnimationController : MonoBehaviour
{
    public AnimationClip clip;

    private Animation animationComponent;
    public float delay = 0f;

    private void Start()
    {
        animationComponent = GetComponent<Animation>();
        animationComponent.clip = clip;
        Invoke("PlayClip", delay);
    }

    public void PlayClip()
    {
        animationComponent.Play();
    }
}
