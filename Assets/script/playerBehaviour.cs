using System;
using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    private float m_speed = 4f;
    private float m_smoothingTime = 0.2f;
    private float m_rotationSpeed = 50f;
    private Vector3 m_currentInput;
    public bool _action;

    private Rigidbody m_rb;
    private Vector3 m_smoothVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        m_currentInput = new Vector3(h, 0f, v);
        Vector3 desiredVelocity = Vector3.Normalize(m_currentInput) * m_speed;
        m_rb.velocity = Vector3.SmoothDamp(m_rb.velocity, desiredVelocity, ref m_smoothVelocity, m_smoothingTime);

        Rotate();
        //


        _action = Input.GetKeyDown(KeyCode.Space);
        if (_action)
        {
            Debug.Log("action");
        }
    }

    private void Rotate()
    {
        if (this.m_currentInput == Vector3.zero)
        {
            this.m_rb.constraints = RigidbodyConstraints.FreezeRotation;
            this.m_rb.constraints = RigidbodyConstraints.FreezePositionY;
            return;
        }
        this.m_rb.constraints = RigidbodyConstraints.None;
        this.m_rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        this.m_rb.constraints = RigidbodyConstraints.FreezeRotationX;

        Quaternion targetRotation = Quaternion.LookRotation(this.m_currentInput.normalized, Vector3.up);
        float step = m_rotationSpeed * Time.fixedDeltaTime;
        Quaternion rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, step);
        this.transform.rotation = rotation;
    }
}
