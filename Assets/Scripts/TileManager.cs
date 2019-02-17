using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TileIndex = System.Tuple<int, int>;

public class TileManager : MonoBehaviour
{
    [SerializeField] public GameObject prefabMars;
    [SerializeField] public GameObject prefabMercury;
    [SerializeField] public GameObject prefabNeptune;
    [SerializeField] public GameObject prefabOrange;
    [SerializeField] public GameObject prefabUranus;
    [SerializeField] public GameObject prefabWorstPlanet;
    [SerializeField] public GameObject prefabRedSun;
    [SerializeField] public GameObject prefabBlackHole;
    [SerializeField] public float minDistanceBetweenScenery;
    [SerializeField] public int minTerrainPerTile;
    [SerializeField] public int maxTerrainPerTile;
    private static readonly float TILE_WIDTH = 50.0f;
    private static readonly float TILE_HEIGHT = 30.0f;
    private Dictionary<TileIndex, List<GameObject>> tiles = new Dictionary<TileIndex, List<GameObject>>();
    private List<GameObject> prefabs = new List<GameObject>();
    private GameObject ship;

    private void Awake()
    {
        prefabs.Add(prefabMars);
        prefabs.Add(prefabMercury);
        prefabs.Add(prefabNeptune);
        prefabs.Add(prefabOrange);
        prefabs.Add(prefabUranus);
        prefabs.Add(prefabWorstPlanet);
        prefabs.Add(prefabRedSun);
        prefabs.Add(prefabBlackHole);
    }

    void Start()
    {
        ship = GameObject.FindGameObjectWithTag("Player");
        if (ship)
        {
            Vector3 shipPos = ship.transform.position;
            TileIndex startingTileIndex = GetTileIndex(shipPos.x, shipPos.y);
            GenerateSurroundingTiles(startingTileIndex);
        }
    }

    void Update()
    {
        if (ship)
        {
            Vector3 shipPos = ship.transform.position;
            TileIndex tileIndex = GetTileIndex(shipPos.x, shipPos.y);
            GenerateSurroundingTiles(tileIndex);
        }

        if (Input.GetButtonDown("reset"))
        {
            foreach (List<GameObject> terrainList in tiles.Values)
            {
                foreach (GameObject terrain in terrainList)
                {
                    Destroy(terrain);
                }
            }
            tiles.Clear();
            if (ship)
            {
                Vector3 shipPos = ship.transform.position;
                TileIndex tileIndex = GetTileIndex(shipPos.x, shipPos.y);
                GenerateSurroundingTiles(tileIndex);
            }
        }
    }

    private TileIndex GetTileIndex(float x, float y)
    {
        return new TileIndex(Mathf.FloorToInt(x / TILE_WIDTH), Mathf.FloorToInt(y / TILE_HEIGHT));
    }

    private void GenerateSurroundingTiles(TileIndex index)
    {
        // System.Random works on the scale of the system clock so any tiles
        // created in this loop would get the same pseudorandom sequence
        // if they didn't share the same Random instance
        System.Random random = new System.Random();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                GenerateTileIfNotExists(new TileIndex(index.Item1 + i, index.Item2 + j), random);
            }
        }
    }

    private void GenerateTileIfNotExists(TileIndex index, System.Random random)
    {
        if (!tiles.ContainsKey(index))
        {
            tiles.Add(index, GenerateTileTerrain(index, random));
        }
    }

    private List<GameObject> GenerateTileTerrain(TileIndex tile, System.Random random)
    {
        List<GameObject> tileTerrain = new List<GameObject>();
        int numScenery = random.Next(minTerrainPerTile, maxTerrainPerTile + 1);
        for (int i = 0; i < numScenery; i++)
        {
            GameObject terrain = PlaceTerrain(tile, random);
            if (terrain)
            {
                tileTerrain.Add(terrain);
            }
        }
        return tileTerrain;
    }

    private GameObject PlaceTerrain(TileIndex index, System.Random random)
    {
        int maxX = (int)TILE_WIDTH / 2;
        int maxY = (int)TILE_HEIGHT / 2;
        Vector2 offset = GenerateRandomOffset(random, maxX, maxY);
        Vector2 absPos = GetTileCentre(index) + offset;

        List<GameObject> nearbyTerrain = GetTerrainFromSurroundingTiles(index);
        if (ship)
        {
            nearbyTerrain.Add(ship);
        }

        bool isPosGood = true;
        int iterations = 0;

        GameObject planetToSpawn = prefabs[random.Next(prefabs.Count)];
        CircleCollider2D us = planetToSpawn.GetComponent<CircleCollider2D>();

        do
        {
            isPosGood = true;
            iterations += 1;
            if (iterations > 10)
            {
                // if we can't find a place for it after a while, just give up
                return null;
            }

            foreach (GameObject sceneryObject in nearbyTerrain)
            {
                float buffer = minDistanceBetweenScenery;
                buffer += us.radius * us.gameObject.transform.localScale.x;

                CircleCollider2D them = sceneryObject.GetComponent<CircleCollider2D>();
                if (us && them)
                {
                    buffer += them.radius * them.gameObject.transform.localScale.x;
                }

                if (Vector2.Distance(sceneryObject.transform.position, absPos) < buffer)
                {
                    isPosGood = false;
                }
            }
            if (!isPosGood)
            {
                offset = GenerateRandomOffset(random, maxX, maxY);
                absPos = GetTileCentre(index) + offset;
            }
        } while (!isPosGood);

        int choice = random.Next(prefabs.Count);
        return Instantiate(planetToSpawn, absPos, Quaternion.identity);
    }

    private Vector2 GenerateRandomOffset(System.Random random, int maxX, int maxY)
    {
        bool isXNeg = false;
        if (random.Next(2) == 1)
        {
            isXNeg = true;

        }

        bool isYNeg = false;
        if (random.Next(2) == 1)
        {
            isYNeg = true;
        }

        int xOffset = random.Next(maxX);
        int yOffset = random.Next(maxY);
        if (isXNeg)
        {
            xOffset *= -1;
        }
        if (isYNeg)
        {
            yOffset *= -1;
        }

        return new Vector2(xOffset, yOffset);
    }

    private Vector2 GetTileCentre(TileIndex index)
    {
        return new Vector2(index.Item1 * TILE_WIDTH + TILE_WIDTH / 2, index.Item2 * TILE_HEIGHT + TILE_HEIGHT / 2);
    }

    private List<GameObject> GetTerrainFromSurroundingTiles(TileIndex index)
    {
        List<GameObject> terrain = new List<GameObject>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                TileIndex neighbour = new TileIndex(index.Item1 + i, index.Item2 + j);
                if (tiles.ContainsKey(neighbour))
                {
                    foreach (GameObject neighbouringTerrain in tiles[neighbour])
                    {
                        terrain.Add(neighbouringTerrain);
                    }
                }
            }
        }
        return terrain;
    }
}
