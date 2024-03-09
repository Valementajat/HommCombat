using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player or a GameObject to follow
    public Vector3 areaSize = new Vector3(10f, 10f, 10f); // The size of the area where the camera can move
    public GameObject plane; 

    private Vector3 targetPosition;

    bool active = false;
    void Update()
    {
        if (active){

       
        if (player != null)
            {
                // Get the position of the player or the target GameObject
                targetPosition = player.position;

                // Clamp the camera's position within the specified area
                float clampedX = Mathf.Clamp(targetPosition.x, -areaSize.x / 2f, areaSize.x / 2f);
                float clampedY = 10;
                float clampedZ = Mathf.Clamp(targetPosition.z, -areaSize.z , areaSize.z / 2f);


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
          

            // Example: Adjust camera position based on plane size
            transform.position = new Vector3(planeSizeX / 8f,  10 , -10);
            player.transform.position = new Vector3(planeSizeX / 8f,  10 , -10);
            targetPosition = player.position;
            active = true;
        }
        else
        {
            Debug.LogError("Plane object not assigned in the Inspector.");
        }
    } 

   
}
