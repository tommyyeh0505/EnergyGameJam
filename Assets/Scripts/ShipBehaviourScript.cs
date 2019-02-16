using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipBehaviourScript : MonoBehaviour {

	public float speed = 3f;
    public float rotateSpeed = 0.1f;   // degrees per second
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody2D rgbd2d;

	// Use this for initialization
	void Start () {
        rgbd2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        moveDirection = new Vector3(0, Input.GetAxis("Vertical"), 0);
        moveDirection = transform.TransformDirection(moveDirection);

        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotateSpeed);
        rgbd2d.AddForce(moveDirection * speed * Time.deltaTime);

        if (Input.GetButtonDown("reset"))
        {
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0.0f);
            rgbd2d.velocity = Vector3.zero;
        }
	}
}
