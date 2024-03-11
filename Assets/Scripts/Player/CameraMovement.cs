using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player or a GameObject to follow
    public Vector3 areaSize = new Vector3(10f, 10f, 10f); // The size of the area where the camera can move
    public GameObject plane; 

    private Vector3 targetPosition;
    private bool isTopDown = false; // Flag to track camera mode
    private float clampedY = 10;
    private float areaSizeZ;
    bool active = false;

    void start(){

    }
    void Update()
    {
        if (active){
            if (Input.GetKeyDown(KeyCode.Space))
        {
            // Toggle between top-down and default camera modes
            isTopDown = !isTopDown;

            // Adjust camera and player positions accordingly
            if (isTopDown)
            {
                AdjustCameraToTopDown();
            }
            else
            {
                AdjustCamera();
            }
        }
       
        if (player != null)
            {
                // Get the position of the player or the target GameObject
                targetPosition = player.position;

                // Clamp the camera's position within the specified area
                float clampedX = Mathf.Clamp(targetPosition.x, -areaSize.x / 2f, areaSize.x / 2f);
                float clampedZ = Mathf.Clamp(targetPosition.z, -areaSizeZ , areaSize.z / 2f );


                // Set the new position of the CameraController within the clamped range
                transform.position = new Vector3(clampedX, clampedY, clampedZ);
            }
        }
    
    }
 public void AdjustCamera()
    {
        // Example: Adjust camera position based on combat area size
        if (plane != null)
        {
            // Get the size of the plane in the X and Z axes
            float planeSizeX = plane.GetComponent<Renderer>().bounds.size.x;
            float planeSizeZ = plane.GetComponent<Renderer>().bounds.size.z;
            clampedY = 10;
            areaSize.x =10f;
            areaSize.z =10f;
            areaSizeZ = areaSize.z;

            // Example: Adjust camera position based on plane size
            transform.position = new Vector3(planeSizeX / 8f,  10 , -10);
            player.transform.position = new Vector3(planeSizeX / 8f,  10 , -10);
            targetPosition = player.position;
            active = true;
            Camera.main.transform.rotation = Quaternion.Euler(45f, 0f, 0f);

        }
        else
        {
            Debug.LogError("Plane object not assigned in the Inspector.");
        }
    } 

       void AdjustCameraToTopDown()
    {
        if (plane != null)
        {
            // Get the size of the plane in the X and Z axes
          


            // Move the camera area to the center of the plane
            float planeSizeX = plane.GetComponent<Renderer>().bounds.size.x;
            float planeSizeZ = plane.GetComponent<Renderer>().bounds.size.z;
            clampedY = 15;
            areaSize.x = 15f;
            areaSize.z = 15f;
            areaSizeZ = areaSize.z / 2f;


            player.transform.position = new Vector3(planeSizeX / 8f,  10 ,planeSizeZ/ 8f);

            targetPosition = player.position;
           

            
            Camera.main.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else
        {
            Debug.LogError("Plane object not assigned in the Inspector.");
        }
    }

    

}
