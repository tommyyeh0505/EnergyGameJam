using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityComponentScript : MonoBehaviour
{
    void Start()
    {
        GravityFieldScript gravityManager = Camera.main.GetComponent<GravityFieldScript>();
        if (gravityManager)
        {
            gravityManager.RegisterAffected(this);
        }
    }

    public void CalculateGravity(float strength, Vector3 gravityWellPosition)
    {
        float r = Vector3.Distance(gravityWellPosition, this.transform.position);
        float rsquared = r * r;
        Vector2 dir = (gravityWellPosition - this.transform.position).normalized;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body)
        {
            body.AddForce(strength * body.mass * dir / rsquared);
        }
    }
}
