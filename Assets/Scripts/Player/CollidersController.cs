using UnityEngine;

public class CollidersController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D crouchingCollider;
    [SerializeField] private CircleCollider2D jumpingCollider;
    // [SerializeField] private BoxCollider2D swordAttackCollider;
    // [SerializeField] private CircleCollider2D jumpSwordAttackCollider;
    private bool _isGrounded;
    
    private void OnEnable()
    {
        PlayerMovement.Jumping += OnJumping;
        PlayerMovement.Crouching += OnCrouching;
        PlayerMovement.WallHanging += OnWallHanging;
        PlayerMovement.Running += OnRunning;
        PlayerMovement.Grounded += OnGrounded;
    }
    
    private void DisableAllColliders()
    {
        standingCollider.enabled = false;
        crouchingCollider.enabled = false;
        jumpingCollider.enabled = false;
        // swordAttackCollider.enabled = false;
        // jumpSwordAttackCollider.enabled = false;
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
    }
    
    private void OnRunning(bool isRunning)
    {
        switch (isRunning)
        {
            case true when _isGrounded:
                DisableAllColliders();
                standingCollider.enabled = true;
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
    }
    
    private void OnDisable()
    {
        PlayerMovement.Jumping -= OnJumping;
        PlayerMovement.Crouching -= OnCrouching;
        PlayerMovement.WallHanging -= OnWallHanging;
        PlayerMovement.Running -= OnRunning;
    }
}
