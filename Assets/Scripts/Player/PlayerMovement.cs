using System;
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
    [SerializeField] private LayerMask wallSlideLayerMask;
    [SerializeField] private float jumpingPower;

    //Movement Variables
    private float _horizontal;
    private float _vertical;
    private bool _isGrounded;
    private bool _isCrouching;
    private bool _isClimbing;
    private bool _isLadder;
    private bool _isWallJumping;
    private float _wallJumpingDirection;
    private bool _isFacingRight = true;
    private bool _isWallSliding;
    private float _wallJumpingCounter;

    //Movement Constants
    private const float WallJumpingTime = 0.2f;
    private const float WallJumpingDuration = 0.2f;
    private readonly Vector2 _wallJumpingPower = new(1f, 5f);
    private const float ClimbingSpeed = 3f;
    private const float Speed = 5f;
    

    public static event Action Jumping;
    public static event Action<bool> Grounded;
    public static event Action<bool> WallHanging;
    public static event Action<bool> Crouching;
    public static event Action<bool> Running;
    public static event Action<bool, bool> LadderClimbing;

    //Player Movement Logic
    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Jumping?.Invoke();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (_isLadder && Mathf.Abs(_vertical) > 0f)
        {
            _isClimbing = true;
        }

        if (IsGrounded() && _vertical < 0f)
        {
            _isCrouching = true;
        }

        // else
        // {
        //     _isCrouching = false;
        // }

        WallSlide();
        WallJump();
        
        WallHanging?.Invoke(_isWallSliding);
        Crouching?.Invoke(_isCrouching);
        Running?.Invoke(Mathf.Abs(_horizontal) > 0f);
        LadderClimbing?.Invoke(_isLadder && _isClimbing, _isLadder && Mathf.Abs(_vertical) > 0f);
        if (!_isWallJumping)
        {
            Flip();
        }

        // This Is Adding Wall Transparency When Not Jumping
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer($"Slide Wall"), _isGrounded);
    }

    private void FixedUpdate()
    {
        if (!_isWallJumping)
        {
            _isCrouching = false;
            rb.linearVelocity = new Vector2(_horizontal * Speed, rb.linearVelocity.y);
        }

        if (_isClimbing)
        {
            _isCrouching = false;
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Input.GetAxis("Vertical") * ClimbingSpeed);
        }
        else
        {
            rb.gravityScale = 1f;
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

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallSlideLayerMask);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded())
        {
            if (_isWallSliding) {return;}
            _isWallSliding = true;
            // rb.linearVelocity = new Vector2(rb.linearVelocity.x,
            //     Mathf.Clamp(rb.linearVelocity.y, -WallSlidingSpeed, float.MaxValue));
            rb.simulated = false;
        }
        else
        {
            if (!_isWallSliding){return;}
            _isWallSliding = false;
            rb.simulated = true;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void WallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpingDirection = -transform.localScale.x;
            _wallJumpingCounter = WallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }

        else
        {
            _wallJumpingCounter -= Time.deltaTime;
        }

        if (!Input.GetButtonDown("Jump") || !(_wallJumpingCounter > 0f)) return;
        _isWallJumping = true;
        rb.linearVelocity = new Vector2(_wallJumpingDirection * _wallJumpingPower.x, _wallJumpingPower.y);
        _wallJumpingCounter = 0f;

        if (!Mathf.Approximately(transform.localScale.x, _wallJumpingDirection))
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        Invoke(nameof(StopWallJumping), WallJumpingDuration);
    }

    private void StopWallJumping()
    {
        _isWallJumping = false;
    }

    private void Flip()
    {
        if ((!_isFacingRight || !(_horizontal < 0f)) && (_isFacingRight || !(_horizontal > 0f))) return;
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            _isLadder = true;
        }
        
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Ladder")) return;
        _isLadder = false;
        _isClimbing = false;
    }
}