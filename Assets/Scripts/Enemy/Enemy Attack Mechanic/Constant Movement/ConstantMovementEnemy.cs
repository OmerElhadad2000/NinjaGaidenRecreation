using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class ConstantMovementEnemy : MonoBehaviour
{
    private Transform[] _movementPoints; 
    
    [SerializeField] private float movementSpeed;
    
    private int _currentPointIndex;

    // private void OnEnable()
    // {
    //     transform.position = movementPoints[0].transform.position;
    // }

    private void Update()
    {
        if (_currentPointIndex < _movementPoints.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                _movementPoints[_currentPointIndex].transform.position, movementSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _movementPoints[_currentPointIndex].transform.position) < 0.1f)
            {
                Debug.Log("Reached point " + _currentPointIndex);
                _currentPointIndex++;
            }

            if (_currentPointIndex == _movementPoints.Length)
            {
                Debug.Log("Reached the end of the path");
                _currentPointIndex = 0;
                Debug.Log("Starting from the beginning");
            }
        }
    }
    
    public void SetMovementPoints(Transform[] movementPoints)
    {
        _movementPoints = movementPoints;
    }
}
