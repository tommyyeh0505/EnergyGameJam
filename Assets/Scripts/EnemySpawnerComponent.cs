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
    private int NumEnemies;
    private float[] SpawnBoundaries; //[Top, Bottom, Left, Right]

    [SerializeField] List<GameObject> Enemies;
    [SerializeField] float EnemiesPerSecond;
    [SerializeField] int MaxEnemiesOnMap;
    [SerializeField] float EnemySpawnGrowthFactor;
    [SerializeField] float SpawnBoundaryPadding; //How much padding around camera for enemy spawn
    private List<OccupiedSpace> EntityPositions; //List of all entities that take up space (Bottom Left Corner, Top Right Corner)
    [SerializeField] float LargestEntityRadius; //Largest used to eliminate risk of overlap
    private float OrthSize;
    //For Timing Enemy Spawn
    private float nextActionTime = 0.0f;
    [SerializeField] float period;

    // Start is called before the first frame update
    void Start()
    {
        NumEnemies = 0;
        CameraHeight = 2f * OrthSize;
        CameraWidth = Camera.main.aspect * OrthSize;
        SpawnBoundaries = new float[4];
        UpdateCamera();
        nextActionTime = Time.time;
    }

    private void UpdateCamera()
    {
        CameraPos = Camera.main.transform.position;
        OrthSize = Camera.main.orthographicSize;
        //Debug.Log("CameraLocation: " + CameraPos.ToString());
        //Debug.Log("Height: " + CameraHeight);
        //Debug.Log("Width: " + CameraWidth);
        SpawnBoundaries[0] = CameraPos.y + OrthSize + SpawnBoundaryPadding;
        SpawnBoundaries[1] = CameraPos.y - OrthSize - SpawnBoundaryPadding;
        SpawnBoundaries[2] = CameraPos.x - (OrthSize * Camera.main.aspect) - SpawnBoundaryPadding;
        SpawnBoundaries[3] = CameraPos.x + (OrthSize * Camera.main.aspect) + SpawnBoundaryPadding;
        Debug.DrawLine(new Vector3(SpawnBoundaries[2], SpawnBoundaries[1], 0), new Vector3(SpawnBoundaries[3], SpawnBoundaries[0], 0), Color.white, 10.0f);
    }

    public int SpawnEnemy(string EnemyType, Vector3 location)
    {   
        Instantiate(Enemies[0], location, Quaternion.identity);
        NumEnemies++;
        return 0;
    }
    // Update is called once per frame
    void Update()
    {
        EntityPositions = new List<OccupiedSpace>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("OccupiesSpace");
        //Debug.Log("Number of Objects occupying space: " + objects.Length);
        for(int i = 0; i < objects.Length; i++)
        {
            Vector3 pos = objects[i].transform.position;
            //Debug.Log("Space occupied: " + pos.ToString());
            Vector3 bl = new Vector3(pos.x - LargestEntityRadius, pos.y - LargestEntityRadius, 0.0f);
            Vector3 tr = new Vector3(pos.x + LargestEntityRadius, pos.y + LargestEntityRadius, 0.0f);
            EntityPositions.Add(new OccupiedSpace(bl, tr));
            DebugDrawColoredRectangle(bl, 2 * LargestEntityRadius);
        }

        //Spawns Enemy
        //TODO add timers and enemytypes

        if (Time.time > nextActionTime && NumEnemies < MaxEnemiesOnMap)
        {
            nextActionTime += period;
            UpdateCamera();
            RandomSpawn();
        }        
    }

    void DebugDrawColoredRectangle(Vector3 position, float size)
    {
        Debug.DrawLine(position, new Vector3(position.x + size, position.y, position.z), Color.red, Time.deltaTime);
        Debug.DrawLine(position, new Vector3(position.x, position.y + size, position.z), Color.red, Time.deltaTime);
        Debug.DrawLine(new Vector3(position.x, position.y + size, position.z), new Vector3(position.x + size, position.y + size, position.z), Color.red, Time.deltaTime);
        Debug.DrawLine(new Vector3(position.x + size, position.y + size, position.z), new Vector3(position.x + size, position.y, position.z), Color.red, Time.deltaTime);
    }

    public void EnemyDied()
    {
        NumEnemies = Mathf.Clamp(NumEnemies - 1, 0, MaxEnemiesOnMap);
    }

    public int RandomSpawn()
    {
        System.Random rnd = new System.Random();
        int side = rnd.Next(1, 5);
        float x;
        float y;
        Vector3 spawnpos;
        bool clear;
        do
        {
            clear = true;
            switch (side)
            {
                case 1: //top
                    x = Random.Range(SpawnBoundaries[2], SpawnBoundaries[3]);
                    y = SpawnBoundaries[0];
                    break;
                case 2: //bottom
                    x = Random.Range(SpawnBoundaries[2], SpawnBoundaries[3]);
                    y = SpawnBoundaries[1];
                    break;
                case 3: //left
                    x = SpawnBoundaries[2];
                    y = Random.Range(SpawnBoundaries[1], SpawnBoundaries[0]);
                    break;
                default: //right
                    x = SpawnBoundaries[3];
                    y = Random.Range(SpawnBoundaries[1], SpawnBoundaries[0]);
                    break;
            }
            spawnpos = new Vector3(x, y, 0.0f);
            Debug.Log("Spawning Enemies at position: " + spawnpos.ToString());

            for (int i = 0; i < EntityPositions.Count; i++)
            {
                //if inside square, dont spawn
                if (EntityPositions[i].bottomleft.x - LargestEntityRadius < x && EntityPositions[i].topright.x + LargestEntityRadius > x 
                    && EntityPositions[i].bottomleft.y - LargestEntityRadius < y && EntityPositions[i].topright.y + LargestEntityRadius > y)
                {
                    clear = false;
                }
            }
        } while (!clear);
        return SpawnEnemy("BasicEnemy", spawnpos);
    }
}
