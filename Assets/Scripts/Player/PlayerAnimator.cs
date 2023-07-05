using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Animator animator;
    [SerializeField] private NetworkAnimator netWorkAnimator;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        netWorkAnimator = GetComponent<NetworkAnimator>();

        player.OnPlayerHit += playerTriggerHit;
        player.OnPlayerJump += playerTriggerJump;
        player.OnPlayerAttack1 += playerTriggerAttack1;
    }

    private void Update()
    {
        if(!IsOwner) return;

        //animator.SetFloat("Speed", player.getVelocity());
        animator.SetBool("isStunned", !player.canMove);

        animator.SetBool("isGrounded", true);//player.IsGrounded()

    }

    public void playerTriggerHit(object sender, EventArgs args)
    {
        netWorkAnimator.SetTrigger("Hit");
    }

    public void playerTriggerJump(object sender, EventArgs args)
    {
        netWorkAnimator.SetTrigger("Jump");
    }

    public void playerTriggerAttack1(object sender, EventArgs args)
    {
        netWorkAnimator.SetTrigger("Attack1");
    }


}
