using System;
using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] private float m_MovePower = 5; // The force added to the ball to move it.
    [SerializeField] private bool m_UseTorque = true; // Whether or not to use torque to move the ball.
    [SerializeField] private float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.
    [SerializeField] private float m_JumpPower = 2; // The force added to the ball when it jumps.
    [SerializeField] private SphereDamager _sphereDamageColider;
    private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
    private Rigidbody m_Rigidbody;
    private float _currentMovePowerMultiplyer;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        // Set the maximum angular velocity.
        GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
    }


    public void Move(Vector3 moveDirection, bool jump,bool crouch,bool run)
    {


        // If on the ground and jump is pressed...
        if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength) && jump)
        {
            // ... add force in upwards.
            m_Rigidbody.AddForce(Vector3.up*m_JumpPower, ForceMode.Impulse);
        }
        if(!Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength))
        {
            _currentMovePowerMultiplyer=  2f;
            if (crouch)
            {
                m_Rigidbody.AddForce(Vector3.down * m_JumpPower*_currentMovePowerMultiplyer, ForceMode.Impulse);
            }
        }
        else 
        {
            _currentMovePowerMultiplyer = 1f;
        }
        if (run) 
        {
            _currentMovePowerMultiplyer *= 1.5f;
        }
        
        
        // If using torque to rotate the ball...
        if (m_UseTorque)
        {
            // ... add torque around the axis defined by the move direction.
            m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x) * m_MovePower*_currentMovePowerMultiplyer);
        }
        else
        {
            // Otherwise add force in the move direction.
            m_Rigidbody.AddForce(moveDirection * m_MovePower*_currentMovePowerMultiplyer);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        _sphereDamageColider.StartDamage(m_Rigidbody.velocity.magnitude);
    }
    private void OnCollisionStay(Collision collision)
    {
       
    }
}

