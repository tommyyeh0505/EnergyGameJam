using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collision2D))]
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
        if (energyComponent && energyComponent.HasEnergyRemaining(shieldDrain))
        {
            shieldOn = true;
            shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            shield.transform.parent = gameObject.transform;

            Animator animator = GetComponent<Animator>();
            if (animator)
            {
                animator.SetBool("shield", true);
            }
        }
    }

    public void ToggleShieldOff()
    {
        shieldOn = false;
        Destroy(shield);
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.SetBool("shield", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject.tag);

        if (shieldOn)
        {
            Bounce(collision);
            //TODO: maybe kill thursters for 1 second after bounce for a disorientating effect
        }
        else
        {
            if (collision.gameObject.tag != "PlayerBullet")
            {
                Die();
            }
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

    public bool IsShieldOn()
    {
        return shieldOn;
    }

    void Update()
    {
        if (shieldOn && energyComponent)
        {
            energyComponent.ReduceEnergy(shieldDrain * Time.deltaTime);
            if (!energyComponent.HasEnergyRemaining(0))
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
