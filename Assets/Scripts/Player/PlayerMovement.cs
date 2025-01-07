
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallSlideLayerMask;

    private float _horizontal;
    private float _vertical;
    private float _speed = 5f;
    private float _jumpingPower = 5f;
    private bool _isFacingRight = true;

    private bool _isWallSliding;
    private float _wallSlidingSpeed = 0.5f;

    private bool _isWallJumping;
    private float _wallJumpingDirection;
    private float _wallJumpingTime = 0.2f;
    private float _wallJumpingCounter;
    private float _wallJumpingDuration = 0.2f;
    private Vector2 _wallJumpingPower = new(1f, 5f);
    
    private bool _isClimbing;
    private bool _isLadder;
    private float _climbingSpeed = 3f;
    private bool _isGrounded;
    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, _jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (_isLadder && Mathf.Abs(_vertical) > 0f)
        {
            _isClimbing = true;
            
        }

        WallSlide();
        WallJump();

        if (!_isWallJumping)
        {
            Flip();
        }
        
        // This Is Adding Wall Transparencie When Not Jumping
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer($"Slide Wall"), _isGrounded);
    }

    private void FixedUpdate()
    {
        if (!_isWallJumping)
        {
            rb.linearVelocity = new Vector2(_horizontal * _speed, rb.linearVelocity.y);
        }

        if (_isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Input.GetAxis("Vertical") * _climbingSpeed);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private bool IsGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return _isGrounded;
    }

    private bool IsWalled()
    {
            return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallSlideLayerMask);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && _horizontal != 0f)
        {
            _isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void WallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpingDirection = -transform.localScale.x;
            _wallJumpingCounter = _wallJumpingTime;

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

        Invoke(nameof(StopWallJumping), _wallJumpingDuration);
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