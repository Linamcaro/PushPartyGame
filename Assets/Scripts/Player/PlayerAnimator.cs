using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const string IS_WALKING = "IsWalking";


    [SerializeField] private PlayerMovement player;
    private NetworkAnimator animator;


    private void Awake()
    {
        animator = GetComponent<NetworkAnimator>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        //animation for walking
       // animator.SetBool(IS_WALKING, player.IsWalking());

        //animation for slide
        if (player.isSliding)
        {
            animator.SetTrigger("Slide");
        }
    }

}
