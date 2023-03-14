using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private float _speed = 5.5f;
    private float _distancePassed;
    public void PathFollow()
    {
        _distancePassed += _speed * Time.deltaTime;
        transform.SetPositionAndRotation(_pathCreator.path.GetPointAtDistance(_distancePassed), _pathCreator.path.GetRotationAtDistance(_distancePassed));
    }
}