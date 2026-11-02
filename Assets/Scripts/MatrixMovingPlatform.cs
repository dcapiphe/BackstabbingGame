using UnityEngine;

/// <summary>Moves a platform by applying a translation matrix to its local start point.</summary>
public class MatrixMovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 localOffset = new Vector3(0f, 0f, 5f);
    [SerializeField] private float cyclesPerSecond = 0.15f;
    [SerializeField] private bool rotateAsWell;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float phase;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        phase += Time.deltaTime * cyclesPerSecond * Mathf.PI * 2f;
        float t = (Mathf.Sin(phase) + 1f) * 0.5f;
        Vector3 displacement = localOffset * t;
        Matrix4x4 movement = Matrix4x4.TRS(displacement, Quaternion.identity, Vector3.one);
        transform.position = startPosition + movement.MultiplyPoint3x4(Vector3.zero);
        if (rotateAsWell)
            transform.rotation = startRotation * Quaternion.Euler(0f, Mathf.Sin(phase) * 25f, 0f);
    }
}
