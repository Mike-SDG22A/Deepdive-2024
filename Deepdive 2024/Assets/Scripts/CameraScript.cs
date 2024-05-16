using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Vector3 Offset; // camera's offset relative to the car.
    [SerializeField] private Transform Target; // where the camera should look.
    [SerializeField] private float TranlateSpeed; // how fast the camera should follow the car.
    [SerializeField] private float RotationSpeed; // how fast the camera should turn with the car.
    [SerializeField] GameObject firstPersonCamera;
    [SerializeField] GameObject thirdPersonCamera;

    PlayerInput input;

    private void Start()
    {
        input = FindObjectOfType<PlayerInput>();
    }
    private void Update()
    {

        HandleTranslation();
        HandleRotation();

        SwitchCamera();
    }

    #region first/third person methods

    void SwitchCamera()
    {
        if (input.actions["CameraSwitching"].WasPerformedThisFrame())
        {
            print(!firstPersonCamera.active);
            firstPersonCamera.active = !firstPersonCamera.active;
            thirdPersonCamera.active = !thirdPersonCamera.active;
        }
    }

    #endregion end methods

    #region handlers

    /// <summary>
    /// makes camera move along with car.
    /// </summary>
    private void HandleTranslation()
    {
        var TargetPosition = Target.TransformPoint(Offset);
        thirdPersonCamera.transform.position = Vector3.Lerp(thirdPersonCamera.transform.position, TargetPosition, TranlateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// makes camera offset it's rotation when the car turns.
    /// </summary>
    private void HandleRotation()
    {
        var direction = Target.position - thirdPersonCamera.transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        thirdPersonCamera.transform.rotation = Quaternion.Lerp(thirdPersonCamera.transform.rotation, rotation, RotationSpeed * Time.deltaTime);
    }

    #endregion end handlers
}
