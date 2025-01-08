using System;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoSingleton<AnimationManager>
{
    
    [SerializeField] private Animator animator;
    
    
    private void OnEnable()
    {
        PlayerMovement.Jumping += OnJumping;
        PlayerMovement.Grounded += OnGrounded;
        PlayerMovement.WallHanging += OnWallHanging;
        PlayerMovement.Crouching += OnCrouching;
        PlayerMovement.Running += OnRunning;
        PlayerMovement.LadderClimbing += OnLadderClimbing;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            SwordAttack();
        }
    }

    private void OnJumping()
    {
        animator.SetBool("Running", false);
        animator.SetBool("Grounded", false);
        animator.SetBool("Jumping", true);
    }

    private void OnGrounded(bool isGrounded)
    {
        animator.SetBool("Jumping", !isGrounded);
        animator.SetBool("Grounded", isGrounded);
    }

    private void OnWallHanging(bool isWallHanged)
    {
        animator.SetBool("Running", false);
        animator.SetBool("WallHang", isWallHanged);
    }
    
    private void OnCrouching(bool isCrouching)
    {
        animator.SetBool("Crouching", isCrouching);
    }
    
    private void OnRunning(bool isRunning)
    {
        animator.SetBool("Running", isRunning);
    }
    
    private void OnLadderClimbing(bool isOnLadder, bool isClmbing)
    {
        animator.SetBool("Climbing", isClmbing);
        animator.SetBool("OnLadder", isOnLadder);
        //take only the first frame of the animation
    }
    
    private void SwordAttack()
    {
        animator.SetTrigger("SwordAttack");
    }
        
    private void OnDisable()
    {
        PlayerMovement.Jumping -= OnJumping;
        PlayerMovement.Grounded -= OnGrounded;
        PlayerMovement.WallHanging -= OnWallHanging;
        PlayerMovement.Crouching -= OnCrouching;
        PlayerMovement.Running -= OnRunning;
        PlayerMovement.LadderClimbing -= OnLadderClimbing;
    }
}
