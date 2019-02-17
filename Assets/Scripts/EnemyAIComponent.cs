using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAIComponent : MonoBehaviour
{
    public float speed = 5f;
    public float lungeRange = 5f;
    public float vicinity = 4f;
    public float recalculatePathThreshold = 8f;
    public float despawnRange = 30f;

    private GameObject playerRef;
    private bool alreadyMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef && alreadyMoving == false)
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
            else if (distance < lungeRange)
            {
                MoveTo(playerRef.transform.position);
            }
            else
            {
                StartCoroutine(ArcToPosition((Vector2)playerRef.transform.position + Random.insideUnitCircle * vicinity));
            }        
        }
    }

    IEnumerator ArcToPosition(Vector2 targetPos)
    {
        alreadyMoving = true;

        Vector2 originPos = transform.position;
        Vector2 midPoint = (originPos + targetPos) / 2;
        Vector2 BezierPivot = (originPos - midPoint).magnitude * Random.insideUnitCircle + midPoint;

        float totalDistanceEst = Vector2.Distance(originPos, BezierPivot) + Vector2.Distance(BezierPivot, targetPos);
        float totalTimeEst = totalDistanceEst / speed;

        float startTime = Time.time;
        float timeFrac = 0f;
        while (timeFrac < 1f)
        {
            if (playerRef && Vector2.Distance(playerRef.transform.position, targetPos) > recalculatePathThreshold)
            {
                break;
            }

            timeFrac = (Time.time - startTime) / totalTimeEst;
            float oneMinusTimeFrac = 1 - timeFrac;

            Vector2 intermediatePos = oneMinusTimeFrac * oneMinusTimeFrac * originPos + 2 * oneMinusTimeFrac * timeFrac * BezierPivot + timeFrac * timeFrac * targetPos;
            Rigidbody2D rigid2D = GetComponent<Rigidbody2D>();
            if (rigid2D)
            {
                rigid2D.MovePosition(intermediatePos);
            }

            yield return null;
        }

        alreadyMoving = false;
    }

    void MoveTo(Vector2 targetPos)
    {
        Rigidbody2D rigid2D = GetComponent<Rigidbody2D>();

        Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
        float distance = speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, moveDir));
        rigid2D.MovePosition((Vector2) transform.position + moveDir * distance);
    }
}
