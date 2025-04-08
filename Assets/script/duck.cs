using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class duck : MonoBehaviour
{
    public bool Activated;
    private playerBehaviour player;
    public float distanceTarget = 1.5f;
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
        if (Vector3.Distance(this.transform.position, player.transform.position) < distanceTarget )
        {
            this.Activated = true;
        }
        else
        {
            this.Activated = false;
        }

        if (Activated)
        {
            this.transform.localScale = Vector3.one * 1f;
        }
        else
        {
            this.transform.localScale = Vector3.one * 0.5f;
        }
    }
}
