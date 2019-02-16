using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAIComponent : MonoBehaviour
{
    public float impulse = 3f;
    public float timeBetweenMoves = 0.2f;

    private GameObject playerRef;
    private float lastMoveTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef)
        {
            if (Time.time - lastMoveTime > timeBetweenMoves)
            {
                MoveTo(playerRef.transform.position);
            }
        }
    }

    void MoveTo(Vector2 targetPos)
    {
        Rigidbody2D rigid2D = GetComponent<Rigidbody2D>();
        rigid2D.AddForce((targetPos - (Vector2) transform.position).normalized * impulse);
    }
}
