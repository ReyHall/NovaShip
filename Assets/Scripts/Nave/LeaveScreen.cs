using UnityEngine;

public class LeaveScreen : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 screenBounds;
    private Vector2 objectSize;

    void Start()
    {
        mainCamera = Camera.main;

        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        screenBounds = new Vector2(cameraWidth / 2, cameraHeight / 2);

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
            objectSize = renderer.bounds.extents;
    }

    void LateUpdate()
    {
        PositionCam();
    }

    private void PositionCam()
    {
        Vector3 position = transform.position;
        Vector3 camPos = mainCamera.transform.position;

        float minX = camPos.x - screenBounds.x + objectSize.x;
        float maxX = camPos.x + screenBounds.x - objectSize.x;
        float minY = camPos.y - screenBounds.y + objectSize.y;
        float maxY = camPos.y + screenBounds.y - objectSize.y;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        transform.position = position;
    }
}
