using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    // Input Actions
    private InputAction lookAction;
    private InputAction camResetAction;
    private Vector2 lookInput;
    // Public variables
    public float rotationSpeed = 50f;
    public GameObject playerObject;
    public float orbitDistance = 5f;
    
    // Orbit angles
    private float yaw = 180f;
    private float pitch = 20f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        lookAction = GameManagerController.Instance.InputActions.FindActionMap("Player").FindAction("Look");
        lookAction.Enable();

        camResetAction = GameManagerController.Instance.InputActions.FindActionMap("Player").FindAction("CamReset");
        camResetAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        lookInput = lookAction.ReadValue<Vector2>();

        // Update yaw and pitch based on look input
        yaw += lookInput.x * rotationSpeed * Time.deltaTime;
        pitch -= lookInput.y * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        if (camResetAction.triggered)
        {
            yaw = 180f;
            pitch = 20f;
        }

        // Calculate camera position around player using Quaternion rotation
        Vector3 playerPos = playerObject.transform.position;
        Vector3 offset = new Vector3(0, 0, orbitDistance);

        // Apply yaw rotation (around Y axis)
        offset = Quaternion.Euler(0, yaw, 0) * offset;

        // Apply pitch rotation (pitch can be set elsewhere)
        offset = Quaternion.Euler(pitch, 0, 0) * offset;

        transform.position = playerPos + offset + new Vector3(0, 1f, 0);

        Vector3 gravityUp = -Physics.gravity.normalized;
        transform.LookAt(playerPos + gravityUp * 0.5f);
    }
}