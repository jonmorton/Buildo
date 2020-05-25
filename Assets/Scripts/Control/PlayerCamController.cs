using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamController : MonoBehaviour
{

    public float panSpeed = 13f;

    public float zoomMin = 5.0f;
    public float zoomMax = 50.0f;
    public float zoomSensitivity = 0.01f;
    public float zoomSpeed = 4f;

    private Vector2 curPanVelocity = Vector2.zero;
    private float targetZoom;
    private new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        targetZoom = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 deltaPan = curPanVelocity * Time.deltaTime * targetZoom / 10f;
        transform.Translate(deltaPan.x, deltaPan.y, 0f);

        float curZoom = camera.orthographicSize;
        float deltaZoom = targetZoom - curZoom;

        if (Math.Abs(deltaZoom) > 0.01)
        {
            // deltaZoom = deltaZoom > 0 ? Math.Max(0.5f, deltaZoom) : Math.Min(-0.5f, deltaZoom);
            curZoom += deltaZoom * Time.deltaTime * zoomSpeed;
        }
        else
        {
            curZoom = targetZoom;
        }

        camera.orthographicSize = curZoom;
    }

    void OnMove(InputValue value)
    {
        curPanVelocity = value.Get<Vector2>() * panSpeed;
    }

    void OnZoom(InputValue value)
    {
        float mag = value.Get<Vector2>().y * zoomSensitivity * -1f;
        targetZoom = Math.Min(zoomMax, Math.Max(zoomMin, targetZoom + mag));
    }
}