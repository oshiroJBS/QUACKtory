using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class duck : MonoBehaviour
{
    public bool _Activated;
    private playerBehaviour player;
    // Start is called before the first frame update

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<playerBehaviour>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
