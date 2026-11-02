using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 7f;
    [SerializeField] private float lookHeight = 1.2f;
    [SerializeField] private float mouseSensitivity = 180f;
    [SerializeField] private float minPitch = -15f;
    [SerializeField] private float maxPitch = 65f;
    [SerializeField] private float followSharpness = 8f;

    private float yaw;
    private float pitch = 20f;

    private void Start()
    {
        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Escape releases the cursor so the user can interact with editor/UI; click to recapture.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion orbit = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focus = target.position + Vector3.up * lookHeight;
        Vector3 desiredPosition = focus - orbit * Vector3.forward * distance;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSharpness * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(focus - transform.position, Vector3.up);
    }
}
