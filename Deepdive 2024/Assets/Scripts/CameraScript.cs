using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Vector3 Offset; // camera's offset relative to the car.
    [SerializeField] private Transform Target; // where the camera should look.
    [SerializeField] private float TranlateSpeed; // how fast the camera should follow the car.
    [SerializeField] private float RotationSpeed; // how fast the camera should turn with the car.
    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }

    #region handlers

    /// <summary>
    /// makes camera move along with car.
    /// </summary>
    private void HandleTranslation()
    {
        var TargetPosition = Target.TransformPoint(Offset);
        transform.position = Vector3.Lerp(transform.position, TargetPosition, TranlateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// makes camera offset it's rotation when the car turns.
    /// </summary>
    private void HandleRotation()
    {
        var direction = Target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
    }

    #endregion end handlers
}
