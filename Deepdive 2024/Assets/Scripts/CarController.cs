using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CarController : MonoBehaviour
{
    [SerializeField] bool forwardDrive = false;
    [SerializeField] bool backDrive = false;
    [SerializeField] WheelCollider[] frontWheels;
    [SerializeField] WheelCollider[] backWheels;
    [SerializeField] float speed = 30;
    [SerializeField] float turnAngle = 60;
    [SerializeField] float currentSpeed;
    [SerializeField] float currentTurnAngle;
    float turnTimer = 0;
    PlayerInput input;

    void Start()
    {
        input = FindObjectOfType<PlayerInput>();
        if (!forwardDrive && !backDrive) backDrive = true;       
    }

    void Update()
    {
        Movement();
        Turn();
    }

    void Turn()
    {
        float angle = input.actions["Horizontal"].ReadValue<float>();

        if (angle != 0)
        {
            turnTimer += Time.deltaTime;

            turnTimer = Mathf.Clamp(turnTimer, 0, 1);

            currentTurnAngle = turnAngle  * 10 * angle * turnTimer;
        }
        else
        {
            turnTimer = 0;
            if (currentTurnAngle > 0)
            {
                currentTurnAngle -= turnAngle  * 10 * Time.deltaTime;
            }
            else if (currentTurnAngle < 0)
            {
                currentTurnAngle += turnAngle * 10 * Time.deltaTime;
            }

            if (currentTurnAngle > -5 && currentTurnAngle < 5) currentTurnAngle = 0;
        }

        currentTurnAngle = Mathf.Clamp(currentTurnAngle, -turnAngle, turnAngle);

        foreach (var wheel in frontWheels)
        {
            wheel.steerAngle = currentTurnAngle;
        }
    }

    void Movement()
    {
        currentSpeed = input.actions["Vertical"].ReadValue<float>() * speed * Time.deltaTime;

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
}
