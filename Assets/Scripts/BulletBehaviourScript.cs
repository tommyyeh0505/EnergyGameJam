using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{
    private Rigidbody2D body;
    public bool hurtsEnemies = true;
    public bool hurtsPlayer = true;
    public float bulletSpeed = 0.5f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (body)
        {
            body.MovePosition(body.GetRelativePoint(new Vector2(0, bulletSpeed)));
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && hurtsPlayer)
        {
        }
        if (col.gameObject.tag == "Enemy" && hurtsEnemies)
        {
            //Reduce health of enemy
            //if enemy is dead, destroy
            Destroy(col.gameObject);
        }
        Destroy(gameObject);
    }
}
