using UnityEngine;

public class BasicDock : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _isEmpty = true;

    private Transform[] _wayPoints = new Transform[5];
    private float m_speed = 1f;
    private int m_wayPointsIndex = 0;

    private void Awake()
    {
        _isEmpty = true;
    }
    virtual public void Start()
    {
        GameObject[] Objects = GameObject.FindGameObjectsWithTag("Waypoints");

        for (int i = 0; i < _wayPoints.Length; i++)
        {
            int index = 0;
            float distTarget = Mathf.Infinity;

            for (int o = 0; o < Objects.Length; o++)
            {
                if (Objects[o] != null)
                {
                    if (Vector3.Distance(this.transform.position, Objects[o].transform.position) < distTarget)
                    {
                        distTarget = Vector3.Distance(this.transform.position, Objects[o].transform.position);
                        index = o;
                    }
                }
            }

            _wayPoints[i] = Objects[index].transform;
            Objects[index] = null;
        }
    }

    // Update is called once per frame
    virtual public void Update()
    {
        Move();
    }
    private void Move()
    {
        if (Vector3.Distance(this.transform.position, _wayPoints[m_wayPointsIndex].position) <= 0.2f)
        {
            m_wayPointsIndex++;
        }
        if (m_wayPointsIndex >= _wayPoints.Length)
        {
            Destroy(this.gameObject);
        }

        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _wayPoints[m_wayPointsIndex].position, m_speed * Time.deltaTime);
        }
    }

    virtual public void GetActivated() { }
}
