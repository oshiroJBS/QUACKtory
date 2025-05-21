using UnityEngine;

public class StaticDock : BasicDock
{
    public int _gameValue = 0;
    // Start is called before the first frame update
    minigameManager m_manager;
    public override void Start()
    {
        if(m_manager == null)
        {
            m_manager = GameObject.FindObjectOfType<minigameManager>();
        }

        if (_gameValue == 0)
        {
            Debug.LogWarning("no game value set at object");
            _gameValue = 1;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (this.transform.GetChild(0).childCount > 0)
            _isEmpty = false;
        else
            _isEmpty = true;
    }

    public override void GetActivated()
    {
        Debug.Log("activate game");
        m_manager.StartMiniGame(_gameValue);
    }
}
