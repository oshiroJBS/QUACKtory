using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceCamera : MonoBehaviour
{
    private Camera m_camera;
    // Start is called before the first frame update
    void Start()
    {
        if (m_camera == null)
            m_camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Maths.RotateToLookAt(this.transform,new Vector3(this.transform.position.x, m_camera.transform.position.y, m_camera.transform.position.z));
    }
}
