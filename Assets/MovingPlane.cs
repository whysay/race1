using UnityEngine;
using System.Collections;

public class MovingPlane : MonoBehaviour {

    Vector3 m_startPos;
	// Use this for initialization
	void Start () {
        m_startPos = transform.position;
	}

    public Vector3 eulerAngleVelocity = new Vector3(0, 0, 0);
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
    }

	// Update is called once per frame
	void Update () {
        //if (rigidbody.isKinematic)
        //{
        //    rigidbody.isKinematic = false;
        //    rigidbody.AddForce(-500.0f, 500.0f, 0.0f);
        //}
        ////m_zRot += Time.deltaTime*0.3f;
        ////if (360.0f < m_zRot)
        ////    m_zRot -= 360.0f;
        ////transform.Rotate(0.0f, 0.0f, Time.deltaTime*90.0f);
        ////transform.position += new Vector3(Time.deltaTime*3.0f, Time.deltaTime * 10.0f, 0.0f);
        //if (7 < transform.position.y)
        //{
        //    rigidbody.isKinematic = true;
        //    transform.position = m_startPos;
        //}
	}
}
