using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;

    private Vector3 _acceleration = Vector3.zero;

    private const float AccelerationMagnitude = 2;

    private const float MaxVelocityMagnitude = 5;

    private const float DamagePerSecond = 50;

    private const float DamageRange = 10;
    private List<Circle> nearbyCircles = new List<Circle>();

    private void Update()
    {
        nearbyCircles.Clear();
        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, DamageRange);
        foreach (var nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider != null && nearbyCollider.TryGetComponent<Circle>(out var circle))
            {
                nearbyCircles.Add(circle);
            }
        }
        Move();
        DamageNearbyShapes();
        UpdateAcceleration();
    }

    private void Move()
    {
        _velocity += _acceleration * Time.deltaTime;
        if (_velocity.magnitude > MaxVelocityMagnitude)
        {
            _velocity = _velocity.normalized * MaxVelocityMagnitude;
        }
        transform.position += _velocity * Time.deltaTime;
    }

    private void UpdateAcceleration()
    {
        var direction = Vector3.zero;
        foreach (var circle in nearbyCircles)
        {
            direction += (circle.transform.position - transform.position) * circle.Health;
        }
        _acceleration = direction.normalized * AccelerationMagnitude;
    }

    private void DamageNearbyShapes()
    {
        // Si aucun cercle proche, on retourne a (0,0,0)
        if (nearbyCircles.Count == 0)
        {
            transform.position = Vector3.zero;
        }

        foreach (var circle in nearbyCircles)
        {
            circle.ReceiveHp(-DamagePerSecond * Time.deltaTime);
        }
    }
}