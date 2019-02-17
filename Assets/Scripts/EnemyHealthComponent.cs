using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collision2D))]
public class EnemyHealthComponent : MonoBehaviour
{
    [SerializeField] public GameObject prefabEnergyPickup;
    private bool alreadyDead = false;
    public ParticleSystem explosion;
    public float MutualCollisionKillSpeed = 5f;
    
    public float cameraShakeDurationOnKill = 0.05f;
    public float cameraShakeMagnitudeOnKill = 5f;

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
            if (collision.relativeVelocity.magnitude < MutualCollisionKillSpeed)
            {
                return;
            }
        }

        if (collision.gameObject.tag == "Bullet")
        {
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

    public void Die()
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

        CameraMovement camera = Camera.main.GetComponent<CameraMovement>();
        if (camera)
        {
            camera.ShakeCamera(cameraShakeMagnitudeOnKill, cameraShakeDurationOnKill);
        }

        StartCoroutine(DestroyTimer(explosion.duration));
    }

    private IEnumerator DestroyTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
