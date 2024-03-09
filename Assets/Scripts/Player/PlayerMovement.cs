using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement
    public float zoomSpeed = 15f; // Speed of the zoom
    public float smoothZoomTime = 0.1f; // Smoothing time for zooming

    private float targetFieldOfView;


  void Start()
    {
        // Assuming the camera is a child of the GameObject
        Camera mainCamera = GetComponentInChildren<Camera>();

        if (mainCamera != null)
        {
            // Initialize targetFieldOfView with the initial field of view value
            targetFieldOfView = mainCamera.fieldOfView;
        }
        else
        {
            Debug.LogError("Main camera not found!");
        }
    }
    void Update()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Move the GameObject based on input
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Get input for zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the target field of view based on scroll input
        targetFieldOfView -= scrollInput * zoomSpeed;

        // Clamp the target field of view to prevent extreme zoom values
        targetFieldOfView = Mathf.Clamp(targetFieldOfView, 10f, 60f);

        // Smoothly interpolate between the current and target field of view
        SmoothZoom();
    }

    void SmoothZoom()
    {
        // Assuming the camera is a child of the GameObject
        Camera mainCamera = GetComponentInChildren<Camera>();

        if (mainCamera != null)
        {
            // Use Mathf.Lerp to smoothly interpolate between the current and target field of view
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFieldOfView, smoothZoomTime);
        }
    }
   
}
