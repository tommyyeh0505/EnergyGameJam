using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemySpawnerComponent : MonoBehaviour
{
    //Stores bottom left and top right corners of gameobject
    public struct OccupiedSpace
    {
        public Vector3 bottomleft;
        public Vector3 topright;

        public OccupiedSpace(Vector3 bl, Vector3 tr)
        {
            bottomleft = bl;
            topright = tr;
        }
    }

    private Vector3 CameraPos;
    private float CameraWidth;
    private float CameraHeight;
    private float[] SpawnBoundaries; //[Top, Bottom, Left, Right]
    [SerializeField] float SpawnBoundaryPadding;
    private List<OccupiedSpace> entityPositions;
    [SerializeField] float LargestEnemyRadius;

    // Start is called before the first frame update
    void Start()
    {
        CameraPos = Camera.main.transform.position;
        float OrthSize = Camera.main.orthographicSize;
        CameraHeight = 2f * OrthSize;
        CameraWidth = Camera.main.aspect * OrthSize;
        //Debug.Log("CameraLocation: " + CameraPos.ToString());
        //Debug.Log("Height: " + CameraHeight);
        //Debug.Log("Width: " + CameraWidth);
        SpawnBoundaries = new float[4];
        SpawnBoundaries[0] = CameraPos.y + OrthSize + SpawnBoundaryPadding;
        SpawnBoundaries[1] = 0f - OrthSize - SpawnBoundaryPadding;
        SpawnBoundaries[2] = 0f - (OrthSize * Camera.main.aspect) - SpawnBoundaryPadding;
        SpawnBoundaries[3] = CameraPos.x + (OrthSize * Camera.main.aspect) + SpawnBoundaryPadding;
        //Debug.DrawLine(new Vector3(SpawnBoundaries[2], SpawnBoundaries[1], 0), new Vector3(SpawnBoundaries[3], SpawnBoundaries[0], 0), Color.white, 10.0f);

    }

    public void SpawnEnemy(string EnemyType, Vector3 location)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        entityPositions = new List<OccupiedSpace>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("OccupiesSpace");
        Debug.Log("Number of Objects occupying space: " + objects.Length);
        for(int i = 0; i < objects.Length; i++)
        {
            Vector3 pos = objects[i].transform.position;
            Debug.Log("Space occupied: " + pos.ToString());
            Vector3 bl = new Vector3(pos.x - LargestEnemyRadius, pos.y - LargestEnemyRadius, 0.0f);
            Vector3 tr = new Vector3(pos.x + LargestEnemyRadius, pos.y + LargestEnemyRadius, 0.0f);
            entityPositions.Add(new OccupiedSpace(bl, tr));
            DebugDrawColoredRectangle(bl, 2 * LargestEnemyRadius);
        }
    }

    void DebugDrawColoredRectangle(Vector3 position, float size)
    {
        Debug.DrawLine(position, new Vector3(position.x + size, position.y, position.z), Color.red, Time.deltaTime);
        Debug.DrawLine(position, new Vector3(position.x, position.y + size, position.z), Color.red, Time.deltaTime);
        Debug.DrawLine(new Vector3(position.x, position.y + size, position.z), new Vector3(position.x + size, position.y + size, position.z), Color.red, Time.deltaTime);
        Debug.DrawLine(new Vector3(position.x + size, position.y + size, position.z), new Vector3(position.x + size, position.y, position.z), Color.red, Time.deltaTime);
    }
}
