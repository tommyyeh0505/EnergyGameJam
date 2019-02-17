using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviourComponent : MonoBehaviour
{
    [SerializeField] public GameObject prefabMars;
    [SerializeField] public GameObject prefabMercury;
    [SerializeField] public GameObject prefabNeptune;
    [SerializeField] public GameObject prefabOrange;
    [SerializeField] public GameObject prefabUranus;
    [SerializeField] public GameObject prefabWorstPlanet;
    [SerializeField] public GameObject prefabRedSun;
    [SerializeField] public float minDistanceBetweenScenery;
    [SerializeField] public int minNumTerrain = 2;
    [SerializeField] public int maxNumTerrain = 4;
    private List<GameObject> prefabs = new List<GameObject>();
    private List<GameObject> scenery = new List<GameObject>();

    private void Awake()
    {
        prefabs.Add(prefabMars);
        prefabs.Add(prefabMercury);
        prefabs.Add(prefabNeptune);
        prefabs.Add(prefabOrange);
        prefabs.Add(prefabUranus);
        prefabs.Add(prefabWorstPlanet);
        prefabs.Add(prefabRedSun);
    }

    public void GenerateTerrain(System.Random random, float width, float height)
    {
        int numScenery = random.Next(minNumTerrain, maxNumTerrain + 1);
        for (int i = 0; i < numScenery; i++)
        {
            PlaceRandomSceneryObject(random, width, height);
        }
    }

    private void PlaceRandomSceneryObject(System.Random random, float width, float height)
    {
        // look at this sa-pa-ghetti code
        int maxX = (int)width / 2;
        int maxY = (int)height / 2;
        Vector2 randPos = GenerateRandomPosition(random, maxX, maxY);

        bool isPosGood = true;
        int iterations = 0;
        do
        {
            isPosGood = true;
            iterations += 1;
            if (iterations > 10)
            {
                // if we can't find a place for it after a while, just give up
                return;
            }
            foreach (GameObject sceneryObject in scenery)
            {
                if (Vector2.Distance(sceneryObject.transform.position, randPos) < minDistanceBetweenScenery)
                {
                    isPosGood = false;
                }
            }
            if (!isPosGood)
            {
                randPos = GenerateRandomPosition(random, maxX, maxY);
            }
        } while (!isPosGood);

        int choice = random.Next(prefabs.Count);
        scenery.Add(Instantiate(prefabs[choice], randPos, Quaternion.identity));
    }

    private Vector2 GenerateRandomPosition(System.Random random, int maxX, int maxY)
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

        Vector2 offset = new Vector2(xOffset, yOffset);
        Vector2 tilePos = new Vector2(transform.position.x, transform.position.y);
        return tilePos + offset;
    }
}
