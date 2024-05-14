using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] bool forwardDrive = false;
    [SerializeField] bool backDrive = false;
    [SerializeField] WheelCollider[] frontWheels;
    [SerializeField] WheelCollider[] backWheels;
    [SerializeField] float[] speed = new float[] { -600, 1200, 2400, 4800 };
    [SerializeField] float currentMaxSpeed;
    [SerializeField] float turnAngle = 60;
    [SerializeField] float brakeStrenght = 2000;
    [SerializeField] float currentSpeed;
    [SerializeField] float currentTurnAngle;
    [SerializeField] int gear = 1;

    PlayerInput input;
    Rigidbody rb;

    void Start()
    {
        currentMaxSpeed = speed[gear];
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.down * 2;
        input = FindObjectOfType<PlayerInput>();
        if (!forwardDrive && !backDrive) backDrive = true;
    }

    void Update()
    {
        Shift();
        Brake();
        Movement();
        Turn();
    }

    void Turn()
    {
        float angle = input.actions["Horizontal"].ReadValue<float>();


        currentTurnAngle = turnAngle * angle;


        foreach (var wheel in frontWheels)
        {
            wheel.steerAngle = currentTurnAngle;
        }
    }

    void Movement()
    {
        currentSpeed = Mathf.Abs(input.actions["Vertical"].ReadValue<float>()) * currentMaxSpeed;

        if (forwardDrive)
        {
            foreach (var wheel in frontWheels)
            {
                wheel.motorTorque = currentSpeed;
            }
        }
        if (backDrive)
        {
            foreach (var wheel in backWheels)
            {
                wheel.motorTorque = currentSpeed;
            }
        }
    }

    void Brake()
    {
        float brakePress = input.actions["Brake"].ReadValue<float>();
        foreach (var wheel in frontWheels)
        {
            wheel.brakeTorque = brakeStrenght * brakePress;
        }
        foreach (var wheel in backWheels)
        {
            wheel.brakeTorque = brakeStrenght * brakePress;
        }
    }

    void Shift()
    {
        float dir = input.actions["Switch"].ReadValue<float>();

        if (input.actions["Switch"].WasPerformedThisFrame())
        {
            if (dir > 0)
            {
                dir = 1;
            }
            if (dir < 0)
            {
                dir = -1;
                StopCoroutine(Switch());
            }

            if (dir > 0 && speed[gear] != currentMaxSpeed) return;

            gear += (int)dir;

            gear = Mathf.Clamp(gear, 0, speed.Length - 1);

            if (dir > 0 && gear < speed.Length)
            {
                StopCoroutine(Switch());
                StartCoroutine(Switch());
            }
            if (dir < 0) currentMaxSpeed = speed[gear];

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gear = 0;
            StopAllCoroutines();
        }
        else if (Input.GetKeyDown(KeyCode.W) && gear == 0)
        {
            gear = 1;
            StopCoroutine(Switch());
        }
    }

    IEnumerator Switch()
    {
        currentMaxSpeed = speed[gear] / 5;

        while (currentMaxSpeed < speed[gear]) 
         {
            currentMaxSpeed = currentMaxSpeed + (speed[gear] - currentMaxSpeed) * Time.deltaTime;


            if (currentMaxSpeed + speed[gear] / 10 >= speed[gear]) currentMaxSpeed = speed[gear];
            yield return null;
         }
    }
}