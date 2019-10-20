using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform camera;
    public float horizontalMovementRate = 0.1f;
    public float VerticalMovementRate = 0.1f;

    private float cameraStartPointX;
    private float cameraStartPointY;

    private float objectStartPointX;
    private float objectStartPointY;

    void Start()
    {
        cameraStartPointX = camera.position.x;
        cameraStartPointY = camera.position.y;

        objectStartPointX = transform.position.x;
        objectStartPointY = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector2(objectStartPointX + (camera.position.x - cameraStartPointX) * horizontalMovementRate, objectStartPointY + (camera.position.y - cameraStartPointY) * VerticalMovementRate);
    }
}
