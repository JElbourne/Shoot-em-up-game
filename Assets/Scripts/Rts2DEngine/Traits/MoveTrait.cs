using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionTrait))]
[RequireComponent(typeof(CharacterStats))]
public class MoveTrait : MonoBehaviour {


    Animator m_CharacterAnimator;
    CharacterStats m_CharacterStats;
    CollisionTrait m_CollisionTrait;
    Vector2 m_FacingDirection;
    float velocityXSmoothing;
    float velocityYSmoothing;
    float zAmount;

    [HideInInspector]
    public Vector3 lastVelocity { get; private set; }

    private void Awake()
    {
        m_CharacterStats = GetComponent<CharacterStats>();
        m_CollisionTrait = GetComponent<CollisionTrait>();
        m_CharacterAnimator = GetComponentInChildren<Animator>();
    }

    public Vector2 GetFacingDirection()
    {
        return m_FacingDirection;
    }

    public void Move(bool rotatePosition = false)
    {
        Move(Vector2.zero, rotatePosition);
    }

    public void Move(Vector2 _input, bool _rotatePosition = false)
    {
        if (m_CharacterStats)
        {
            float maxSpeed = m_CharacterStats.maxSpeed.value;
            float accelerationTime = m_CharacterStats.accelerationTime;
            Move(maxSpeed, accelerationTime, _input, _rotatePosition);
        }
    }

    public void Move(float _maxSpeed, float _accelerationTime, Vector2 _input, bool rotatePosition = false)
    {
        //if (_input.x > 0.5f || _input.x < -0.5f || _input.y > 0.5f || _input.y < -0.5f)
        //{
        //    m_FacingDirection = _input;
            // Debug.Log("Current Dir: " + m_CurrentDirection);
        //}

        Vector3 m_Pos = transform.position;
        Vector3 m_Velocity = CalculateVelocity(_input, _maxSpeed, _accelerationTime);
        
        if (m_CollisionTrait)
        {
            m_CollisionTrait.UpdateRaycastOrigins();
            m_CollisionTrait.collisions.Reset();
            m_CollisionTrait.collisions.velocityOld = m_Velocity;

            m_CollisionTrait.HorizontalCollisions(ref m_Velocity);
            m_CollisionTrait.VerticalCollisions(ref m_Velocity);
        }

        if (rotatePosition)
        {
            m_Pos += transform.rotation * m_Velocity;
        }
        else
        {
            m_Pos += m_Velocity;
        }

        transform.position = m_Pos;
    }

    Vector3 CalculateVelocity(Vector2 _input, float _maxSpeed, float _accelerationTime)
    {
        Vector3 m_Velocity = Vector3.zero;
        float targetVelocityX = _input.x * _maxSpeed;
        m_Velocity.x = Mathf.SmoothDamp(
            m_Velocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            _accelerationTime);

        float targetVelocityY = _input.y * _maxSpeed;
        m_Velocity.y = Mathf.SmoothDamp(
            m_Velocity.y,
            targetVelocityY,
            ref velocityYSmoothing,
            _accelerationTime);

        m_Velocity.z = 0f;
        lastVelocity = m_Velocity;
        UpdateCurrentDirection(m_Velocity);

        return m_Velocity * Time.deltaTime;
    }

    void UpdateCurrentDirection(Vector3 _velocity)
    {
        Vector2 m_TempFacingDirection = Vector2.zero;

        if (Mathf.Abs(_velocity.x) > Mathf.Abs(_velocity.y))
        {
            // Moving Horizontally
            m_TempFacingDirection.x = (_velocity.x > 0) ? 1 : -1;
            m_TempFacingDirection.y = 0;
        }

        if (Mathf.Abs(_velocity.x) < Mathf.Abs(_velocity.y))
        {
            // Moving Vertically
            m_TempFacingDirection.y = (_velocity.y > 0) ? 1 : -1;
            m_TempFacingDirection.x = 0;
        }

        if (m_TempFacingDirection != Vector2.zero)
        {
            m_FacingDirection = m_TempFacingDirection;
        }

        if (m_CharacterAnimator != null)
        {
            m_CharacterAnimator.SetFloat("directionX", m_FacingDirection.x);
            m_CharacterAnimator.SetFloat("directionY", m_FacingDirection.y);
        }
    }
}
