using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collision2D))]
[RequireComponent(typeof(ShipEnergyComponent))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class ShipHealthComponent : MonoBehaviour
{
    [SerializeField] public float shieldDrain = 15f;
    ShipEnergyComponent energyComponent;
    public ParticleSystem explosion;

    public float destroyedRetainTimer = 4f;
    public GameObject shieldPrefab;
    public float shieldBounciness = 80f;

    public float cameraShakeDuration = 0.5f;
    public float cameraShakeMagnitude = 1f;

    private GameObject shield;
    private bool shieldOn = false;
    private bool dead = false;

    private CircleCollider2D CircleCollider;
    private PolygonCollider2D PolygonCollider;

    [SerializeField] public AudioClip explodeAudio;

    void Start()
    {
        energyComponent = GetComponent<ShipEnergyComponent>();
        CircleCollider = GetComponent<CircleCollider2D>();
        PolygonCollider = GetComponent<PolygonCollider2D>();
    }

    public void ToggleShieldOn()
    {
        if (energyComponent && energyComponent.HasEnergyRemaining(0))
        {
            shieldOn = true;
            Debug.Log("CircleCollider Active");
            CircleCollider.enabled = true;
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
        CircleCollider.enabled = false;
        shieldOn = false;
        Destroy(shield);
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.SetBool("shield", false);
        }
    }

    //Shield Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnergyPickup")
        {
            return;
        }
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "EnergyPickup")
        {
            return;
        }

        if (shieldOn)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                Destroy(collision.gameObject);
            } else if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyHealthComponent>().Die();
            }
            else
            {
                Debug.Log("bounced");
                Bounce(collision);
            }
            //TODO: maybe kill thursters for 1 second after bounce for a disorientating effect
        }
        else
        {
            if (collision.gameObject.tag != "PlayerBullet" && !dead)
            {
                Bounce(collision);
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

        CameraMovement camera = Camera.main.GetComponent<CameraMovement>();
        if (camera)
        {
            camera.ShakeCamera(cameraShakeMagnitude, cameraShakeDuration);
        }
    }

    private void Die()
    {
        Debug.Log("DIE");
        dead = true;
        SpriteRenderer ren = GetComponent<SpriteRenderer>();
        if (ren)
        {
            // TODO: death anim
            StartCoroutine(PlayExplosion());
            Debug.Log("after exp");
            ren.color = Color.red;
        }
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.PlayOneShot(explodeAudio, 1);
        }
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body)
        {
            body.Sleep();
        }
        ShipBehaviourScript behavior = GetComponent<ShipBehaviourScript>();
        if (behavior)
        {
            behavior.Die();
        }

        ShipGunComponent firing = GetComponent<ShipGunComponent>();
        if (firing)
        {
            firing.enabled = false;
        }

        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.SetTrigger("dying");
        }
        GameObject.FindGameObjectsWithTag("GameUI")[0].SetActive(false);
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyedRetainTimer);
        Destroy(gameObject);
        Camera.main.GetComponent<GameOver>().SetGameOverScreen();
    }

    private IEnumerator PlayExplosion()
    {
        yield return new WaitForSeconds(0);
        explosion.Play();
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
