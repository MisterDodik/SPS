using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    //Movement
    Vector2 m_moveInput;
    public float m_speed = 50f;
    public float m_groundDrag = 10;

    //Camera rotation
    public Transform cameraTransform;

    //Crosshair
    public LayerMask layerMask;
    [SerializeField] private RawImage crosshairImage;

    private bool isSprinting = false;
    [SerializeField] private float sprintingMultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ControlsManager.Instance.OnSprintPerformed += ControlsManager_OnSprintPerformed;
        ControlsManager.Instance.OnSprintCanceled += ControlsManager_OnSprintCanceled;
        rb = GetComponent<Rigidbody>();
        rb.linearDamping= m_groundDrag;
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
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, 2, layerMask))
        {
            crosshairImage.color = Color.black;
        }
        else
        {
            crosshairImage.color = Color.white;
        }
    }


    private void FixedUpdate()
    {
        Vector3 move = cameraTransform.forward * m_moveInput.y + cameraTransform.right * m_moveInput.x;
        move.y = 0;
        rb.AddForce(move.normalized * m_speed * (isSprinting ? sprintingMultiplier : 1) * Time.deltaTime, ForceMode.VelocityChange);
    }
    public void onMove(Vector2 input)
    {
        m_moveInput = input;
    }
}
