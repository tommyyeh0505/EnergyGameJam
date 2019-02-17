using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    public float minScrollAmount = 0.0f;
    public float maxScrollAmount = 5.0f;
    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;

    //public Vector3 cameraVelocity = new Vector3(0, 0, -10.0f);
    public Vector3 shakeCameraOffset = new Vector3(0, 0, -10.0f);
    public bool isShaking = false;
    public int shakeFreaquency = 1000;
    public float shakeAmount = 1f;
    public float shakeDuration = 3f;
    private float timeStartShake = 0f;
    private Coroutine cameraShakeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float scrollAmount = -Input.GetAxis("Mouse ScrollWheel");
        float direction = Mathf.Sign(scrollAmount);
        if (Mathf.Abs(scrollAmount) > Mathf.Epsilon)
        {
            float zoomAmount = direction * Mathf.Clamp(scrollAmount * scrollSpeed, minScrollAmount, maxScrollAmount);

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + zoomAmount, minZoom, maxZoom);
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player)
        {
            // delay effect wtf is that jerking motion?
            //cameraVelocity = (new Vector3(player.transform.position.x, player.transform.position.y, -10f)- Camera.main.transform.position);
            //Camera.main.transform.position += cameraVelocity*Time.deltaTime;

            Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        }
    }

    public void ShakeCamera(float magnitude, float duration)
    {
        if (cameraShakeCoroutine != null)
        {
            StopCoroutine(cameraShakeCoroutine);
        }

        cameraShakeCoroutine = StartCoroutine(StartCameraShake(magnitude, duration));
    }

    IEnumerator StartCameraShake(float magnitude, float duration)
    {
        timeStartShake = Time.time;

        while(Time.time - timeStartShake < shakeDuration)
        {
            float completion = ((Time.time - timeStartShake) - shakeDuration) / shakeDuration;
            // calculate the offset of the camera
            shakeCameraOffset = new Vector3(Mathf.PerlinNoise(Time.time * shakeFreaquency * completion, 0f) * shakeAmount * completion,
                                            Mathf.PerlinNoise(0f, Time.time * shakeFreaquency * completion) * shakeAmount * completion, -10f);
            Camera.main.transform.position += shakeCameraOffset * magnitude;

            yield return null;
        }
    }
}
