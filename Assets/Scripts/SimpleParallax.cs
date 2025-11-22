using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField][Range(0f, 1f)] private float parallaxSpeed = 0.5f;

    [Header("Axis Control")]
    [SerializeField] private bool parallaxX = true;
    [SerializeField] private bool parallaxY = false;

    private Vector3 startPosition;

    void Start()
    {
        // Find camera if not assigned
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        startPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = transform.position;

        if (parallaxX)
        {
            float distanceX = cameraTransform.position.x * parallaxSpeed;
            newPosition.x = startPosition.x + distanceX;
        }

        if (parallaxY)
        {
            float distanceY = cameraTransform.position.y * parallaxSpeed;
            newPosition.y = startPosition.y + distanceY;
        }

        transform.position = newPosition;
    }
}