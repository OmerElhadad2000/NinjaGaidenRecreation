using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoSingleton<AnimationManager>
{
    
    [SerializeField] private Animator animator;
    
    
    private void OnEnable()
    {
        PlayerMovement.Jumping += OnJumping;
        PlayerMovement.Grounded += OnGrounded;
        PlayerMovement.WallHanged += OnWallHanged;
    }
    
    
    private void OnJumping()
    {
        animator.SetBool("Grounded", false);
        animator.SetBool("Jumping", true);
    }

    private void OnGrounded(bool isGrounded)
    {
        animator.SetBool("Jumping", !isGrounded);
        animator.SetBool("Grounded", isGrounded);
    }

    private void OnWallHanged(bool isWallHanged)
    {
        animator.SetBool("WallHang", isWallHanged);
    }
    
    
    
    
    private void OnDisable()
    {
        PlayerMovement.Jumping -= OnJumping;
        PlayerMovement.Grounded -= OnGrounded;
    }
}
