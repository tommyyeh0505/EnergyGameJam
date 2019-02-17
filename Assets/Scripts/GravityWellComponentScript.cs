using UnityEngine;
using System.Collections.Generic;

using System.Collections;
public class GravityWellComponentScript : MonoBehaviour
{
    [SerializeField] public float strength = 5f;
    [SerializeField] public float effectiveRange = 100f;

    private OrbitalForceComponent orbitalForce;

    private void Start()
    {
        orbitalForce = GetComponent<OrbitalForceComponent>();
    }

    void Update()
    {
        GravityFieldScript gravityManager = Camera.main.GetComponent<GravityFieldScript>();
        if (gravityManager)
        {
            List<GravityComponentScript> affected = gravityManager.GetAffected();
            foreach (GravityComponentScript target in affected)
            {
                Vector2 difference = this.transform.position - target.transform.position;
                float distance = difference.magnitude;
                float distanceFactor = difference.sqrMagnitude / distance;
                Vector2 dir = difference.normalized;
                Vector2 force = dir * strength / distanceFactor;

                if (orbitalForce && orbitalForce.IsInOrbit(distance))
                {
                    orbitalForce.ApplyOrbitalForce(target, force);
                }
                else if (distance < effectiveRange)
                {
                    target.ApplyGravity(force);
                }
            }
        }
    }
}
