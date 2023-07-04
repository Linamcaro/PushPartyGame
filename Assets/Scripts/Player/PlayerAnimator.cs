using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private PlayerMovement player;
    private Animator animator;
    private NetworkAnimator networkAnimator;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();

        player.OnPlayerHit += playerTriggerHit;
        player.OnPlayerJump += playerTriggerJump;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        animator.SetFloat("Speed", player.getVelocity());
        animator.SetBool("isStunned", !player.canMove);

        animator.SetBool("isGrounded", player.IsGrounded());

    }

    public void playerTriggerHit(object sender, EventArgs args)
    {
        animator.SetTrigger("Hit");
    }

    public void playerTriggerJump(object sender, EventArgs args)
    {
        animator.SetTrigger("Jump");
    }

}
