using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    public float minScrollAmount = 0.0f;
    public float maxScrollAmount = 5.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 20.0f;
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
            Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10.0f);
        }
    }
}
