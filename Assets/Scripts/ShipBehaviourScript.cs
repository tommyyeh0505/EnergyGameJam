using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipBehaviourScript : MonoBehaviour {

	public float thrust = 300f;
    public float maxSpeed = 30f;
    public float rotateSpeed = 0.1f;   // degrees per second
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody2D rgbd2d;

	// Use this for initialization
	void Start () {
        rgbd2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        moveDirection = new Vector3(0, Mathf.Max(0.0f, Input.GetAxis("Vertical")), 0);
        moveDirection = transform.TransformDirection(moveDirection);

        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotateSpeed);

        rgbd2d.AddForce(moveDirection * thrust * Time.deltaTime);
        if (rgbd2d.velocity.magnitude > maxSpeed)
        {
            rgbd2d.velocity = rgbd2d.velocity.normalized * maxSpeed;
        }

        if (Input.GetButtonDown("reset"))
        {
            transform.position = Vector3.zero;
            rgbd2d.velocity = Vector3.zero;
        }

        if (Input.GetButtonDown("shield"))
        {
            ShipHealthComponent shield = GetComponent<ShipHealthComponent>();
            if (shield)
            {
                shield.ToggleShield(true);
            }
        }

        if (Input.GetButtonUp("shield"))
        {
            ShipHealthComponent shield = GetComponent<ShipHealthComponent>();
            if (shield)
            {
                shield.ToggleShield(false);
            }
        }
    }
}
