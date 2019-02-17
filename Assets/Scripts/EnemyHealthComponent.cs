using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collision2D))]
public class EnemyHealthComponent : MonoBehaviour
{
    [SerializeField] public GameObject prefabEnergyPickup;
    [SerializeField] public GameObject ExplosionPrefab;
    [SerializeField] public int energyPickupSpawnFrequency;
    private bool alreadyDead = false;
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
            if (shipHealth)
            {
                shipHealth.KilledEnemy();
            }
            Die();
        }
    }

    public void Die()
    {
        Camera.main.GetComponent<ExplosionController>().ExplodeAtLocation(gameObject.transform.position);
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

        StartCoroutine(DestroyTimer(0));

        System.Random random = new System.Random();
        if (random.Next(energyPickupSpawnFrequency) == 0)
        {
            Instantiate(prefabEnergyPickup, gameObject.transform.position, Quaternion.identity);
        }
    }

    private IEnumerator DestroyTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
