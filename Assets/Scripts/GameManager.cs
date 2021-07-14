using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float m_zBound = 28;
    public float ZBound
    {
        get { return m_zBound; }
        set { m_zBound = value; }
    }
    private float m_xBound = 28;
    public float XBound
    {
        get { return m_xBound; }
        set { m_xBound = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void WallBoundary()
    {
        if (transform.position.z > ZBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, ZBound);
        }
        else if (transform.position.z < -ZBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -ZBound);
        }
        if (transform.position.x > XBound)
        {
            transform.position = new Vector3(XBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -XBound)
        {
            transform.position = new Vector3(-XBound, transform.position.y, transform.position.z);
        }
    }
}
