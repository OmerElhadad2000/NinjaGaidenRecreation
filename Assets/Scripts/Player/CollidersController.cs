using UnityEngine;

public class CollidersController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D crouchingCollider;
    [SerializeField] private CircleCollider2D jumpingCollider;
    [SerializeField] private CircleCollider2D jumpingSwordAttackCollider;

    private bool _isGrounded;
    private bool _isCrouching;
    private bool _isRunning;
    
    private void OnEnable()
    {
        PlayerMovement.Jumping += OnJumping;
        PlayerMovement.Crouching += OnCrouching;
        PlayerMovement.WallHanging += OnWallHanging;
        PlayerMovement.Running += OnRunning;
        PlayerMovement.Grounded += OnGrounded;
        PlayerAttacks.JumpingSwordAttack += OnJumpingSwordAttack;
    }
    
    private void DisableAllColliders()
    {
        standingCollider.enabled = false;
        crouchingCollider.enabled = false;
        jumpingCollider.enabled = false;
        // swordAttackCollider.enabled = false;
        // jumpingSwordAttackCollider.enabled = false;
    }
    
    private void OnJumping()
    {
        DisableAllColliders();
        jumpingCollider.enabled = true;
    }
    
    private void OnCrouching(bool isCrouching)
    {
        if (!isCrouching) return;
        DisableAllColliders();
        crouchingCollider.enabled = true;
    }
    
    private void OnWallHanging(bool isWallHanging)
    {
        if (!isWallHanging) return;
        DisableAllColliders();
        jumpingCollider.enabled = true;
        jumpingSwordAttackCollider.enabled = false;
    }
    
    private void OnRunning(bool isRunning)
    {
        _isRunning = isRunning;
        switch (isRunning)
        {
            case true when _isGrounded:
                DisableAllColliders();
                standingCollider.enabled = true;
                jumpingSwordAttackCollider.enabled = false;
                break;
            case true when !_isGrounded:
                DisableAllColliders();
                jumpingCollider.enabled = true;
                break;
        }
    }
    
    
    private void OnGrounded(bool isGrounded)
    {
        _isGrounded = isGrounded;
        switch (isGrounded)
        {
            case true when _isCrouching:
                DisableAllColliders();
                crouchingCollider.enabled = true;
                jumpingSwordAttackCollider.enabled = false;
                break;
            case true when !_isRunning:
                DisableAllColliders();
                standingCollider.enabled = true;
                break;
        }
    }
    
    private void OnJumpingSwordAttack()
    {
        DisableAllColliders();
        jumpingCollider.enabled = true;     
        jumpingSwordAttackCollider.enabled = true;
    }
    
    private void OnDisable()
    {
        PlayerMovement.Jumping -= OnJumping;
        PlayerMovement.Crouching -= OnCrouching;
        PlayerMovement.WallHanging -= OnWallHanging;
        PlayerMovement.Running -= OnRunning;
        PlayerMovement.Grounded -= OnGrounded;
        PlayerAttacks.JumpingSwordAttack -= OnJumpingSwordAttack;
    }
}
