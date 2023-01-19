using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    [Header("Hover over fields to see their purpose.")]
    [Space]

    [SerializeField]
    [Tooltip("Changes the moving and max lateral velocity of the player.")]
    float MOVE_SPEED = 10f;

    [SerializeField]
    [Tooltip("Changes the force with which the player jumps.")]
    float JUMP_FORCE = 10f;

    [SerializeField]
    [Tooltip("Changes the force with which the player falls.")]
    float GRAVITY = -9.81f;

    [SerializeField]
    [Tooltip("Changes the max fallspeed for the player.")]
    float MAX_FALL_SPEED = -10f;

    [SerializeField]
    [Tooltip("If true, applies a braking force when no input is pressed.")]
    bool USE_BRAKES = false;

    [SerializeField]
    [Tooltip("Changes the rate at which the player slows down.")]
    float BRAKE_MULTIPLIER = 2f;

    CharacterController m_characterController;
    Vector3 m_currentInput;
    Vector3 m_currentImpact;
    float m_yvelo;
    bool lockout = false;

    private void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_currentInput = Vector3.zero;
        m_currentImpact = Vector3.zero;
        m_yvelo = 0f;

        lockout = !m_characterController.isGrounded;
    }

    private void Update()
    {
        if (!m_characterController.isGrounded)
        {
            m_yvelo += GRAVITY * Time.deltaTime;
        }

        if (lockout)
        {
            lockout = !m_characterController.isGrounded;

            if (m_yvelo < MAX_FALL_SPEED)
            {
                m_yvelo = MAX_FALL_SPEED;
            }
        }

        if (Input.anyKey)
        {
            m_currentInput.x = Input.GetAxis("Horizontal");
            m_currentInput.z = Input.GetAxis("Vertical");

            if (!lockout && Input.GetAxis("Jump") != 0f)
            {
                m_yvelo = JUMP_FORCE;

                lockout = true;
            }
        }

        else if (USE_BRAKES)
        {
            m_currentInput /= BRAKE_MULTIPLIER;
        }
    }

    
    private void FixedUpdate()
    {
        Vector3 v = transform.TransformDirection(m_currentInput);

        m_currentImpact.x = v.x * MOVE_SPEED * 0.05f;
        m_currentImpact.y = m_yvelo * 0.1f;
        m_currentImpact.z = v.z * MOVE_SPEED * 0.05f;

        m_characterController.Move(m_currentImpact);
    }
}
