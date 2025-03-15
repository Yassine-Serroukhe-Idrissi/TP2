using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;

    private Vector3 _acceleration = Vector3.zero;

    private const float AccelerationMagnitude = 2;

    private const float MaxVelocityMagnitude = 5;

    private const float DamagePerSecond = 50;

    private const float DamageRange = 10;

    private void Update()
    {
        Move();

        /*Les deux fonction DamageNearbyCircles et UpdateAcceleration utilise toute deux le m�me voisinage
        On le d�termine donc en amont.*/
        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, DamageRange);
        //On r�cup�re directemment les cercles � partir des colliders pr�lev�s juste au dessus.
        /*Dans le cas o� un de ces colliders ne serait pas attach� � un gameObject avec une composant Circle,
        nous avons pr�f�r� utiliser une liste que l'on transforme ensuite en array pour une plus grande performance
        du fait d'un acc�s contigu des valeurs en m�moire.*/
        var nearbyCirclesList = new List<Circle>();
        for (var i = 0; i < nearbyColliders.Length; i++)
        {
            var nearbyCollider = nearbyColliders[i];
            if (nearbyCollider != null && nearbyCollider.TryGetComponent<Circle>(out var circle))
            {
                nearbyCirclesList.Add(circle);
            }
        }
        var nearbyCircles = nearbyCirclesList.ToArray();

        DamageNearbyCircles(nearbyCircles);
        UpdateAcceleration(nearbyCircles);
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

    private void UpdateAcceleration(Circle[] nearbyCircles)
    {
        var direction = Vector3.zero;
        foreach (var circle in nearbyCircles)
        {
            direction += (circle.transform.position - transform.position) * circle.Health;
        }
        _acceleration = direction.normalized * AccelerationMagnitude;
    }

    private void DamageNearbyCircles(Circle[] nearbyCircles)
    {
        // Si aucun cercle proche, on retourne a (0,0,0)
        if (nearbyCircles.Length == 0)
        {
            transform.position = Vector3.zero;
        }

        //Calcul des HPs enlev�s avant d'it�rer sur tous les cercles voisins.
        float hpReceived = -DamagePerSecond * Time.deltaTime;
        foreach (var circle in nearbyCircles)
        {
            circle.ReceiveHp(hpReceived);
        }
    }
}