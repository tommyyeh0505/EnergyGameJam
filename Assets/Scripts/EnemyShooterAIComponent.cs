using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyShooterAIComponent : EnemyAIComponent
{
    [SerializeField] public AudioClip fireAudio;
    private AudioSource audioSource;

    public float tooCloseDistance = 5f;
    public float safeSpace = 10f;
    public float tooFarDistance = 12f;
    public GameObject bulletPrefab;
    public float timeBetweenShots = 2f;
    public float shotAngleDeviance = 20f;

    private Coroutine movementArc;
    private bool runningAway = false;
    private bool approaching = false;
    private float lastFiredTime;

    // Start is called before the first frame update
    void Start()
    {
        lastFiredTime = Time.time;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef)
        {
            float distance = Vector3.Distance(playerRef.transform.position, transform.position);
            if (distance > despawnRange)
            {
                EnemyHealthComponent health = GetComponent<EnemyHealthComponent>();
                if (health)
                {
                    health.Die();
                }
            }
            else if (distance < tooCloseDistance)
            {
                if (!runningAway)
                {
                    Vector2 escapeVector = (transform.position - playerRef.transform.position).normalized * safeSpace;
                    MoveArc((Vector2) transform.position + escapeVector);
                    runningAway = true;
                    approaching = false;
                }
            }
            else if (distance > tooFarDistance)
            {
                if (!approaching)
                {
                    approaching = true;
                    runningAway = false;
                    MoveArc(((Vector2)playerRef.transform.position + Random.insideUnitCircle * vicinity));
                }
            }
            else
            {
                approaching = false;
                runningAway = false;
                Shoot();
            }
        }
    }

    void StopMoveArc()
    {
        if (movementArc != null)
        {
            StopCoroutine(movementArc);
            movementArc = null;
        }
    }

    void MoveArc(Vector2 targetPos)
    {
        StopMoveArc();
        movementArc = StartCoroutine(ArcToPosition(targetPos));
    }

    void Shoot()
    {
        if (playerRef && Time.time - lastFiredTime > timeBetweenShots)
        {
            Vector2 bulletDirection = (playerRef.transform.position - transform.position).normalized;
            float angle = Vector2.SignedAngle(Vector2.up, bulletDirection);
            angle += Random.Range(-shotAngleDeviance, shotAngleDeviance);

            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

            Instantiate(bulletPrefab, transform.position, bulletRotation);
            lastFiredTime = Time.time;
            if (audioSource)
            {
                audioSource.PlayOneShot(fireAudio, 1);
            }
        }
    }
}
