using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class minigameManager : MonoBehaviour
{
    private playerBehaviour m_player;
    private Canvas m_canva;
    public GameObject[] _gameScreens;

    public Transform _gameOneSpawnPoint;
    public Transform _gameOneEndPoint;

    public Transform _gameTwoSpawnPoint;
    public Image _gameTwoProgress;
    private int m_wrongInput = 0;

    public Transform _gameThreeSpawnPoint;
    public Transform _gameThreeSkull;
    public GameObject _crownPic;
    public GameObject _duckPic;

    private Vector2 m_CrownXY;
    private Vector2 m_TargetXY;
    private Vector2[] m_SkullXY;
    private const int C_SkullMaxNumb = 4;


    private StaticDock m_managedDock;
    public float _triggerDistance = 100;
    public GameObject[] _arrows;
    public int _activeGame;

    private bool m_InCooldown;
    private int m_levelModifier;
    private const int m_levelTimer = 25;
    private float m_gameTimer;

    private int m_arrowNumb;
    private GameObject[] m_arrowsSequence;

    private string m_arrowInput;
    // Start is called before the first frame update
    void Start()
    {
        if (m_player == null)
            m_player = GameObject.FindObjectOfType<playerBehaviour>();

        if (m_canva == null)
            m_canva = GameObject.FindObjectOfType<Canvas>();

        m_levelModifier = 0;
        for (int i = 0; i < _gameScreens.Length; i++)
        {
            _gameScreens[i].SetActive(false);
        }

        _gameTwoProgress.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            QuitGame();

        if (!m_player._startedGame)
            return;

        m_gameTimer += Time.deltaTime;

        if (m_levelModifier <= 4)
        {
            if(m_gameTimer >= m_levelTimer)
            {
                m_levelModifier++;
                m_gameTimer = 0;
            }
        }


        if (_activeGame != 0)
        {
            m_player._inPlay = true;

            switch (_activeGame)
            {
                case 1://tuck
                    if (GetInput() != null && m_arrowsSequence[0] != null)
                    {
                        m_arrowInput = GetInput();

                        if (Vector3.Distance(_gameOneEndPoint.position, m_arrowsSequence[0].transform.position) <= _triggerDistance)
                        {
                            if (m_arrowInput == m_arrowsSequence[0].name)
                            {
                                _gameOneEndPoint.GetComponent<Animation>().Play("correct");

                                GameObject[] buffer = m_arrowsSequence;

                                Destroy(m_arrowsSequence[0]);

                                m_arrowsSequence = new GameObject[m_arrowsSequence.Length - 1];

                                for (int i = 0; i < m_arrowsSequence.Length; i++)
                                {
                                    m_arrowsSequence[i] = buffer[i + 1];
                                }
                                _gameOneEndPoint.GetComponent<Animation>().Play("correct");
                            }
                            else
                            {
                                _gameOneEndPoint.GetComponent<Animation>().Play("shake");
                            }
                        }
                        else
                        {
                            _gameOneEndPoint.GetComponent<Animation>().Play("shake");
                        }
                    }
                    if (m_arrowsSequence.Length == 0)
                        winMinigame();
                    break;

                case 2://dock
                    if (GetInput() != null && m_arrowsSequence[0] != null)
                    {
                        m_arrowInput = GetInput();

                        if (m_InCooldown) return;

                        if (m_arrowInput == m_arrowsSequence[0].name)
                        {
                            GameObject[] buffer = m_arrowsSequence;

                            Destroy(m_arrowsSequence[0]);

                            m_arrowsSequence = new GameObject[m_arrowsSequence.Length - 1];

                            for (int i = 0; i < m_arrowsSequence.Length; i++)
                            {
                                m_arrowsSequence[i] = buffer[i + 1];
                            }
                            _gameTwoProgress.fillAmount = ((float)m_arrowNumb - m_arrowsSequence.Length) / m_arrowNumb;
                        }
                        else
                        {
                            m_wrongInput++;

                            if (m_wrongInput >= 3)
                                LoseMiniGame();
                            return;
                        }
                    }
                    if (m_arrowsSequence.Length == 0)
                        winMinigame();
                    break;

                case 3://crown

                    if (m_InCooldown) return;

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        m_CrownXY.y -= 1;
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        m_CrownXY.y += 1;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        m_CrownXY.x -= 1;
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        m_CrownXY.x += 1;
                    }

                    m_CrownXY.x = Mathf.Clamp(m_CrownXY.x, 0, 4);
                    m_CrownXY.y = Mathf.Clamp(m_CrownXY.y, 0, 4);

                    _crownPic.transform.position = new Vector3(_gameThreeSpawnPoint.position.x + m_CrownXY.x * 200, _gameThreeSpawnPoint.position.y - m_CrownXY.y * 200, 0);

                    if (m_CrownXY == m_TargetXY)
                    {
                        winMinigame();
                    }
                    else
                    {
                        for (int i = 0; i < m_SkullXY.Length; i++)
                        {
                            if (m_CrownXY == m_SkullXY[i])
                            {
                                LoseMiniGame();
                            }
                        }
                    }

                    break;
            }
        }
        else m_player._inPlay = false;
    }


    #region MINI_GAME

    public void StartMiniGame(int index,StaticDock currentDock)
    {
        m_managedDock = currentDock;

        m_arrowNumb = Random.Range(4, 6) + m_levelModifier;
        m_arrowsSequence = new GameObject[m_arrowNumb];
        Debug.Log(m_arrowNumb + " + Modifier : " + m_levelModifier);
        if (index != 3) //game three activate itself 
            _gameScreens[index - 1].SetActive(true);
        _activeGame = index;
        m_InCooldown = true;

        switch (_activeGame)
        {
            case 1:
                DelayFunction();// tuckMinigame
                break;

            case 2:
                DockMiniGame();
                m_InCooldown = false;
                break;

            case 3:
                CrownMiniGame();
                m_InCooldown = false;
                break;

            default:
                break;
        }
    }
    private void TuckMiniGame() //Game 1
    {
        if (!m_InCooldown && m_arrowNumb > 0)
        {
            Transform arrow;
            arrow = Instantiate(_arrows[Random.Range(0, _arrows.Length)]).transform;
            arrow.SetParent(m_canva.transform);
            arrow.GetComponent<arrow>()._arrowDirection = global::arrow._Direction.Left;
            arrow.GetComponent<arrow>()._speed = 1.5f;
            arrow.position = _gameOneSpawnPoint.position;

            m_arrowsSequence[m_arrowsSequence.Length - m_arrowNumb] = arrow.gameObject;

            Invoke("DelayFunction", 0.5f);

            m_InCooldown = true;
            m_arrowNumb--;
        }
    }

    public void DelayFunction()
    {
        m_InCooldown = false;
        TuckMiniGame();
    }

    private void DockMiniGame()
    {
        for (int i = 0; i < m_arrowNumb; i++)
        {
            Transform arrow;
            arrow = Instantiate(_arrows[Random.Range(0, _arrows.Length)]).transform;
            arrow.SetParent(_gameTwoSpawnPoint.transform);
            arrow.GetComponent<arrow>()._speed = 0f;

            arrow.localPosition = new Vector3(-50 + ((100 * (i + 1)) / (m_arrowNumb + 1)), 0, 0);

            m_arrowsSequence[i] = arrow.gameObject;
        }
    }

    private void CrownMiniGame()
    {
        m_CrownXY.x = Random.Range(0, 5);
        m_CrownXY.y = 0;
        _crownPic.transform.position = new Vector3(_gameThreeSpawnPoint.position.x + m_CrownXY.x * 200, _gameThreeSpawnPoint.position.y, 0);

        int PosX = Random.Range(0, 5);
        _duckPic.transform.position = new Vector3(_gameThreeSpawnPoint.position.x + PosX * 200, _gameThreeSpawnPoint.position.y - 800, 0);
        m_TargetXY = new Vector2(PosX, 4);

        m_SkullXY = new Vector2[C_SkullMaxNumb];
        for (int i = 0; i < C_SkullMaxNumb; i++)
        {
            PosX = Random.Range(0, 5);
            int PosY = Random.Range(1, 4);
            Transform skull;
            skull = Instantiate(_gameThreeSkull).transform;
            skull.SetParent(_gameThreeSpawnPoint.transform);
            skull.position = new Vector3(_gameThreeSpawnPoint.position.x + PosX * 200, _gameThreeSpawnPoint.position.y - PosY * 200, 0);
            skull.tag = "skull";
            m_SkullXY[i] = new Vector2(PosX, PosY);
        }
        _gameScreens[2].SetActive(true); //Activate screen
    }

    public void winMinigame()
    {
        this.m_managedDock._managedDuck.GameCleared();
        EndMiniGame();
    }
    public void LoseMiniGame()
    {
        EndMiniGame();
    }

    private void EndMiniGame()
    {
        Debug.Log("END GAME " + _activeGame);


        switch (_activeGame)
        {
            case 1://tuck

                for (int i = 0; i < m_arrowsSequence.Length; i++)
                {
                    Destroy(m_arrowsSequence[i]);
                }
                m_arrowsSequence = null;
                break;

            case 2://dock

                for (int i = 0; i < m_arrowsSequence.Length; i++)
                {
                    Destroy(m_arrowsSequence[i]);
                }
                m_arrowsSequence = null;

                m_wrongInput = 0;

                _gameTwoProgress.fillAmount = 0;
                break;

            case 3://crown

                GameObject[] buffer = GameObject.FindGameObjectsWithTag("skull");
                if (buffer != null)
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        Destroy(buffer[i]);
                        Debug.Log("skull removed :" + i);
                    }
                }
                else
                {
                    Debug.Log("no skull");
                }
                break;

            default:
                break;
        }
        for (int i = 0; i < _gameScreens.Length; i++)
        {
            _gameScreens[i].SetActive(false);
        }

        _activeGame = 0;
    }

    #endregion

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

    public void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
