using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityIndicatorComponent : MonoBehaviour
{
    public GameObject trianglePrefab;
    public float radius = 5f;
    public float triangleScaleFactor;
    public float considerationRadius = 20f;

    private GameObject playerRef;
    private List<GameObject> currentIndicators = new List<GameObject>(); // from last from
    
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        transform.parent = playerRef.transform;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject indicator in currentIndicators)
        {
            Destroy(indicator);
        }
        currentIndicators.Clear();

        if (!playerRef)
        {
            return;
        }

        GravityWellComponentScript[] wells = FindObjectsOfType<GravityWellComponentScript>();
        List<Vector2> forces = new List<Vector2>();
        foreach (GravityWellComponentScript well in wells)
        {
            if (Vector2.Distance(playerRef.transform.position, well.transform.position) < considerationRadius)
            {
                forces.Add(well.FindForce(playerRef.transform.position));           
            }
        }

        foreach (Vector2 force in forces)
        {
            Vector2 position = (Vector2) transform.position + force.normalized * radius;
            Quaternion rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, force.normalized));
            float scale = Mathf.Clamp(force.magnitude * triangleScaleFactor, 0f, 2f);
            Vector2 scale2D = new Vector2(1, scale);

            GameObject triangle = Instantiate(trianglePrefab, position, rotation);
            triangle.transform.localScale = scale2D;
            currentIndicators.Add(triangle);
        }
    }
}
