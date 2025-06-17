using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Assign these cameras in the Inspector.
    // Make sure these are actual Camera components in your scene, NOT the main camera.
    public Camera player1Camera;
    public Camera player2Camera;

    // Assign Player 1 (already in scene) and Player 2 (will be spawned) transforms
    public Transform player1Target;
    private Transform player2Target; // This will be set by PlayerJoinManager

    public float smoothSpeed = 0.125f; // How smoothly the camera follows the player
    public Vector3 offset = new Vector3(0, 2, -10); // Offset from the player (x, y, z)

    void Start()
    {
        // Ensure both cameras exist
        if (player1Camera == null || player2Camera == null)
        {
            Debug.LogError("Player cameras are not assigned in CameraManager!");
            enabled = false; // Disable script if cameras are missing
            return;
        }

        // Set up the initial viewports for split-screen
        // Player 1 Camera: Left half of the screen
        player1Camera.rect = new Rect(0f, 0f, 0.5f, 1f);
        // Player 2 Camera: Right half of the screen (initially hidden or empty until P2 joins)
        // We'll adjust this when P2 joins. For now, it's just a placeholder.
        player2Camera.rect = new Rect(0.5f, 0f, 0.5f, 1f);

        // Initially disable player2Camera if P2 hasn't joined
        player2Camera.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        // Player 1 camera follow
        if (player1Target != null)
        {
            Vector3 desiredPosition1 = player1Target.position + offset;
            Vector3 smoothedPosition1 = Vector3.Lerp(player1Camera.transform.position, desiredPosition1, smoothSpeed);
            player1Camera.transform.position = smoothedPosition1;
        }

        // Player 2 camera follow (only if Player 2 has joined)
        if (player2Target != null)
        {
            // Activate player2Camera when P2 joins
            if (!player2Camera.gameObject.activeSelf)
            {
                player2Camera.gameObject.SetActive(true);
            }

            Vector3 desiredPosition2 = player2Target.position + offset;
            Vector3 smoothedPosition2 = Vector3.Lerp(player2Camera.transform.position, desiredPosition2, smoothSpeed);
            player2Camera.transform.position = smoothedPosition2;
        }
    }

    // This method is called by PlayerJoinManager when Player 2 joins
    public void SetPlayer2Target(Transform target)
    {
        player2Target = target;
        Debug.Log("CameraManager received Player 2 target.");
    }
}
