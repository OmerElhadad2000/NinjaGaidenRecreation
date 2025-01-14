using System;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoSingleton<AnimationManager>
{
    [SerializeField] private Animator animator;

    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int WallHang = Animator.StringToHash("WallHang");
    private static readonly int Crouching = Animator.StringToHash("Crouching");
    private static readonly int Attack = Animator.StringToHash("SwordAttack");
    private void OnEnable()
    {
        PlayerMovement.Jumping += OnJumping;
        PlayerMovement.Grounded += OnGrounded;
        PlayerMovement.WallHanging += OnWallHanging;
        PlayerMovement.Crouching += OnCrouching;
        PlayerMovement.Running += OnRunning;
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
        animator.SetBool(Running, false);
        animator.SetBool(Grounded, false);
        animator.SetBool(Jumping, true);
    }

    private void OnGrounded(bool isGrounded)
    {
        animator.SetBool(Jumping, !isGrounded);
        animator.SetBool(Grounded, isGrounded);
    }

    private void OnWallHanging(bool isWallHanged)
    {
        animator.SetBool(Running, false);
        animator.SetBool(WallHang, isWallHanged);
    }
    
    private void OnCrouching(bool isCrouching)
    {
        animator.SetBool(Crouching, isCrouching);
    }
    
    private void OnRunning(bool isRunning)
    {
        animator.SetBool(Running, isRunning);
    }
    
    private void SwordAttack()
    {
        animator.SetTrigger(Attack);
    }
        
    private void OnDisable()
    {
        PlayerMovement.Jumping -= OnJumping;
        PlayerMovement.Grounded -= OnGrounded;
        PlayerMovement.WallHanging -= OnWallHanging;
        PlayerMovement.Crouching -= OnCrouching;
        PlayerMovement.Running -= OnRunning;
    }
}
