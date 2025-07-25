using UnityEngine;

public class StaticDock : BasicDock
{
    public int _gameValue = 0;
    private minigameManager m_manager;


    public override void Start()
    {
        if (m_manager == null)
        {
            m_manager = GameObject.FindObjectOfType<minigameManager>();
        }
    }

    public override void Update()
    {
        if (this.transform.GetChild(0).childCount > 0)
            _isEmpty = false;
        else
            _isEmpty = true;
    }

    public override void GetActivated()
    {
        if (_gameValue != 0)
            if ((int)_managedDuck._duckType == _gameValue || _managedDuck._duckType == duck.duckType.CLASSIC)
            {
                m_manager.StartMiniGame(_gameValue,this.transform.GetComponent<StaticDock>());
            }
    }
}
