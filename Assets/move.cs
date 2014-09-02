using UnityEngine;
using System.Collections;

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

    float motor = 0;
    float brake = 0;

    bool go_forward = true;

    float forward = 0;
    float back = 0;

    float current_speed = 0;
    float steer = 0;

    public float speed = 0;


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
    }

    void FixedUpdate()
    {
        current_speed = rigidbody.velocity.sqrMagnitude;

        steer = 0.0f;
        if (130.0f < transform.position.z)
            steer = -1.0f;
        else if (transform.position.z < 100.0f)
            steer = -1.0f;
        //steer = Input.GetAxis("Horizontal");
        

        //forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
        forward = 1.0f;
        back = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);

        float rpm = FL_Wheel.rpm * 50;

        FL_Wheel.motorTorque = max_torque * motor;
        FR_Wheel.motorTorque = max_torque * motor;

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
            motor = forward*10.0f;
            brake = -back;

            if (speed >= 610)
            {
                speed = 600;
                //motor = 0;
            }
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

        speed = Mathf.Abs(0.5120276f * 20 * 3.14159f * FL_Wheel.rpm / 60);
    }

}
