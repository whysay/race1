using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class move : MonoBehaviour
{


    WheelCollider FL_Wheel;
    WheelCollider FR_Wheel;
    WheelCollider BL_Wheel;
    WheelCollider BR_Wheel;

    Transform FL_Pos;
    Transform FR_Pos;

    Transform FL_Mesh;
    Transform FR_Mesh;
    Transform BL_Mesh;
    Transform BR_Mesh;

    public Vector3 center_of_mass;

    public float max_steer = 50;
    public float m_curSteer = 0;

    public float max_torque = 10;
    public float max_brake = 10;

    public float motor = 0;
    public float m_rpm = 0;
    float brake = 0;

    //bool go_forward = true;

    float forward = 0;
    float back = 0;

    public float degree = 0;
    public Vector3 crossVal = new Vector3();
    public float current_speed = 0;
    public float steer = 0;

    public float speed = 0;
    public float dotVal = 0.0f;

    public GameObject m_ball;
    public float m_ballCreateTime = 0;

    Vector3 [] m_targetPos = new Vector3[6];
    public bool[] m_bArrived = { false, false, false, false, false, false };

    public List<GameObject> m_listObj = new List<GameObject>();


    void Start()
    {
        Physics.gravity = new Vector3(0.0f, -30.0f, 0.0f);
        FL_Wheel = transform.FindChild("fl_col").transform.collider as WheelCollider;
        FR_Wheel = transform.FindChild("fr_col").transform.collider as WheelCollider;
        BL_Wheel = transform.FindChild("rl_col").transform.collider as WheelCollider;
        BR_Wheel = transform.FindChild("rr_col").transform.collider as WheelCollider;

        FL_Pos = transform.FindChild("fl_col").transform;
        FR_Pos = transform.FindChild("fr_col").transform;

        FL_Mesh = transform.FindChild("SUV_wheel_front_left").transform;
        FR_Mesh = transform.FindChild("SUV_wheel_front_right").transform;
        BL_Mesh = transform.FindChild("SUV_wheel_rear_left").transform;
        BR_Mesh = transform.FindChild("SUV_wheel_rear_right").transform;

        rigidbody.centerOfMass = center_of_mass;

        m_targetPos[0] = new Vector3(130.0f,0.0f,145.0f);
        m_targetPos[1] = new Vector3(70.0f, 0.0f, 145.0f);
        m_targetPos[2] = new Vector3(60.0f, 0.0f, 130.0f);
        m_targetPos[3] = new Vector3(70.0f, 0.0f, 110.0f);
        m_targetPos[4] = new Vector3(130.0f, 0.0f, 110.0f);
        m_targetPos[5] = new Vector3(140.0f, 0.0f, 130.0f);
    }

    //bool isCurve = false;
    void FixedUpdate()
    {
        m_ballCreateTime += Time.deltaTime;
        if (1.0f < m_ballCreateTime)
        {
            m_ballCreateTime = 0.0f;
            GameObject cloneBall = (GameObject)Instantiate(m_ball);
            m_listObj.Add(cloneBall);
            if (10 < m_listObj.Count)
            {
                GameObject[] arr = m_listObj.ToArray();
                Destroy(arr[0]);
                m_listObj.RemoveAt(0);
            }
                
            //m_ball.transform.position = transform.position;
            Vector3 curPos = transform.position;
            curPos.y = 10.0f;
            cloneBall.transform.position = curPos;
            //cloneBall.transform.Translate(0.0f, 10.0f, 0,0f);
        }
        current_speed = rigidbody.velocity.sqrMagnitude;

        bool isArrivedDest = true;
        if (0.0f < current_speed)
        {
            Vector3 centerPos = new Vector3(120.0f, 0.0f, 120.0f);
            Vector3 dirVec = centerPos - transform.position;

            steer = 0.0f;
            for (int i = 0; i < 6; ++i)
            {
                if (m_bArrived[i] == false)
                {
                    isArrivedDest = false;

                    GameObject curCube = GameObject.Find("Cube" + i);
                    //centerPos = m_targetPos[i];
                    centerPos = curCube.transform.position;
                    dirVec = centerPos - transform.position;
                    dirVec.y = 0.0f;
                    if (dirVec.magnitude < 10.0f)
                    {
                        m_bArrived[i] = true;
                        break;
                    }

                    // 방향설정
                    Vector3 forwardVal = transform.forward;
                    forwardVal.y = 0.0f;
                    forwardVal.Normalize();
                    dirVec.Normalize();
                    dotVal = Vector3.Dot(forwardVal, dirVec);
                    degree = (float)Math.Acos(dotVal) * (180 / 3.141592f);
                    crossVal = Vector3.Cross(forwardVal, dirVec);

                    //목표지점과 각도가 10도이상 차이나면
                    if (10 < degree)
                    {
                        // 외적값에 따라 왼쪽인지 오른쪽인지 처리
                        if (0.0f < crossVal.y)
                        {
                            steer = 1 * Math.Max((degree / 180.0f), 0.9f);
                        }
                        else
                        {
                            steer = -1 * Math.Max((degree / 180.0f), 0.9f);
                        }

                        m_curSteer += Time.deltaTime*steer*30.0f;
                        if (70.0f < m_curSteer)
                            m_curSteer = 70.0f;
                        else if (m_curSteer < -70.0f)
                            m_curSteer = -70.0f;
                            
                    }

                    // 목표지점과 거의 직선 방향이 되었으면
                    if (0.97f < dotVal)
                    {
                        m_curSteer = 0.0f;
                        //if (m_curSteer < 0.0f)
                        //{
                        //    m_curSteer += Time.deltaTime*10.0f;
                        //}
                        //else
                        //{
                        //    m_curSteer -= Time.deltaTime*10.0f;
                        //}
                    }

                    break;
                }
            }

            if (isArrivedDest)
            {
                for (int i = 0; i < 6; ++i)
                    m_bArrived[i] = false;
            }
        }
        

        //steer = 0.0f;
        //if (isCurve == false && 20.0f < dirVec.magnitude)
        //{
        //    isCurve = true;
        //}

        //if (isCurve)
        //{
        //    steer = -1.0f;
        //    forwardVal.Normalize();
        //    dirVec.Normalize();
        //    dotVal = Vector3.Dot(forwardVal, dirVec);
        //    if (0.9f < dotVal)
        //    {
        //        steer = 0.0f;
        //        isCurve = false;
        //    }
        //}

        
        //if (130.0f < transform.position.z)
        //    steer = -1.0f;
        //else if (transform.position.z < 100.0f)
        //    steer = -1.0f;
        //steer = Input.GetAxis("Horizontal");
        

        //forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
        forward = 1.0f;
        //if (isArrivedDest)
        //    forward = 0.0f;
        //back = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
        back = 0.0f;

        m_rpm = FL_Wheel.rpm;
        if (400.0f < m_rpm)
        {
            m_rpm = 400.0f;
            motor = 0.0f;
        }

        FL_Wheel.motorTorque = max_torque * motor;
        FR_Wheel.motorTorque = max_torque * motor;
        //BL_Wheel.motorTorque = max_torque * motor;
        //BR_Wheel.motorTorque = max_torque * motor;

        BL_Wheel.brakeTorque = max_brake * brake;
        BR_Wheel.brakeTorque = max_brake * brake;

        // 커브 적용
        FL_Mesh.localEulerAngles = new Vector3(0, m_curSteer, 0);
        FR_Mesh.localEulerAngles = new Vector3(0, m_curSteer, 0);
        FL_Pos.localEulerAngles = new Vector3(0, m_curSteer, 0);
        FR_Pos.localEulerAngles = new Vector3(0, m_curSteer, 0);
        //FL_Mesh.localEulerAngles = new Vector3(0, steer * max_steer, 0);
        //FR_Mesh.localEulerAngles = new Vector3(0, steer * max_steer, 0);
        //FL_Pos.localEulerAngles = new Vector3(0, steer * max_steer, 0);
        //FR_Pos.localEulerAngles = new Vector3(0, steer * max_steer, 0);

        FL_Mesh.Rotate(m_rpm * Time.deltaTime, 0, 0);
        FR_Mesh.Rotate(m_rpm * Time.deltaTime, 0, 0);
        BL_Mesh.Rotate(m_rpm * Time.deltaTime, 0, 0);
        BR_Mesh.Rotate(m_rpm * Time.deltaTime, 0, 0);


        //if (current_speed <= 0.1f)
        //{
        //    if (back < 0)
        //        go_forward = false;
        //    if (forward > 0)
        //        go_forward = true;
        //}

        //if (go_forward == true)
        {
            motor = forward * 1.0f * (2.0f - degree/180.0f);
            brake = -back;

            //if (speed >= 610)
            //{
            //    //speed = 600;
            //    //motor = 0;
            //    //motor *= 0.2f;
            //}
            if (current_speed >= 0 && forward == 0 && back == 0)
            {
                brake = 2;
            }
        }
        //else if (go_forward == false)
        //{
        //    //motor = back;
        //    brake = 0.5f;

        //    if (speed >= 21)
        //    {
        //        speed = 20;
        //        motor -= 0.5f;
        //    }
        //    if (current_speed >= 0 && forward == 0 && back == 0)
        //    {
        //        brake = 1;
        //    }
        //}

        //speed = Mathf.Abs(0.5120276f * 20 * 3.14159f * FL_Wheel.rpm / 60);
        speed = Mathf.Abs(0.5120276f * 20 * 3.14159f * FL_Wheel.rpm * Time.deltaTime);
    }

    //void OnTriggerEnter(Collider obj)
    //{
    //    Debug.Log("collide!!!!\n");
    //    //Vector3 forwardVal = transform.forward;
    //    //forwardVal.x += 100.0f;
    //    //transform.forward = forwardVal;

    //    //centerPos = m_targetPos[i];
    //    Vector3 centerPos = obj.transform.position;
    //    Vector3 dirVec = centerPos - transform.position;
    //    dirVec.y = 0.0f;

    //    // 방향설정
    //    Vector3 forwardVal = transform.forward;
    //    forwardVal.y = 0.0f;
    //    forwardVal.Normalize();
    //    dirVec.Normalize();
    //    dotVal = Vector3.Dot(forwardVal, dirVec);
    //    degree = (float)Math.Acos(dotVal) * (180 / 3.141592f);
    //    crossVal = Vector3.Cross(forwardVal, dirVec);

    //    //steer = 1;
    //    //if (10 < degree)
    //    //{
    //        if (0.0f < crossVal.y)
    //        {
    //            //Vector3 tmpForwardVal = transform.forward;
    //            //tmpForwardVal.x -= 30.0f;
    //            //transform.forward = tmpForwardVal;
    //            transform.Rotate(0.0f, -30.0f, 0.0f);
    //            Debug.Log("collide ++++ \n");
    //        }
    //        else
    //        {
    //            //Vector3 tmpForwardVal = transform.forward;
    //            //tmpForwardVal.x += 30.0f;
    //            //transform.forward = tmpForwardVal;
    //            transform.Rotate(0.0f, 30.0f, 0.0f);
    //            Debug.Log("collide ---- \n");
    //        }
    //    //}
    //}

}
