using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class KnifeThrowerMovement : BasicEnemyMovement
{
    private enum EnemyState
    {
        Standing,
        Crouching
    }
    
    [Header("Knife Thrower Settings")]
    [SerializeField] private Transform knifeThrowPoint;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwCooldown;
    
    private EnemyState _state;
    private float _throwCooldown;
    private bool _canPatrol = true;
    
    private void FixedUpdate()
    {
        CheckingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        CheckingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        PlayerInRange = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        if (!PlayerInRange || _canPatrol)
        {
            Patrolling();
        }
        
        if (PlayerInRange)
        {
            OnPlayerSeen();
        }
    }
    
    private void OnPlayerSeen()
    {
        FlipTowardsPlayer();
        if (_throwCooldown <= 0)
        {
            _canPatrol = false;
            RandomizeState();
            StartCoroutine(StopAndMove());
            _throwCooldown = throwCooldown;
        }
        else
        {
            _throwCooldown -= Time.deltaTime;
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator StopAndMove()
    {
        EnemyRigidbody2D.linearVelocity = Vector2.zero;
        EnemyAnimator.SetTrigger(_state == EnemyState.Standing ? "SeenPlayerStanding" : "SeenPlayerCrouching");
        yield return new WaitForSeconds(0.2f);
        ThrowKnife();
    }
    
    private void ThrowKnife()
    {
        var knife = EnemyProjectilePool.Instance.Get();
        knife.transform.position = knifeThrowPoint.position;
        if (FacingRight){return;}
        knife.transform.Rotate(0, 180, 0);
    }
    
    public void EnablePatrol()
    {
        _canPatrol = true;
    }

    private void RandomizeState()
    {
        _state = (EnemyState)Random.Range(0, 2);
    }
    
    public new void Reset()
    {
        base.Reset();
        _state = EnemyState.Standing;
        _throwCooldown = 0;
        _canPatrol = true;
    }
}
