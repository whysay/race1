using UnityEngine;
using System.Collections;

public class MovingCapsule : MonoBehaviour {

    Vector3 m_startPos;
    bool m_movDir = false;
	// Use this for initialization
	void Start () {
        m_startPos = transform.position;
        //rigidbody.AddForce(0.0f, 0.0f, -1000.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (10 < (m_startPos - transform.position).magnitude)
            m_movDir = !m_movDir;

        //if (rigidbody.isKinematic)
        //{
        //    rigidbody.isKinematic = false;
        //    if (180.0f < transform.position.z)
        //        rigidbody.AddForce(0.0f, 0.0f, -1000.0f);
        //    else
        //        rigidbody.AddForce(0.0f, 0.0f, 1000.0f);

        //    return;
        //}

        //if (195.0f < transform.position.z)
        //    rigidbody.isKinematic = true;
        //else if (transform.position.z < 165.0f)
        //    rigidbody.isKinematic = true;

        if (m_movDir)
            transform.position += new Vector3(Time.deltaTime, 0.0f, Time.deltaTime * 10.0f);
        else
            transform.position -= new Vector3(Time.deltaTime, 0.0f, Time.deltaTime * 10.0f);

        transform.Rotate(Time.deltaTime * 45.0f, 0.0f, 0.0f);
	}
}
