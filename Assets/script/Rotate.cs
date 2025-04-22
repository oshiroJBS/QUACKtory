using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed = 10f;
    public void Update()
    {
        this.transform.Rotate(0 , Time.deltaTime* rotateSpeed, 0);
    }
}
