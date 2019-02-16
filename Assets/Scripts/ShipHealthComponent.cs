using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipEnergyComponent))]
public class ShipHealthComponent : MonoBehaviour
{
    public float destroyedRetainTimer = 4f;
    public GameObject shieldPrefab;
    public float shieldBounciness = 80f;

    private GameObject shield;
    private bool shieldOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToggleShield(bool On)
    {
        if (On)
        {
            ShipEnergyComponent energyComponent = GetComponent<ShipEnergyComponent>();

            if (energyComponent && energyComponent.HasEnergy())
            {
                shieldOn = On;
                shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                shield.transform.parent = gameObject.transform;
            }
            else {
                return;
            }
        }
        else
        {
            Destroy(shield);
        }

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
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer)
        {
            // TODO: death anim
            renderer.color = Color.red;
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

    // Update is called once per frame
    void Update()
    {
        ShipEnergyComponent energyComponent = GetComponent<ShipEnergyComponent>();

        // Energy reduction from shield
        if (shieldOn && energyComponent) {
            energyComponent.ReduceEnergy(10f * Time.deltaTime);
        }

        if (Input.GetButtonDown("shield"))
        {
            ToggleShield(true);
        }

        if (Input.GetButtonUp("shield"))
        {
            ToggleShield(false);
        }
    }
}
