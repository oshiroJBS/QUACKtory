using TMPro;
using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    public bool _inPlay = true;
    public bool _startedGame = false;

    public float _speed = 4f;
    private float m_smoothingTime = 0.2f;
    private float m_rotationSpeed = 50f;
    private Vector3 m_currentInput;
    public bool _action;

    public int _score = 0;

    private Transform m_target;
    private bool m_canPickUp = true;
    public Transform _duckTarget;

    public float _distanceTarget = 2f;

    private float m_gameTimer;

    public Rigidbody _rb;
    private Vector3 m_smoothVelocity = Vector3.zero;

    [SerializeField] private TextMeshProUGUI m_scoreText;
    //
    [SerializeField] private TextMeshProUGUI m_scoreWinTxt;
    [SerializeField] private TextMeshProUGUI m_TimerWinTxt;
    [SerializeField] private GameObject m_endScreen;



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _startedGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_startedGame) return;

        m_gameTimer += Time.deltaTime;

        _action = Input.GetKeyDown(KeyCode.Space);

        // movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        m_currentInput = new Vector3(h, 0f, v);

        if (_inPlay) m_currentInput = Vector3.zero;

        Vector3 desiredVelocity = Vector3.Normalize(m_currentInput) * _speed;
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, desiredVelocity, ref m_smoothVelocity, m_smoothingTime);

        Rotate();
        //

        // ACTION

        if (_inPlay) return;

        if (m_canPickUp)
            m_target = Maths.GetClosestObject(this.transform.position, "Duck");
        else
            m_target = Maths.GetClosestObject(this.transform.position, "Dock");

        if (m_target != null)
        {
            if (Vector3.Distance(this.transform.position, m_target.position) < _distanceTarget)
            {
                if (_action)
                {
                    if (m_canPickUp)
                    {
                        Debug.Log(m_target.name);
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

        m_scoreText.text = _score.ToString();

        if (_score >= 2500 || m_gameTimer >= 180)
        {
            m_scoreWinTxt.text = "Your Score: " + _score.ToString();
            m_TimerWinTxt.text = "Timer : " + ((int)m_gameTimer).ToString();
            _startedGame = false;
            this.m_endScreen.SetActive(true);
        }
    }

    private void Rotate()
    {
        if (m_currentInput != Vector3.zero)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            _rb.isKinematic = false;

            Quaternion targetRotation = Quaternion.LookRotation(this.m_currentInput/*.normalized*/, Vector3.up);
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
            buffer._isEmpty = true;
        else
            print("warning no BasicDock");

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

            duck PickedUpDuck = _duckTarget.GetChild(0).GetComponent<duck>();
            PickedUpDuck.transform.parent = TargetedDock.GetChild(0);
            PickedUpDuck.transform.localPosition = Vector3.zero;
            PickedUpDuck.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 90));
            m_canPickUp = true;

            buffer._managedDuck = PickedUpDuck;
            buffer.GetActivated();

            buffer._isEmpty = false;
        }
        else
            Debug.Log("no basicDock");
    }

    public void StartGame()
    {
        _startedGame = true; // rose noir, hornet, mog, 
    }
}
