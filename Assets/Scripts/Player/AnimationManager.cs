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
    private static readonly int SpecialJumpAttack = Animator.StringToHash("SpecialJumpAttack");
    private static readonly int ShurikenAttack = Animator.StringToHash("ShurikenAttack");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int SmokeBomb = Animator.StringToHash("SmokeBomb");


    private void OnEnable()
    {
        PlayerMovement.Grounded += OnGrounded;
        PlayerMovement.WallHanging += OnWallHanging;
        PlayerMovement.Crouching += OnCrouching;
        PlayerMovement.Running += OnRunning;
        PlayerAttacks.SwordAttack += SwordAttack;
        PlayerAttacks.JumpingSwordAttack += OnJumpingSwordAttack;
        PlayerAttacks.ShurikenAttack += OnShurikenAttack;
        PlayerBehavior.PlayerHit += OnPlayerHit;
        PlayerAttacks.SmokeBombPreform += OnSmokeBombPreform;
    }
    
    

    private void OnGrounded(bool isGrounded)
    {
        animator.SetBool(Jumping, !isGrounded);
        animator.SetBool(Grounded, isGrounded);
        if (!isGrounded) return;
        animator.SetBool(SpecialJumpAttack, false);
        animator.SetBool(Jumping, false);
    }
    
    private void OnSmokeBombPreform()
    {
        animator.SetTrigger(SmokeBomb);
    }

    private void OnWallHanging(bool isWallHanged)
    {
        animator.SetBool(Running, false);
        if (isWallHanged)
        {
            animator.SetBool(Grounded, false);
            animator.SetBool(Jumping, false);
            animator.SetBool(SpecialJumpAttack, false);
        }
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
    
    private void OnJumpingSwordAttack()
    {
        animator.SetBool(SpecialJumpAttack, true);
        animator.SetBool(Grounded, false);
        animator.SetBool(Jumping, true);
    }
    
    private void OnShurikenAttack()
    {
        animator.SetTrigger(ShurikenAttack);
    }
    
    private void OnPlayerHit(Vector2 hitDirection)
    {
        animator.SetTrigger(Hit);
    }
    
        
    private void OnDisable()
    {
        PlayerMovement.Grounded -= OnGrounded;
        PlayerMovement.WallHanging -= OnWallHanging;
        PlayerMovement.Crouching -= OnCrouching;
        PlayerMovement.Running -= OnRunning;
        PlayerAttacks.SwordAttack -= SwordAttack;
        PlayerAttacks.JumpingSwordAttack -= OnJumpingSwordAttack;
        PlayerAttacks.ShurikenAttack -= OnShurikenAttack;
        PlayerBehavior.PlayerHit -= OnPlayerHit;
        
    }
}
