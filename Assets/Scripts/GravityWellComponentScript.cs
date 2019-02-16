using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityWellComponentScript : MonoBehaviour
{
    [SerializeField] public float strength = 9.8f;

    void Update()
    {
        GravityFieldScript gravityManager = Camera.main.GetComponent<GravityFieldScript>();
        if (gravityManager)
        {
            List<GravityComponentScript> affected = gravityManager.GetAffected();
            foreach (GravityComponentScript target in affected)
            {
                target.CalculateGravity(strength, this.transform.position);
            }
        }
    }
}
