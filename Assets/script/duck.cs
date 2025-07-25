using UnityEngine;

public class duck : MonoBehaviour
{
    private playerBehaviour m_player;

    public int _scoreValue = 50;
    public enum duckType
    {
        CLASSIC, TUCK, DOCK, DUC
    }

    public duckType _duckType = duckType.CLASSIC;

    private void Start()
    {
        if (m_player == null)
            m_player = GameObject.FindObjectOfType<playerBehaviour>();

        if (this._duckType != duckType.TUCK)
            this.transform.GetChild(0).gameObject.SetActive(false);
        else
            this.transform.GetChild(0).gameObject.SetActive(true);

        if (this._duckType != duckType.CLASSIC)
        {
            _scoreValue = -5;
            Debug.Log("not classic");
        }
        else
            _scoreValue = 50;
    }

    public void GameCleared()
    {
        if (this._duckType != duckType.TUCK)
            this.transform.GetChild(0).gameObject.SetActive(true);
        else
            this.transform.GetChild(0).gameObject.SetActive(false);


        if (this._duckType != duckType.CLASSIC && _scoreValue < 200)
            _scoreValue = 200;
        else
            _scoreValue += 50;
    }

    private void OnDestroy()
    {
        this.m_player._score += this._scoreValue;
    }
}
