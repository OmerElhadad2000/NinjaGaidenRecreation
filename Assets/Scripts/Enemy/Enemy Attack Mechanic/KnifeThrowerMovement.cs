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
    
    [SerializeField] private Transform knifeThrowPoint;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwCooldown;
    private EnemyState _state;
    private float _throwCooldown;
    private bool canPatrol = true;
    
    private void FixedUpdate()
    {
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        // AnimationController();
        if (!canSeePlayer || canPatrol)
        {
            Patrolling();
        }
        
        if (canSeePlayer)
        {
            OnPlayerSeen();
        }
    }
    
    private void OnPlayerSeen()
    {
        FlipTowardsPlayer();
        if (_throwCooldown <= 0)
        {
            canPatrol = false;
            RandomizeState();
            ThrowKnife();
            _throwCooldown = throwCooldown;
            StartCoroutine(StopAndMove());
        }
        else
        {
            _throwCooldown -= Time.deltaTime;
        }
    }
    
    private IEnumerator StopAndMove()
    {
        yield return new WaitForSeconds(3);
        canPatrol = true;
    }
    
    private void ThrowKnife()
    {
        print("Throwing knife from the state: " + _state);
        // get a knife prefab from the pool
        
        // throw the knife from the state
    }

    private void RandomizeState()
    {
        _state = (EnemyState)Random.Range(0, 2);
    }
}
