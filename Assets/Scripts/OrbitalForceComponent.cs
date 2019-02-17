using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityWellComponentScript))]
[RequireComponent(typeof(CircleCollider2D))]
public class OrbitalForceComponent : MonoBehaviour
{
    public float orbitalDistance = 5.0f;
    public float orbitalNudgeStrength = 1.0f;
    public float orbitalNetForceFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        //CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        //if (circleCollider)
        //{
        //    Gizmos.DrawSphere(transform.position, circleCollider.radius * transform.localScale.x);
        //}
    }

    public bool IsInOrbit(float distance)
    {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider)
        {
            return distance < orbitalDistance + circleCollider.radius * transform.localScale.x;
        }
        return distance < orbitalDistance;
    }

    public void ApplyOrbitalForce(GravityComponentScript gravComponent, Vector2 force)
    {
        Vector2 incidentDir = (transform.position - gravComponent.transform.position).normalized;
        Vector2 perpendicular = Vector2.Perpendicular(incidentDir);

        bool clockWise = true;
        Rigidbody2D objectBody = gravComponent.GetComponent<Rigidbody2D>();
        if (objectBody)
        {
            Vector2 projection = Vector3.Project(objectBody.velocity, perpendicular);
            if ((projection - perpendicular).magnitude > projection.magnitude)
            {
                perpendicular = -perpendicular;
                clockWise = false;
            }
        }

        gravComponent.ApplyGravity(((perpendicular * force.magnitude * orbitalNudgeStrength) + force).normalized * orbitalNetForceFactor);

        ShipBehaviourScript behavior = gravComponent.GetComponent<ShipBehaviourScript>();
        if (behavior)
        {
            float angle = Vector2.Angle(Vector2.right, perpendicular);
            behavior.StartOrbit(clockWise, this);
        }
    }
}
