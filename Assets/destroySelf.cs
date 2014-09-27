using UnityEngine;
using System.Collections;

public class destroySelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    float m_destroyTime = 0;
	// Update is called once per frame
	void Update () {
        m_destroyTime += Time.deltaTime;
	}
}
