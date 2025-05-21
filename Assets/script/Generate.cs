using UnityEngine;

public class Generate : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform _generationPointer;
    public BasicDock _basket;
    public Transform[] _ducks;
    private float m_timer;
    private const float Cooldown = 3f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= Cooldown)
        {
            m_timer = 0;
            BasicDock buffer = Instantiate(_basket);
            buffer.transform.position = _generationPointer.position;

            int rng = Random.Range(1, 4);
            if (rng < 3)
            {
                Transform duckBuffer = Instantiate(_ducks[0]);
                duckBuffer.parent = buffer.transform.GetChild(0);
                duckBuffer.localPosition = Vector3.zero;
                buffer._isEmpty = false;
            }
        }
    }
}
