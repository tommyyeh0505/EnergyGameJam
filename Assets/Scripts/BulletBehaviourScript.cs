using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] public bool hurtsEnemies;
    [SerializeField] public bool hurtsPlayer;
    public float bulletSpeed = 0.5f;
    private float shipVelocity;
    private ShipBehaviourScript ship;
    private readonly float lifetime = 1.0f;


    void Start()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
        ship = go[0].GetComponent<ShipBehaviourScript>();
    }

    void Update()
    {
        gameObject.transform.Translate(0f, 0.5f + Mathf.Abs(shipVelocity), 0f);
        Destroy(gameObject, lifetime);

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && hurtsPlayer)
        {
            //Player Dies?
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
