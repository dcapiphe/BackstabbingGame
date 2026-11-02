using TMPro;
using UnityEngine;

/// <summary>WASD platform movement with hand-written gravity and a grounded jump check.</summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpHeight = 2.2f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform bodyToRotate;
    [SerializeField] private float turnSharpness = 14f;
    [SerializeField] private TMP_Text positionText;

    private CharacterController controller;
    private float verticalSpeed;
    private bool wasGrounded;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Quaternion bodyFacingOffset = Quaternion.identity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        // The player's placed position in each scene is that level's restart point.
        startPosition = transform.position;
        startRotation = transform.rotation;

        Transform facingTransform = bodyToRotate != null ? bodyToRotate : transform;
        if (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            if (cameraForward.sqrMagnitude > 0.001f)
                bodyFacingOffset = Quaternion.Inverse(Quaternion.LookRotation(cameraForward.normalized, Vector3.up))
                    * facingTransform.rotation;
        }
    }

    public void RespawnAtLevelStart()
    {
        controller.enabled = false;
        transform.SetPositionAndRotation(startPosition, startRotation);
        verticalSpeed = 0f;
        wasGrounded = false;
        controller.enabled = true;
    }

    private void Update()
    {
        bool grounded = controller.isGrounded;
        if (grounded && verticalSpeed < 0f) verticalSpeed = -2f;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 forward = cameraTransform != null ? cameraTransform.forward : Vector3.forward;
        Vector3 right = cameraTransform != null ? cameraTransform.right : Vector3.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();
        Vector3 horizontal = Vector3.ClampMagnitude(right * x + forward * z, 1f) * moveSpeed;

        // Turn the visible rig toward the camera's horizontal look direction.
        // The rig's hand bones and anything parented to them (such as a dagger) follow this rotation.
        Transform facingTransform = bodyToRotate != null ? bodyToRotate : transform;
        if (cameraTransform != null && forward.sqrMagnitude > 0.001f)
        {
            Quaternion desiredFacing = Quaternion.LookRotation(forward, Vector3.up) * bodyFacingOffset;
            facingTransform.rotation = Quaternion.Slerp(
                facingTransform.rotation,
                desiredFacing,
                turnSharpness * Time.deltaTime);
        }

        if (grounded && Input.GetKeyDown(KeyCode.Space))
            verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
        verticalSpeed += gravity * Time.deltaTime;

        controller.Move((horizontal + Vector3.up * verticalSpeed) * Time.deltaTime);
        grounded = controller.isGrounded;
        if (grounded && !wasGrounded) verticalSpeed = -2f;
        wasGrounded = grounded;

        if (positionText != null)
        {
            Vector3 p = transform.position;
            positionText.text = $"Position: ({p.x:0.0}, {p.y:0.0}, {p.z:0.0})";
        }
    }
}
