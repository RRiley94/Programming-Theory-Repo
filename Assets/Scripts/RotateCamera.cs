using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : PlayerController
{
    public float rotationSpeed;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerRb.velocity * 999, Vector3.up);

        WallBoundary();
    }
}
