using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.1f;

    [Header("Boundaries")]
    public float minX = 0f;
    public float maxX = 16f;
    public float minY = 0f;
    public float maxY = 15f;

    // Follows the set object, manually set to the player.
    void LateUpdate()
    {
        if (target == null) return;

        // Follow target
        float x = Mathf.Clamp(target.position.x, minX, maxX);
        float y = Mathf.Clamp(target.position.y, minY, maxY);

        Vector3 desired = new Vector3(x, y, -10f);
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed);
    }
}