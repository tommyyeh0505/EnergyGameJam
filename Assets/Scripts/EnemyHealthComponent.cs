﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collision2D))]
public class EnemyHealthComponent : MonoBehaviour
{
    [SerializeField] public GameObject prefabEnergyPickup;
    float destroyedRetainTimer = 4f;
    private bool alreadyDead = false;
    public ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("enemy enemy");
            return;
        }
        ShipHealthComponent shipHealth = collision.gameObject.GetComponent<ShipHealthComponent>();
        if (shipHealth && !shipHealth.IsShieldOn())
        {
            return;
        }
        if (!alreadyDead)
        {
            Instantiate(prefabEnergyPickup, gameObject.transform.position, Quaternion.identity);
            Die();
        }
    }

    private void Die()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer)
        {
            // TODO: death anim
            explosion.Play();
            renderer.color = Color.clear;
        }
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body)
        {
            body.Sleep();
        }

        EnemyAIComponent ai = GetComponent<EnemyAIComponent>();
        ai.enabled = false;

        EnemySpawnerComponent spawner = Camera.main.GetComponent<EnemySpawnerComponent>();
        if (spawner)
        {
            spawner.EnemyDied();
        }

        alreadyDead = true;
      //  Destroy(gameObject);

        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyedRetainTimer);
        Destroy(gameObject);
    }
}
