using UnityEngine;

/// <summary>
/// Makes objects float/hover with smooth animation
/// Great for magical ingredients
/// </summary>
public class FloatingAnimation : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 0.1f;  // How high it floats
    public float floatSpeed = 1f;        // How fast it floats
    public bool enableRotation = true;
    public float rotationSpeed = 30f;

    private Vector3 startPosition;
    private float randomOffset;

    private void Start()
    {
        startPosition = transform.position;
        
        // Random offset so not all objects float in sync
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        // Floating motion
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed + randomOffset) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Gentle rotation
        if (enableRotation)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Update start position (call after moving object)
    /// </summary>
    public void UpdateStartPosition()
    {
        startPosition = transform.position;
    }

    /// <summary>
    /// Stop floating animation
    /// </summary>
    public void StopFloating()
    {
        enabled = false;
    }

    /// <summary>
    /// Resume floating animation
    /// </summary>
    public void ResumeFloating()
    {
        startPosition = transform.position;
        enabled = true;
    }
}
