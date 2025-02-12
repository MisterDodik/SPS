using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] private float m_speed = 50f;
    [SerializeField] private float m_groundDrag = 10;
    [SerializeField] private float sprintingMultiplier;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float fallForce;

    private bool isSprinting = false;
    private Vector2 m_moveInput;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 1f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;

    [Space]
    //Camera rotation
    public Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ControlsManager.Instance.OnSprintPerformed += ControlsManager_OnSprintPerformed;
        ControlsManager.Instance.OnSprintCanceled += ControlsManager_OnSprintCanceled;
        ControlsManager.Instance.OnJump += ConstrolsManager_OnJump;
    }

    private void ConstrolsManager_OnJump(object sender, System.EventArgs e)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ControlsManager_OnSprintCanceled(object sender, System.EventArgs e)
    {
        isSprinting = false;
    }

    private void ControlsManager_OnSprintPerformed(object sender, System.EventArgs e)
    {
        isSprinting = true;
    }

    private void Update()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.y);

        if (flatVel.magnitude > m_speed)
        {
            Vector3 limitedVel = flatVel.normalized * m_speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundMask);
        if (isGrounded)
        {
            rb.linearDamping = m_groundDrag;
        }
        else
        {
            rb.linearDamping = 0f;
            rb.AddForce(Vector3.down * fallForce, ForceMode.Acceleration);
        }
    }


    private void FixedUpdate()
    {
        Vector3 moveDirection = cameraTransform.forward * m_moveInput.y + cameraTransform.right * m_moveInput.x;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * m_speed * (isSprinting ? sprintingMultiplier : 1), ForceMode.Force);
        }
        else
        {
            rb.AddForce(airMultiplier * moveDirection.normalized * m_speed * (isSprinting ? sprintingMultiplier : 1), ForceMode.Force);
        }
    }
    public void onMove(Vector2 input)
    {
        m_moveInput = input;
    }
}