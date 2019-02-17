using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ShipEnergyComponent))]
public class ShipBehaviourScript : MonoBehaviour {

	public float thrust = 300f;
    public float maxSpeed = 30f;
    public float rotateSpeed = 0.1f;   // degrees per second
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody2D rgbd2d;
    private ShipEnergyComponent energyComponent;
    private float thrustDrain = 5f;
    private Quaternion orientation;
    private Quaternion currentDirection;
    private OrbitalForceComponent orbitRef;
    private bool currentlyOrbiting = false;

	void Start () {
        rgbd2d = GetComponent<Rigidbody2D>();
        energyComponent = GetComponent<ShipEnergyComponent>();
        orientation = Quaternion.identity;
        currentDirection = Quaternion.identity;
	}

    void Update()
    {
        moveDirection = new Vector3(0, Mathf.Max(0.0f, Input.GetAxis("Vertical")), 0);
        moveDirection = transform.TransformDirection(moveDirection);

        currentDirection *= Quaternion.Euler(0, 0, -Input.GetAxis("Horizontal") * rotateSpeed);
        transform.rotation = orientation * currentDirection;

        rgbd2d.AddForce(moveDirection * thrust * Time.deltaTime);
        if (rgbd2d.velocity.magnitude > maxSpeed)
        {
            rgbd2d.velocity = rgbd2d.velocity.normalized * maxSpeed;
        }

        if (energyComponent && Input.GetButton("Vertical") ) {
            energyComponent.ReduceEnergy(thrustDrain * Time.deltaTime);
        }

        if (Input.GetButtonDown("reset"))
        {
            transform.position = Vector3.zero;
            rgbd2d.velocity = Vector3.zero;
            energyComponent.ResetEnergy();
        }
    }
}
