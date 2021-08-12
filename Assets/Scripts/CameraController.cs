using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    // zoom
    float zoom = 8f;
    public float zoomSpeed = 4f;
    public float minZoom = 2f;
    public float maxZoom = 12f;

    // rotate
    public float pitch = 1.25f;
    public float yawSpeed = 0f;

    float yaw = 100f;

    void Update()
    {
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; // updates zoom
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom); // clamps zoom

        yaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime; // updates rotation
    }

    void LateUpdate()
    {
        transform.position = player.position - offset * zoom; // moves camera
        transform.LookAt(player.position + Vector3.up * pitch); // aims camera at player
        transform.RotateAround(player.position, Vector3.up, yaw); // rotates camera
    }
}
