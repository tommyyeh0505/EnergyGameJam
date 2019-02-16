﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipEnergyComponent))]
public class ShipHealthComponent : MonoBehaviour
{
    [SerializeField] public float shieldDrain = 15f;
    ShipEnergyComponent energyComponent;

    public float destroyedRetainTimer = 4f;
    public GameObject shieldPrefab;
    public float shieldBounciness = 80f;

    private GameObject shield;
    private bool shieldOn = false;

    void Start()
    {
        energyComponent = GetComponent<ShipEnergyComponent>();
    }

    public void ToggleShieldOn()
    {
        if (energyComponent && energyComponent.HasEnergy())
        {
            shieldOn = true;
            shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            shield.transform.parent = gameObject.transform;
        }
    }

    public void ToggleShieldOff()
    {
        shieldOn = false;
        Destroy(shield);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (shieldOn)
        {
            Bounce(collision);
            //TODO: maybe kill thursters for 1 second after bounce for a disorientating effect
        }
        else
        {
            Die();
        }
    }

    private void Bounce(Collision2D collision)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body)
        {
            body.AddForce((body.position - (Vector2)collision.transform.position).normalized * body.velocity.magnitude * shieldBounciness);
        }

    }

    private void Die()
    {
        SpriteRenderer ren = GetComponent<SpriteRenderer>();
        if (ren)
        {
            // TODO: death anim
            ren.color = Color.red;
        }
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body)
        {
            body.Sleep();
        }
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyedRetainTimer);
        Destroy(gameObject);
    }

    void Update()
    {
        if (shieldOn && energyComponent)
        {
            energyComponent.ReduceEnergy(shieldDrain * Time.deltaTime);
            if (!energyComponent.HasEnergy())
            {
                ToggleShieldOff();
            }
        }

        if (Input.GetButtonDown("shield"))
        {
            ToggleShieldOn();
        }

        if (Input.GetButtonUp("shield"))
        {
            ToggleShieldOff();
        }
    }
}
