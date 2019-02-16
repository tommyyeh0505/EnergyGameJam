using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (body)
        {
            body.MovePosition(body.GetRelativePoint(new Vector2(0, 0.5f)));
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            return;
        }
        if (col.gameObject.tag == "Enemy")
        {
            //Reduce health of enemy
            //if enemy is dead, destroy
            Destroy(col.gameObject);
        }
        Destroy(gameObject);
    }
}
