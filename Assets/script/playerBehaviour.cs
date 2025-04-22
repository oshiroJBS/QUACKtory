using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    public float _speed = 4f;
    private float m_smoothingTime = 0.2f;
    private float m_rotationSpeed = 50f;
    private Vector3 m_currentInput;
    public bool _action;

    private Transform m_target;
    private bool m_canPickUp = true;
    public Transform _duckTarget;

    public float _distanceTarget = 2f;

    public Rigidbody _rb;
    private Vector3 m_smoothVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _action = Input.GetKeyDown(KeyCode.Space);

        // movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        m_currentInput = new Vector3(h, 0f, v);
        Vector3 desiredVelocity = Vector3.Normalize(m_currentInput) * _speed;
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, desiredVelocity, ref m_smoothVelocity, m_smoothingTime);

        Rotate();
        //


        // ACTION
        if (m_canPickUp)
        {
            m_target = Maths.GetClosestObject(this.transform.position, "Duck");
        }
        else
        {
            m_target = Maths.GetClosestObject(this.transform.position, "Dock");
        }
        Debug.DrawLine(this.transform.position, m_target.position);

        if (Vector3.Distance(this.transform.position, m_target.position) < _distanceTarget)
        {
            if (_action)
            {
                if (m_canPickUp)
                {
                    PickUp(m_target);
                    Debug.Log("Pick up Duck");
                }
                else if (!m_canPickUp)
                {
                    PutDown(m_target);
                    Debug.Log("Put down Duck");
                }
            }
        }
    }

    private void Rotate()
    {
        if (m_currentInput != Vector3.zero)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            _rb.isKinematic = false;

            Quaternion targetRotation = Quaternion.LookRotation(this.m_currentInput.normalized, Vector3.up);
            float step = m_rotationSpeed * Time.fixedDeltaTime;
            Quaternion rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, step);
            this.transform.rotation = rotation;
        }
        else
            _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }
    private void PickUp(Transform PickedUpDuck)
    {
        BasicDock buffer;
        if (buffer = PickedUpDuck.parent.parent.GetComponent<BasicDock>())
        {
            buffer._isEmpty = true;
        }
        else
        {
            print("warning no BasicDock");
        }

        PickedUpDuck.parent = _duckTarget;
        PickedUpDuck.localPosition = Vector3.zero;
        PickedUpDuck.localRotation = Quaternion.Euler(new Vector3(-90, 0, 90));
        m_canPickUp = false;
    }

    private void PutDown(Transform TargetedDock)
    {
        BasicDock buffer;
        if (buffer = TargetedDock.GetComponent<BasicDock>())
        {
            if (!buffer._isEmpty)
                return;
            Transform PickedUpDuck = _duckTarget.GetChild(0);
            PickedUpDuck.parent = TargetedDock.GetChild(0);
            PickedUpDuck.localPosition = Vector3.zero;
            PickedUpDuck.localRotation = Quaternion.Euler(new Vector3(-90, 0, 90));
            m_canPickUp = true;
            buffer._isEmpty = false;
        }
        else
        {
            Debug.Log("no basicDock");
        }
    }
}
