using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityWellComponentScript))]
public class OrbitalForceComponent : MonoBehaviour
{
    public float orbitalDistance = 5.0f;
    public float orbitalNudgeStrength = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsInOrbit(float distance)
    {
        return distance < orbitalDistance;
    }

    public void ApplyOrbitalForce(GravityComponentScript gravComponent, Vector2 force)
    {
        Vector2 incidentDir = (transform.position - gravComponent.transform.position).normalized;
        Vector2 perpendicular = Vector2.Perpendicular(incidentDir);

        Rigidbody2D objectBody = gravComponent.GetComponent<Rigidbody2D>();
        if (objectBody)
        {
            Vector2 projection = Vector3.Project(objectBody.velocity, perpendicular);
            if ((projection - perpendicular).magnitude > projection.magnitude)
            {
                perpendicular = -perpendicular;
            }
        }

        gravComponent.ApplyGravity((perpendicular * orbitalNudgeStrength) + force);

        //TODO: wap?
        //ShipBehaviourScript behavior = gravComponent.GetComponent<ShipBehaviourScript>();
        //if (behavior)
        //{
        //    float angle = Vector2.Angle(Vector2.right, perpendicular);
        //    behavior.StartOrbit(this);
        //}
    }
}
