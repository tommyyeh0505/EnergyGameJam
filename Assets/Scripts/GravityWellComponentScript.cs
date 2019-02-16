using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityWellComponentScript : MonoBehaviour
{
    [SerializeField] public float strength = 5f;
    [SerializeField] public float orbitDistance = 3.0f;
    [SerializeField] public float effectiveRange = 100f;

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
                if (distance < effectiveRange)
                {
                    Vector2 dir = difference.normalized;
                    Vector2 force = dir * strength / distance;

                    target.ApplyGravity(force);
                }
            }
        }
    }
}
