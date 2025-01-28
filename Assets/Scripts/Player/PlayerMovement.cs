using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    //Components
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float jumpingPower;
    [SerializeField] private LayerMask slideLayer;
    [SerializeField] private GameObject player;
    
    [SerializeField] private float knockbackForce = 3f; // Force applied when hit
    [SerializeField] private float knockbackDuration = 0.5f; // Duration of knockback

    //Movement Variables
    private float _horizontal;
    private float _vertical;
    private bool _isGrounded;
    private bool _isCrouching;
    private bool _isWallJumping;
    private float _wallJumpingDirection;
    private bool _isFacingRight = true;
    private bool _isWallSliding;
    private float _wallJumpingCounter;

    //Movement Constants
    private const float WallJumpingTime = 0.2f;
    private const float WallJumpingDuration = 0.2f;
    private readonly Vector2 _wallJumpingPower = new(1.1f, 6f);
    private const float Speed = 5f;


    public static event Action Jumping;
    public static event Action<bool> Grounded;
    public static event Action<bool> WallHanging;
    public static event Action<bool> Crouching;
    public static event Action<bool> Running;
    
    public static event Action StartInvincibility;

    public static event Action<bool> FacingRight;
    public static event Action JumpKeyPressed; 
    
    //Player Movement Logic

    private void OnEnable()
    {
        PlayerBehavior.PlayerHit += OnPlayerHit;
        WallJump.WallJumpingEnabled += OnWallJumpingEnabled;
    }
    
    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            JumpKeyPressed?.Invoke();
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        }

        if (rb.linearVelocity.y > 0f)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Jumping?.Invoke();
        }

        if (IsGrounded() && _vertical < 0f)
        {
            _isCrouching = true;
        }
        
        PreformWallJump();
        FacingRight?.Invoke(_isFacingRight);
        WallHanging?.Invoke(_isWallSliding);
        Crouching?.Invoke(_isCrouching);
        Running?.Invoke(Mathf.Abs(_horizontal) > 0f);
        
        if (!_isWallJumping)
        {
            
            Flip();
        }
    }

    private void OnWallJumpingEnabled()
    {
        _isWallSliding = true;
    }
    private void FixedUpdate()
    {
        if (!_isWallJumping)
        {
            _isCrouching = false;
            rb.linearVelocity = new Vector2(_horizontal * Speed, rb.linearVelocity.y);
        }
        if (_isCrouching)
        {
            rb.linearVelocity = new Vector2(0f, 0f);
        }
    }

    private bool IsGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        Grounded?.Invoke(_isGrounded);
        return _isGrounded;
    }
    

    // ReSharper disable Unity.PerformanceAnalysis
    private void PreformWallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpingDirection = -player.transform.localScale.x;
            _wallJumpingCounter = WallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }

        else
        {
            _wallJumpingCounter -= Time.deltaTime;
        }

        if (!Input.GetButtonDown("Jump") || !(_wallJumpingCounter > 0f)) return;
        JumpKeyPressed?.Invoke();
        _isWallJumping = true;
        _isWallSliding = false;
        rb.simulated = true;
        rb.linearVelocity = new Vector2(_wallJumpingDirection * _wallJumpingPower.x, _wallJumpingPower.y);
        _wallJumpingCounter = 0f;
        

        if (!Mathf.Approximately(player.transform.localScale.x, _wallJumpingDirection))
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = player.transform.localScale;
            localScale.x *= -1f;
            player.transform.localScale = localScale;
        }

        Invoke(nameof(StopWallJumping), WallJumpingDuration);
    }
    
    private void StopWallJumping()
    {
        _isWallJumping = false;
    }

    private void Flip()
    {
        if ((!_isFacingRight || !(_horizontal < 0f)) && (_isFacingRight || !(_horizontal > 0f)) || _isWallSliding) return;
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = player.transform.localScale;
        localScale.x *= -1f;
        player.transform.localScale = localScale;
    }
    
    
    void OnPlayerHit(Vector2 contactPoint)
    {
        // print(contactPoint);
        // if (contactPoint == Vector2.zero)
        // {
        //     contactPoint = rb.position + Vector2.left; // Default direction if contact point is zero
        // }
        //
        // Vector2 knockbackDirection = (rb.position - contactPoint).normalized;
        //
        // if (IsGrounded())
        // {
        //     knockbackDirection.x = Mathf.Max(Mathf.Abs(knockbackDirection.x), 1f) * Mathf.Sign(knockbackDirection.x); 
        // }
        // else
        // {
        //     knockbackDirection.x = Mathf.Max(Mathf.Abs(knockbackDirection.x), 1f) * Mathf.Sign(knockbackDirection.x); // Ensure a small horizontal component
        //     knockbackDirection.y = Mathf.Max(knockbackDirection.y, 1f); // Ensure a small upward component
        // }
        //
        // Vector2 knockbackVelocity = knockbackDirection.normalized * knockbackForce;
        // rb.linearVelocity = knockbackVelocity;
        
        rb.AddForce(contactPoint * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(DisableMovementUntilGrounded());
    }

    private IEnumerator DisableMovementUntilGrounded()
    {
        enabled = false; // Disable player movement
        yield return new WaitUntil(IsGrounded); // Wait until the player is grounded
        enabled = true; // Re-enable player movement
        StartInvincibility?.Invoke();
    }
    
    void OnDisable()
    {
        PlayerBehavior.PlayerHit -= OnPlayerHit;
    }
}