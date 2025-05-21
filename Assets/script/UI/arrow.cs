using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 1;
    public enum _Direction 
    {
        Up,
        Left
    };

    public _Direction _arrowDirection = _Direction.Left;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_arrowDirection)
        {
            case _Direction.Up:
                this.transform.Translate(speed * Vector3.up);
                break;

            case _Direction.Left:
                this.transform.Translate(speed * Vector3.left);
                break;
                 
            default:
                break;
        }
    }
}
