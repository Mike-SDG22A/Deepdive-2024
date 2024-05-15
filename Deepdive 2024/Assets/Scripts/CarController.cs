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
    public float rmpCounter = 0;
    public int gear = 1;
    [SerializeField] float downForce = 0.05f;
    bool canDrive = true;
    float slip;

    PlayerInput input;
    Rigidbody rb;

    public float GetMaxSpeed() => speed[gear];
    public float GetCurrentSpeed() => currentSpeed;

    void Start()
    {
        slip = backWheels[0].sidewaysFriction.stiffness;
        currentMaxSpeed = speed[gear];
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.down ;
        input = FindObjectOfType<PlayerInput>();
        if (!forwardDrive && !backDrive) backDrive = true;
    }

    void Update()
    {
        Shift();
        if (canDrive) Movement();
        Brake();

        if (rb.velocity.sqrMagnitude < 10)
        {
            currentMaxSpeed = speed[1];
            rmpCounter = 1000;
        }

        Turn();
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.down * downForce * rb.velocity.sqrMagnitude, ForceMode.Force);
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

        currentSpeed += ((Mathf.Abs(input.actions["Vertical"].ReadValue<float>()) * currentMaxSpeed) - currentSpeed) * Mathf.Clamp(1 - Mathf.Pow(2.5f, gear > 0 ? gear : 1) / 10 + (2 * currentSpeed / 10000), 0.05f, 1) * Time.deltaTime;

        rmpCounter = Mathf.Lerp(rmpCounter, (Mathf.Abs(input.actions["Vertical"].ReadValue<float>()) * 2 - 1) != -1 ? 10000 : 1000, Time.deltaTime);

        foreach (var wheel in frontWheels)
        {
            
            WheelFrictionCurve side = wheel.sidewaysFriction;
            side.stiffness = Mathf.Clamp(slip - currentSpeed / 1500, 1, slip);
            wheel.sidewaysFriction = side;

        }
        foreach (var wheel in backWheels)
        {
            WheelFrictionCurve side = wheel.sidewaysFriction;
            side.stiffness = Mathf.Clamp(slip - currentSpeed / 1500, 1, slip);
            wheel.sidewaysFriction = side;
        }

        if (currentSpeed * 10 < 10000 * gear && gear > 1) currentSpeed -= ((Mathf.Abs(input.actions["Vertical"].ReadValue<float>()) * currentMaxSpeed) - currentSpeed) * (1 / gear) * Time.deltaTime;

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

        if (input.actions["Brake"].IsPressed())
        {
            rmpCounter = Mathf.Lerp(rmpCounter, 1000, 10 * Time.deltaTime);
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime);
        }

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
                //StopCoroutine(Switch());
            }

            gear += (int)dir;

            gear = Mathf.Clamp(gear, 0, speed.Length - 1);

            if (dir > 0 && gear < speed.Length)
            {
                currentSpeed *= (Mathf.Abs(rmpCounter / 10000));
                //StopCoroutine(Switch());
                //StartCoroutine(Switch());
                currentMaxSpeed = speed[gear];
                rmpCounter = 1000;
            }
            if (dir < 0)
            {
                rmpCounter += 10000;
                if (rmpCounter > 13000) canDrive = false;
                currentMaxSpeed = speed[gear];
            }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gear = 0;
            rmpCounter += 10000;
            if (rmpCounter > 13000) canDrive = false;
            //StopAllCoroutines();
        }
        else if (Input.GetKeyDown(KeyCode.W) && gear == 0)
        {
            gear = 1;
            currentSpeed *= (Mathf.Abs(rmpCounter / 10000));
            rmpCounter = 1000;
            //StopCoroutine(Switch());
        }
    }


    IEnumerator Switch()
    {
        currentSpeed = speed[gear] * (Mathf.Abs(rmpCounter / 10000));

        while (currentMaxSpeed < speed[gear]) 
         {
            currentMaxSpeed = currentMaxSpeed + (speed[gear] - currentMaxSpeed) * Time.deltaTime;


            if (currentMaxSpeed + speed[gear] / 10 >= speed[gear]) currentMaxSpeed = speed[gear];
            yield return null;
         }
    }
}