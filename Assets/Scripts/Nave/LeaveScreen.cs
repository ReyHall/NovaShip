using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveScreen : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 screenBounds;
    private Vector2 objectSize;

    void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        
        if (renderer != null) objectSize = renderer.bounds.extents;
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
        
        transform.position = new Vector3(
            Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY),
            position.z
        );
    }
}
