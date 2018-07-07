using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionTrait))]
[RequireComponent(typeof(EntityController))]
public class MoveTrait : MonoBehaviour {

    EntityController m_EntityController;
    CollisionTrait m_CollisionTrait;
    Vector2 m_CurrentDirection;
    float velocityXSmoothing;
    float velocityYSmoothing;
    float zAmount;

    private void Awake()
    {
        m_EntityController = GetComponent<EntityController>();
        m_CollisionTrait = GetComponent<CollisionTrait>(); 
    }

    public Vector2 GetCurrentDirection()
    {
        return m_CurrentDirection;
    }

    public void Move(bool rotatePosition = false)
    {
        Move(Vector2.zero, rotatePosition);
    }

    public void Move(Vector2 _input, bool _rotatePosition = false)
    {
        if (m_EntityController)
        {
            float maxSpeed = m_EntityController.maxSpeed;
            float accelerationTime = m_EntityController.accelerationTime;
            Move(maxSpeed, accelerationTime, _input, _rotatePosition);
        }
    }

    public void Move(float _maxSpeed, float _accelerationTime, Vector2 _input, bool rotatePosition = false)
    {
        if (_input.x > 0.5f || _input.x < -0.5f || _input.y > 0.5f || _input.y < -0.5f)
        {
            m_CurrentDirection = _input;
            // Debug.Log("Current Dir: " + m_CurrentDirection);
        }

        Vector3 pos = transform.position;
        Vector3 velocity = CalculateVelocity(_input, _maxSpeed, _accelerationTime);
        
        if (m_CollisionTrait)
        {
            m_CollisionTrait.UpdateRaycastOrigins();
            m_CollisionTrait.collisions.Reset();
            m_CollisionTrait.collisions.velocityOld = velocity;

            m_CollisionTrait.HorizontalCollisions(ref velocity);
            m_CollisionTrait.VerticalCollisions(ref velocity);
        }

        if (rotatePosition)
        {
            pos += transform.rotation * velocity;
        }
        else
        {
            pos += velocity;
        }

        transform.position = pos;
    }

    Vector3 CalculateVelocity(Vector2 _input, float _maxSpeed, float _accelerationTime)
    {
        Vector3 velocity = Vector3.zero;
        float targetVelocityX = _input.x * _maxSpeed;
        velocity.x = Mathf.SmoothDamp(
            velocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            _accelerationTime);

        float targetVelocityY = _input.y * _maxSpeed;
        velocity.y = Mathf.SmoothDamp(
            velocity.y,
            targetVelocityY,
            ref velocityYSmoothing,
            _accelerationTime);

        velocity.z = 0f;
        return velocity * Time.deltaTime;
    }
}
