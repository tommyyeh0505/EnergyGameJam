using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TileIndex = System.Tuple<int, int>;

public class TileManager : MonoBehaviour
{
    [SerializeField] public GameObject prefabMars;
    [SerializeField] public GameObject prefabMercury;
    [SerializeField] public GameObject prefabNeptune;
    [SerializeField] public GameObject prefabOrangee;
    [SerializeField] public GameObject prefabUranus;
    [SerializeField] public GameObject prefabWorstPlanet;
    [SerializeField] public GameObject prefabRedSun;
    [SerializeField] public GameObject prefabTile;
    private static readonly float TILE_WIDTH = 50.0f;
    private static readonly float TILE_HEIGHT = 30.0f;
    private Dictionary<TileIndex, TileBehaviourComponent> tiles = new Dictionary<TileIndex, TileBehaviourComponent>();
    private GameObject ship;

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
    }

    private TileIndex GetTileIndex(float x, float y)
    {
        return new TileIndex(Mathf.FloorToInt(x / TILE_WIDTH), Mathf.FloorToInt(y / TILE_HEIGHT));
    }

    private void GenerateSurroundingTiles(TileIndex index)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                GenerateTileIfNotExists(new TileIndex(index.Item1 + i, index.Item2 + j));
            }
        }
    }

    private void GenerateTileIfNotExists(TileIndex index)
    {
        if (!tiles.ContainsKey(index))
        {
            Vector2 pos = new Vector2(index.Item1 * TILE_WIDTH + TILE_WIDTH / 2, index.Item2 * TILE_HEIGHT + TILE_HEIGHT / 2);
            GameObject tile = Instantiate(prefabTile, pos, Quaternion.identity);
            TileBehaviourComponent tileBehaviour = tile.GetComponent<TileBehaviourComponent>();
            if (tileBehaviour)
            {
                tiles.Add(index, tileBehaviour);
                Debug.Log("Added tile at " + index);
            }
        }
    }
}
