using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Jumping = Animator.StringToHash("Jumping");

    [SerializeField]
    private Transform playerGroundPosition;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float moveSpeed;

    private bool _isJumping;
    
    private bool _currentlyJumping;

    private bool _isInAir;

    private bool _isCrouching;

    private float _lastXDirection;

    [SerializeField] private Rigidbody2D playerRb;

    private void Update()
    {
        _isJumping = IsOnGround() && Input.GetKey(KeyCode.UpArrow);
        
        _isCrouching = Input.GetKey(KeyCode.DownArrow) && !_isInAir;
        
        animator.SetBool("Crouching", _isCrouching);
        
    }

    private void FixedUpdate()
    {
        float xInput = Input.GetAxis("Horizontal");
        
        
        
        // Move the player horizontally only if not crouching
        playerRb.linearVelocity = new Vector2(xInput * moveSpeed, playerRb.linearVelocity.y);

        if (_isCrouching)
        {
            Crouch();
        }
        else
        {
            if (!_isJumping && playerRb.linearVelocity.x is > 3f or < -3f)
            {
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Running", false);
            }
            
            if (xInput != 0)
            {
                _lastXDirection = xInput;
            }
        }

        if (_lastXDirection != 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Sign(_lastXDirection) * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }

        // Check if the up key is pressed and apply an impulse force if grounded
        if (_isJumping)
        {
            
            
            animator.SetBool("Ground", false);
            animator.SetBool(Jumping, _isJumping);
            // Reset the Y velocity before jumping

            JumpNoSword();
            // animator.SetBool(Running, false);
            

            // flip the player if moving in the opposite direction
            
        }
        else
        {
            animator.SetBool("Ground", true);
        }
    }

    private void JumpNoSword()
    {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
        playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
    
    private void Crouch()
    {
        playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
    }

    private void Run()
    {
        
    }

    private bool IsOnGround()
    {
        var hit = Physics2D.Raycast(playerGroundPosition.position, Vector2.down, 0.2f, groundLayer);
        _isInAir = hit.collider == null;
        return hit.collider != null;
    }
}