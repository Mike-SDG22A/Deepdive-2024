using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CarController : MonoBehaviour
{
    [SerializeField] bool forwardDrive = false;
    [SerializeField] bool backDrive = false;
    [SerializeField] WheelCollider[] frontWheels;
    [SerializeField] WheelCollider[] backWheels;
    [SerializeField] Transform[] frontWheelsObj;
    [SerializeField] Transform[] backWheelsObj;
    [SerializeField] float[] speed = new float[] { -600, 1200, 2400, 4800 };
    [SerializeField] float currentMaxSpeed;
    [SerializeField] float turnAngle = 60;
    [SerializeField] float brakeStrenght = 2000;
    [SerializeField] float currentSpeed;
    [SerializeField] float currentTurnAngle;
    [SerializeField] float fDriftSlip = 0.8f;
    [SerializeField] float sDriftSlip = 0.022f;
    public float rmpCounter = 0;
    public int gear = 1;
    [SerializeField] float downForce = 0.05f;
    bool canDrive = true;
    bool isDrifting = false;
    float slip;
    float forwardSlip;
    WheelFrictionCurve sideFriction;
    WheelFrictionCurve forwardFriction;
    float[] frontWheelY = new float[2];
    LapScript lapScript;

    public float kmp;

    [SerializeField] TrailRenderer[] backTrail;

    PlayerInput input;
    Rigidbody rb;

    void Start()
    {
        lapScript = GetComponent<LapScript>(); 

        for (int i = 0; i < frontWheelY.Length; i++)
        {
            frontWheelY[i] = frontWheelsObj[i].eulerAngles.y;
        }

        sideFriction = backWheels[0].sidewaysFriction;
        forwardFriction = backWheels[0].forwardFriction;
        forwardSlip = backWheels[0].forwardFriction.stiffness;
        slip = backWheels[0].sidewaysFriction.stiffness;

        currentMaxSpeed = speed[gear];

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.down ;

        input = FindObjectOfType<PlayerInput>();

        if (!forwardDrive && !backDrive) backDrive = true;
    }

    void Update()
    {
        kmp = currentSpeed / 75 * 3.6f;
        if (input.actions["Reset"].WasPressedThisFrame())
        {
            StartCoroutine(ResetCar());
        }
        else
        {
            Shift();
            if (canDrive) Movement();
            else
            {
                currentSpeed = 0;

                foreach (var wheel in frontWheels)
                {
                    wheel.motorTorque = 0;
                }
                foreach (var wheel in backWheels)
                {
                    wheel.motorTorque = 0;
                }
            }
            Brake();
            Drift();

            if (!input.actions["Drift"].IsPressed() && !input.actions["Brake"].IsPressed())
            {
                foreach (var trail in backTrail)
                {
                    trail.emitting = false;
                }
            }

            if (rb.velocity.sqrMagnitude < 10 && gear != 0)
            {
                gear = 1;
                currentMaxSpeed = speed[gear];
                rmpCounter = 1000;
            }

            Turn();

            SetWheelPostion();
        }
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.down * downForce * rb.velocity.sqrMagnitude * 0.3f, ForceMode.Force);
        rb.AddForce(Vector3.down * downForce * rb.velocity.sqrMagnitude * 0.3f, ForceMode.Force);
    }

    void SetWheelPostion()
    {
        for (int i = 0; i < frontWheels.Length; i++) 
        {
            frontWheels[i].GetWorldPose(out Vector3 fPos, out Quaternion fRot);
            backWheels[i].GetWorldPose(out Vector3 bPos, out Quaternion bRot);
            
            frontWheelsObj[i].position = fPos;
            backWheelsObj[i].position = bPos;

            frontWheelsObj[i].rotation = fRot;
            backWheelsObj[i].rotation = bRot;
        }
    }

    void Turn()
    {
        float angle = input.actions["Horizontal"].ReadValue<float>();

        float turn = turnAngle;

        if (isDrifting) turn = 60;

        currentTurnAngle = turn * angle;


        foreach (var wheel in frontWheels)
        {
            wheel.steerAngle = currentTurnAngle;
        }
    }

    void Movement()
    {

        currentSpeed += ((Mathf.Abs(input.actions["Vertical"].ReadValue<float>()) * currentMaxSpeed) - currentSpeed) * Mathf.Clamp(1 - Mathf.Pow(2.5f, gear > 0 ? gear : 1) / 10 + (2 * currentSpeed / 10000), 0.05f, 1) * Time.deltaTime;

        rmpCounter = Mathf.Lerp(rmpCounter, (Mathf.Abs(input.actions["Vertical"].ReadValue<float>()) * 2 - 1) != -1 ? 10000 : 1000, Time.deltaTime);

        if (!isDrifting)
        {
            foreach (var wheel in frontWheels)
            {
                sideFriction.stiffness = Mathf.Clamp(slip - currentSpeed / 1500, 1, slip);
                wheel.sidewaysFriction = sideFriction;
                wheel.forwardFriction = forwardFriction;
            }
            foreach (var wheel in backWheels)
            {
                sideFriction.stiffness = Mathf.Clamp(slip - currentSpeed / 1500, 1, slip);
                wheel.sidewaysFriction = sideFriction;
                wheel.forwardFriction = forwardFriction;
            }
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

    void Drift()
    {
        if (input.actions["Drift"].IsPressed())
        {
            foreach(var trail in backTrail)
            {
                trail.emitting = true;
            }

            float horizontal = input.actions["Horizontal"].ReadValue<float>();

            rb.AddForce((transform.right * -horizontal + transform.forward * 0.2f + rb.velocity.normalized * 0.2f) * currentSpeed, ForceMode.Force);


            float tempBrake = 0;
            float tempBrakeBack = currentSpeed / 2f;
            isDrifting = true;

            foreach (var wheel in frontWheels)
            {
                wheel.sidewaysFriction = CreateFrictionCurve(0.8f, 1, 2, 0.5f, sDriftSlip);
                wheel.forwardFriction = CreateFrictionCurve(0.4f, 1, 1, 0.5f, fDriftSlip);
            }
            foreach (var wheel in backWheels)
            {
                wheel.sidewaysFriction = CreateFrictionCurve(0.8f, 0.5f, 0.545f, 0.5f, sDriftSlip);
                wheel.forwardFriction = CreateFrictionCurve(0.4f, 1, 1, 0.5f, fDriftSlip);
            }

            foreach (var wheel in frontWheels)
            {
                wheel.brakeTorque = tempBrake;
            }
            foreach (var wheel in backWheels)
            {
                wheel.brakeTorque = tempBrakeBack;
            }
        }
        else
        {
            foreach (var wheel in backWheels)
            {
                wheel.brakeTorque = 0;
            }
            isDrifting = false;
        }
    }

    void Brake()
    {
        float brakePress = input.actions["Brake"].ReadValue<float>();

        if (input.actions["Brake"].IsPressed())
        {
            rmpCounter = Mathf.Lerp(rmpCounter, 1000, 10 * Time.deltaTime);
            currentSpeed = Mathf.Lerp(currentSpeed, 0,  Time.deltaTime);

            foreach (var trail in backTrail)
            {
                trail.emitting = true;
            }
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
                if (gear == 3) return;
            }
            if (dir < 0)
            {
                dir = -1;
                if (gear == 0) return;
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


    WheelFrictionCurve CreateFrictionCurve(float extremumSlip, float extremumValue, float asymptoteSlip, float asymptoteValue, float stiffness)
     {
        WheelFrictionCurve curve = new WheelFrictionCurve();

        curve.extremumSlip = extremumSlip;
        curve.extremumValue = extremumValue;
        curve.asymptoteSlip = asymptoteSlip;
        curve.asymptoteValue= asymptoteValue;
        curve.stiffness = stiffness;


        return curve;
     }

    IEnumerator ResetCar()
    {
        float time = 0;

        rb.isKinematic = true;

        while (time < 1) 
         {
            print(transform.position == lapScript.allCheckpoints[lapScript.checkPointCount + 1].transform.position + Vector3.up * 10);
            if (transform.position == lapScript.allCheckpoints[lapScript.checkPointCount + 1].transform.position + Vector3.up * 10) time += Time.deltaTime;

            rb.velocity = Vector3.zero;
            transform.position = lapScript.allCheckpoints[lapScript.checkPointCount + 1].transform.position + Vector3.up * 10;
            transform.rotation = Quaternion.LookRotation(lapScript.allCheckpoints[lapScript.checkPointCount].transform.position - transform.position);
            
            
            yield return null;
        }

        canDrive = true;
        rb.isKinematic = false;

    }
}