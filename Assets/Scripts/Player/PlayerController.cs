using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    public float m_speed = 50f;

    public float m_groundDrag = 10;

    public Transform cameraTransform;
    Vector2 m_moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping= m_groundDrag;
    }

    private void FixedUpdate()
    {
        Vector3 move = cameraTransform.forward * m_moveInput.y + cameraTransform.right * m_moveInput.x;
        move.y = 0;
        // rb.linearVelocity = new Vector3(input.x, rb.linearVelocity.y, input.y)*speed*Time.deltaTime;
        rb.AddForce(move.normalized * m_speed * Time.deltaTime, ForceMode.VelocityChange);
    }
    public void onMove(Vector2 input)
    {
        m_moveInput = input;
    }
}
