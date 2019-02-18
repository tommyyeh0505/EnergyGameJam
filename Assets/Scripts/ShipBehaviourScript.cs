using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ShipEnergyComponent))]
public class ShipBehaviourScript : MonoBehaviour {

    public float thrust = 300f;
    public float maxSpeed = 30f;
    public float rotateSpeed = 0.1f;   // degrees per second
    public float rotationDragFactor = 0.5f;
    public float decelerationWeight = 2f;
    [SerializeField] public Vector2 velocity;
    public float maxSpeedDecelerationFactor = 0.98f;
    public float speedBoostForceMagnitude = 5f;
    public float speedBoostDuration = 1f;

    private Rigidbody2D rgbd2d;
    private ShipEnergyComponent energyComponent;
    [SerializeField] public float thrustDrain = 5f;
    private Quaternion orientation;
    private Quaternion currentDirection;

    private Vector2 orbitalOrientation;
    private OrbitalForceComponent orbitRef;
    private bool currentlyOrbiting = false;
    private bool orbitClockWise = false;
    private bool alreadyDead = false;
    private bool isSpeedBoosting = false;
    private Coroutine speedCorouting;


    void Start () {
        rgbd2d = GetComponent<Rigidbody2D>();
        energyComponent = GetComponent<ShipEnergyComponent>();
        orientation = Quaternion.identity;
        currentDirection = Quaternion.identity;
        alreadyDead = false;
	}

    void Update()
    {
        if (alreadyDead == false)
        {
            currentDirection *= Quaternion.Euler(0, 0, -Input.GetAxis("Horizontal") * rotateSpeed);

            Vector3 moveDirection = new Vector3(0, Mathf.Max(0.0f, Input.GetAxis("Vertical")), 0);
            moveDirection = transform.TransformDirection(moveDirection);

            Vector2 force = moveDirection * thrust;
            Vector2 projection = Vector3.Project(force, rgbd2d.velocity);
            float degree = Vector2.SignedAngle(projection, rgbd2d.velocity);
            if (Mathf.Abs(Vector2.SignedAngle(projection, rgbd2d.velocity)) > float.Epsilon + 1)
            {
                force = force + projection.normalized * decelerationWeight * force.magnitude;
            }

            rgbd2d.AddForce(force * Time.deltaTime);
            if (isSpeedBoosting == false && rgbd2d.velocity.magnitude > maxSpeed)
            {
                rgbd2d.velocity *= (maxSpeed / rgbd2d.velocity.magnitude) * maxSpeedDecelerationFactor;
            }

            if (energyComponent && Input.GetButton("Vertical"))
            {
                energyComponent.ReduceEnergy(thrustDrain * Time.deltaTime);
            }

            if (Input.GetButtonDown("reset"))
            {
                transform.position = Vector3.zero;
                rgbd2d.velocity = Vector3.zero;
                energyComponent.ResetEnergy();
                currentlyOrbiting = false;
                orbitRef = null;
            }
        }

        if (currentlyOrbiting)
        {
            Vector2 difference = orbitRef.transform.position - transform.position;
            if (orbitRef.IsInOrbit(difference.magnitude))
            {
                currentlyOrbiting = false;
            }       

            Vector2 dirToObject = (orbitRef.transform.position - transform.position).normalized;
            orientation *= Quaternion.Euler(0, 0, (orbitClockWise ? -1 : 1) * Mathf.LerpAngle(0, Vector2.Angle(dirToObject, orbitalOrientation), rotationDragFactor));
            orbitalOrientation = dirToObject;
        }
        velocity = rgbd2d.velocity;

        transform.rotation = orientation * currentDirection;


    }

    public void StartOrbit(bool clockWise, OrbitalForceComponent orbital)
    {
        if (currentlyOrbiting == false)
        {
            orbitRef = orbital;
            orbitalOrientation = (orbitRef.transform.position - transform.position).normalized;
            currentlyOrbiting = true;
            orbitClockWise = clockWise;
        }
    }

    public void Die()
    {
        alreadyDead = true;
    }

    public void SpeedBoost()
    {
        isSpeedBoosting = true;
        Vector2 moveDirection = transform.TransformDirection(Vector2.up);
        rgbd2d.AddForce(moveDirection * speedBoostForceMagnitude, ForceMode2D.Impulse);

        if (speedCorouting != null)
        {
            StopCoroutine(speedCorouting);
        }
        speedCorouting = StartCoroutine(StopSpeedBoost());
    }

    IEnumerator StopSpeedBoost()
    {
        yield return new WaitForSeconds(speedBoostDuration);
        isSpeedBoosting = false;
    }
}
