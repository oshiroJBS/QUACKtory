//using System;
using UnityEngine;

public class minigameManager : MonoBehaviour
{
    private Canvas m_canva;
    public GameObject[] _gameScreens;
    public Transform _gameOneSpawnPoint;
    public Transform _gameOneEndPoint;
    public float triggerDistance = 100;
    public GameObject[] _arrows;
    public int _activeGame;

    private bool m_InCooldown;
    private int m_levelModifier;
    private const int m_levelTimer = 100;

    private int arrowNumb;
    private GameObject[] arrowsSequence;

    private string m_arrowInput;
    // Start is called before the first frame update
    void Start()
    {
        if (m_canva == null)
            m_canva = GameObject.FindObjectOfType<Canvas>();

        m_levelModifier = 0;
        for (int i = 0; i < _gameScreens.Length; i++)
        {
            _gameScreens[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_levelModifier <= 4)
            m_levelModifier += (int)(Time.unscaledTime / m_levelTimer);

        if (_activeGame != 0)
        {
            if (GetInput() != null)
            {
                switch (_activeGame)
                {
                    case 1://tuck
                        if (Vector3.Distance(_gameOneEndPoint.position, arrowsSequence[0].transform.position) <= triggerDistance)
                        {
                            m_arrowInput = GetInput();
                            if (m_arrowInput == arrowsSequence[0].name)
                            {
                                GameObject[] buffer = arrowsSequence;

                                Destroy(arrowsSequence[0]);

                                arrowsSequence = new GameObject[arrowsSequence.Length - 1];

                                for (int i = 0; i < arrowsSequence.Length; i++)
                                {
                                    arrowsSequence[i] = buffer[i + 1];
                                }
                            }
                        }
                        break;

                    case 2://dock
                        if (m_arrowInput == arrowsSequence[0].name)
                        {
                            GameObject[] buffer = arrowsSequence;

                            Destroy(arrowsSequence[0]);

                            arrowsSequence = new GameObject[arrowsSequence.Length - 1];

                            for (int i = 0; i < arrowsSequence.Length; i++)
                            {
                                arrowsSequence[i] = buffer[i + 1];
                            }
                        }
                        break;

                    case 3://crown

                        break;

                    default:
                        break;
                }

            }


            if (arrowsSequence.Length == 0)
                EndMiniGame();
        }
    }

    public void StartMiniGame(int index)
    {
        arrowNumb = Random.Range(4, 7) + m_levelModifier;
        arrowsSequence = new GameObject[arrowNumb];
        _gameScreens[index - 1].SetActive(true);
        _activeGame = index;

        Invoke("DelayFunction", 0.2f);
    }

    private void EndMiniGame()
    {
        for (int i = 0; i < _gameScreens.Length; i++)
        {
            _gameScreens[i].SetActive(false);
        }

        _activeGame = 0;
    }
    public void DelayFunction()
    {
        switch (_activeGame)
        {
            case 1:
                m_InCooldown = false;
                TuckMiniGame();
                break;

            case 2:

                m_InCooldown = false;
                DockMiniGame();
                break;

            case 3:
                m_InCooldown = false;
                CrownMiniGame();
                break;

            default:
                break;
        }
    }

    private void TuckMiniGame() //Game 1
    {
        if (!m_InCooldown && arrowNumb > 0)
        {
            Transform arrow;
            arrow = Instantiate(_arrows[Random.Range(0, _arrows.Length)]).transform;
            arrow.SetParent(m_canva.transform);
            arrow.GetComponent<arrow>()._arrowDirection = global::arrow._Direction.Left;
            arrow.GetComponent<arrow>().speed = 1.5f;
            arrow.position = _gameOneSpawnPoint.position;

            arrowsSequence[arrowsSequence.Length - arrowNumb] = arrow.gameObject;

            Invoke("DelayFunction", 0.5f - (m_levelModifier / 20));

            m_InCooldown = true;
            arrowNumb--;
        }
    }
    private void DockMiniGame()
    {
        for (int i = 0; i < arrowNumb; i++)
        {
            Transform arrow;
            arrow = Instantiate(_arrows[Random.Range(0, _arrows.Length)]).transform;
            arrow.SetParent(m_canva.transform);
            arrow.GetComponent<arrow>().speed = 0f;
        }
    }
    private void CrownMiniGame()
    {

    }
    private string GetInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return "UP(Clone)";
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return "DOWN(Clone)";
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return "LEFT(Clone)";
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return "RIGHT(Clone)";
        }

        return null;
    }
}
