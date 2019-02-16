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

    private void OnDestroy()
    {
        GravityFieldScript gravityManager = Camera.main.GetComponent<GravityFieldScript>();
        if (gravityManager)
        {
            gravityManager.UnregisterAffected(this);
        }
    }

    public void ApplyGravity(Vector2 force)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body)
        {
            body.AddForce(force * body.velocity.magnitude);
        }
    }
}
