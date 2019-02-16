using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviourScript : MonoBehaviour {

	public float speed = 3f;
    public float rotateSpeed = 0.1f;   // degrees per second
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        moveDirection = new Vector3(0, Input.GetAxis("Vertical"), 0);
        moveDirection = transform.TransformDirection(moveDirection);

        transform.Rotate(0, 0, Input.GetAxis("Horizontal") * rotateSpeed);
        controller.Move(moveDirection * speed * Time.deltaTime);
	}
}
