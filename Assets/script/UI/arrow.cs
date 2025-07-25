using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float _speed = 1;
    private minigameManager m_manager;
    public enum _Direction 
    {
        Up,
        Left
    };

    public _Direction _arrowDirection = _Direction.Left;

    // Start is called before the first frame update
    void Start()
    {
        if (m_manager == null)
            m_manager = GameObject.FindObjectOfType<minigameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_arrowDirection)
        {
            case _Direction.Up:
                this.transform.Translate(_speed * Vector3.up);
                break;

            case _Direction.Left:
                this.transform.Translate(_speed * Vector3.left);
                break;
                 
            default:
                break;
        }

        if(m_manager._activeGame == 1 && this.transform.localPosition.x <= -550)
        {
            Debug.Log("lose Game");
            m_manager.LoseMiniGame();
            m_manager._activeGame = 0;
        }
    }
}
