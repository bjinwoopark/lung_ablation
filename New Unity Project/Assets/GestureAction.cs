using Academy.HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// GestureAction performs custom actions based on
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour
{
    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 1.0f;

    private Vector3 manipulationPreviousPosition;

    private float rotationFactorx;
    private float rotationFactory;
    private float rotationFactorz;

    void Update()
    {
        PerformRotation();
    }

    private void PerformRotation()
    {
        if (GestureManager.Instance.IsNavigating)
        {
            // Calculate rotationFactor based on GestureManager's NavigationPosition.X and multiply by RotationSensitivity.
            // This will help control the amount of rotation.
            rotationFactorx = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;
            rotationFactory = GestureManager.Instance.NavigationPosition.y * RotationSensitivity;
            rotationFactorz = GestureManager.Instance.NavigationPosition.z * RotationSensitivity;

            // transform.Rotate along the Y axis using rotationFactor.
            Debug.Log("Rotating...");
            transform.Rotate(new Vector3(-1 * rotationFactorz, -1 * rotationFactorx, -1 * rotationFactory));
        }
    }

    void PerformManipulationStart(Vector3 position)
    {
        manipulationPreviousPosition = position;
    }

    void PerformManipulationUpdate(Vector3 position)
    {
        if (GestureManager.Instance.IsManipulating)
        {

            Vector3 moveVector = Vector3.zero;

            // Calculate the moveVector as position - manipulationPreviousPosition.
            moveVector = position - manipulationPreviousPosition;

            // Update the manipulationPreviousPosition with the current position.
            manipulationPreviousPosition = position;

            // Increment this transform's position by the moveVector.
            transform.position += moveVector;
        }
    }
}