using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelController : MonoBehaviour
{
    [Header("World Size")]
    public float tileWidth;
    public float tileLength;
    public List<GameObject> tilePrefabs;
    public List<GameObject> activeTiles;
    public GameObject startTile;
    // Start is called before the first frame update

    [Header("Navigation")]
    private NavMeshSurface surface;
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        BuildWorld();
    }
    void Start()
    {
        
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildWorld()
    {
        for (int width = 0; width < tileWidth; width++)
        {
            for (int length = 0; length < tileLength; length++)
            {
                if(activeTiles.Count < 1)
                {
                    activeTiles.Add(startTile);
                    continue;
                }
                var randomTileIndex = Random.Range(0, tilePrefabs.Count);
                var randomTilePosition = new Vector3(width * 16, 0.0f, length * 16);
                var randomTileRotation = Random.Range(0, 4) * 90.0f;
                var randomTile = Instantiate(tilePrefabs[randomTileIndex], randomTilePosition, Quaternion.Euler(0.0f, randomTileRotation, 0.0f));
                randomTile.transform.parent = this.transform; //make this GameObject the level parent
                activeTiles.Add(randomTile);

            }
        }
    }
}
