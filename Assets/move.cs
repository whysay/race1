using UnityEngine;
using System.Collections;
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

    public float max_steer = 20;
    public float max_torque = 200;
    public float max_brake = 10;

    public float motor = 0;
    float brake = 0;

    bool go_forward = true;

    float forward = 0;
    float back = 0;

    public float degree = 0;
    public Vector3 crossVal = new Vector3();
    public float current_speed = 0;
    public float steer = 0;

    public float speed = 0;
    public float dotVal = 0.0f;

    Vector3 [] m_targetPos = new Vector3[6];
    public bool[] m_bArrived = { false, false, false, false, false, false };


    void Start()
    {
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

    bool isCurve = false;
    void FixedUpdate()
    {
        current_speed = rigidbody.velocity.sqrMagnitude;

        Vector3 centerPos = new Vector3(120.0f, 0.0f, 120.0f);
        Vector3 dirVec = centerPos - transform.position;

        steer = 0.0f;
        bool isArrivedDest = true;
        for (int i = 0; i < 6; ++i )
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

                //steer = 1;
                if (30 < degree)
                {
                    if (0.0f < crossVal.y)
                        steer = 1 * Math.Max((degree / 180.0f), 0.5f);
                    else
                        steer = -1 * Math.Max((degree / 180.0f), 0.5f);
                }

                //if (crossVal.y < -0.2f)
                    
                

                //if (dotVal < -0.5f)
                //{
                //    steer = -1;
                //    //steer = (1+dotVal)* (-1.0f);
                //}
                //else
                //{
                //    steer = 1;
                //    //steer = (1-dotVal);
                //}
                if (0.95f < dotVal)
                {
                    steer = 0.0f;
                    //isCurve = false;
                }

                break;
            }
        }

        if (isArrivedDest)
        {
            for (int i = 0; i < 6; ++i)
                m_bArrived[i] = false;
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
        if (isArrivedDest)
            forward = 0.0f;
        //back = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
        back = 0.0f;

        float rpm = FL_Wheel.rpm * 10;

        FL_Wheel.motorTorque = max_torque * motor;
        FR_Wheel.motorTorque = max_torque * motor;
        //BL_Wheel.motorTorque = max_torque * motor;
        //BR_Wheel.motorTorque = max_torque * motor;

        BL_Wheel.brakeTorque = max_brake * brake;
        BR_Wheel.brakeTorque = max_brake * brake;


        FL_Mesh.localEulerAngles = new Vector3(0, steer * max_steer, 0);
        FR_Mesh.localEulerAngles = new Vector3(0, steer * max_steer, 0);
        FL_Pos.localEulerAngles = new Vector3(0, steer * max_steer, 0);
        FR_Pos.localEulerAngles = new Vector3(0, steer * max_steer, 0);

        FL_Mesh.Rotate(rpm * Time.deltaTime, 0, 0);
        FR_Mesh.Rotate(rpm * Time.deltaTime, 0, 0);
        BL_Mesh.Rotate(rpm * Time.deltaTime, 0, 0);
        BR_Mesh.Rotate(rpm * Time.deltaTime, 0, 0);


        if (current_speed <= 0.1f)
        {
            if (back < 0)
                go_forward = false;
            if (forward > 0)
                go_forward = true;
        }

        if (go_forward == true)
        {
            motor = forward*2.0f;
            brake = -back;

            //if (speed >= 610)
            //{
            //    speed = 600;
            //    //motor = 0;
            //}
            if (current_speed >= 0 && forward == 0 && back == 0)
            {
                brake = 2;
            }
        }
        else if (go_forward == false)
        {
            //motor = back;
            brake = 0.5f;

            if (speed >= 21)
            {
                speed = 20;
                motor -= 0.5f;
            }
            if (current_speed >= 0 && forward == 0 && back == 0)
            {
                brake = 1;
            }
        }

        //speed = Mathf.Abs(0.5120276f * 20 * 3.14159f * FL_Wheel.rpm / 60);
        speed = Mathf.Abs(0.5120276f * 20 * 3.14159f * FL_Wheel.rpm * Time.deltaTime);
    }

}
