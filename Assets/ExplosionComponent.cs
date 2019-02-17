using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionComponent : MonoBehaviour
{
    public ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
       
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer)
        {
            Debug.Log("explode");
            // TODO: death anim
            explosion.Play();
            renderer.color = Color.clear;
        }
        Debug.Log("done explosion1");
    }
}
