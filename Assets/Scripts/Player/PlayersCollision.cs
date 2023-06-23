using Unity.Netcode;
using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class PlayersCollision : NetworkBehaviour
{
    private Rigidbody rb;
    private bool isStunned = false;
    private float pushForce;
    private Vector3 pushDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void HitPlayer(Vector3 velocityF, float time)
    {
        rb.velocity = velocityF;

        pushForce = velocityF.magnitude;
        pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }

    private IEnumerator Decrease(float value, float duration)
    {
        if (isStunned)
            yield break;

        isStunned = true;

        float delta = value / duration;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
            pushForce = Mathf.Clamp(pushForce - Time.deltaTime * delta, 0f, Mathf.Infinity);
            rb.AddForce(new Vector3(0f, -Physics.gravity.y * rb.mass, 0f));
        }

        isStunned = false;
    }

}
