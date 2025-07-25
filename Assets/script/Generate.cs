using UnityEngine;

public class Generate : MonoBehaviour
{
    private playerBehaviour m_player;

    public Transform _generationPointer;
    public BasicDock _basket;
    public Transform[] _ducks;
    private float m_timer;
    private const float Cooldown = 3f;

    private void Start()
    {
        if (m_player == null)
            m_player = GameObject.FindObjectOfType<playerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_player._startedGame)
            return;

        m_timer += Time.deltaTime;

        if (m_timer >= Cooldown)
        {
            m_timer = 0;
            BasicDock buffer = Instantiate(_basket);
            buffer.transform.position = _generationPointer.position;

            int rng = Random.Range(1, 7);
            if (rng < 5)
            {
                rng = Random.Range(1, 7);

                Transform duckBuffer = Instantiate(_ducks[rng/2]);
                duckBuffer.parent = buffer.transform.GetChild(0);
                duckBuffer.localPosition = Vector3.zero;
                buffer._isEmpty = false;
            }
        }
    }
}
