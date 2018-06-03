using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    #region Fields & Properties 

    [SerializeField]
    private GameObject areas;


    private GameObject Areas
    {
        get
        {
            if (areas == null)
            {
                areas = GameObject.Find("Areas");
            }

            return areas;
        }
    }


    [SerializeField]
    private GameObject[] tilePrefabs;


    public Dictionary<Point, Tile> Tiles { get; set; }


    public float TileSize
    {
        get { return tilePrefabs[1].GetComponent<MeshRenderer>().bounds.size.x; }
    }


    [SerializeField]
    private Vector3 worldStart = new Vector3(-32.5f, 0, 32.5f);


    [SerializeField]
    private bool genRandom = false;


    [SerializeField]
    private int ranH = 10;


    [SerializeField]
    private int ranW = 10;


    [SerializeField]
    private bool hasOffset;


    [SerializeField]
    private float offset;


    [Space(20)]
    [TextArea(8, 20)]
    [SerializeField]
    private string level;


    [SerializeField]
    private bool updateMap = false;


    [SerializeField]
    private bool resetAreaTransform = false;


    private Dictionary<string, GameObject> allAreas;

    #endregion

    public void GenerateLevel()
    {
        allAreas = new Dictionary<string, GameObject>();

        foreach (GameObject go in tilePrefabs)
        {
            var area = new GameObject(go.name + "s");
            area.transform.SetParent(Areas.transform);
            allAreas.Add(go.name, area);
        }

        CreateLevel();

        if (resetAreaTransform)
        {
            ResetAreas();
        }
    }


    private void Update()
    {
        if (updateMap)
        {
            CreateLevel();
        }

        if (resetAreaTransform)
        {
            ResetAreas();
            resetAreaTransform = false;
        }
    }


    private void ResetAreas()
    {
        foreach (KeyValuePair<string, GameObject> area in allAreas)
        {
            area.Value.transform.localPosition = Vector3.zero;
        }
    }


    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, Tile>();

        if (updateMap)
        {
            DeletePreviouslyGeneratedMap();
        }

        string[] mapData = genRandom ? GenerateRandom(ranH, ranW, 0, 3) : ReadLevelString();

        int mapX = mapData[0].Length;
        int mapZ = mapData.Length;

        for (int z = 0; z < mapZ; z++)
        {
            char[] newTiles = mapData[z].ToCharArray();

            for (int x = 0; x < mapX; x++)
            {
                if (char.IsLetter(newTiles[x]))
                {
                    PlaceTile(System.Convert.ToInt32(newTiles[x].ToString(), 16).ToString(), x, z, worldStart);
                }
                else
                {
                    if (newTiles[x] != '*')
                    {
                        PlaceTile(newTiles[x].ToString(), x, z, worldStart);
                    }
                }
            }
        }
    }


    private void DeletePreviouslyGeneratedMap()
    {
        // Delete previous level
        var children = new List<GameObject>();

        // Add all of the generated objects to a list to be deleted
        foreach (KeyValuePair<string, GameObject> area in allAreas)
        {
            foreach (Transform child in area.Value.transform)
                children.Add(child.gameObject);
        }

        children.ForEach(child => Destroy(child));
        updateMap = false;
    }


    private void PlaceTile(string tileType, int x, int z, Vector3 worldStart)
    {
        int typeIndex = int.Parse(tileType);

        Tile newTile = Instantiate(tilePrefabs[typeIndex]).GetComponent<Tile>();

        if (newTile.name.Contains("("))
            newTile.name = newTile.name.Split('(')[0];

        GameObject tileArea = allAreas[newTile.name];
        newTile.transform.SetParent(tileArea.transform);

        newTile.Setup(new Point(x, z), new Vector3(worldStart.x + (TileSize * x), 0, worldStart.z - (TileSize * z) + (hasOffset ? offset : 0)));
        newTile.TileID = tileType;

        Tiles.Add(new Point(x, z), newTile);
    }


    private string[] ReadLevelString()
    {
        string data = level.Replace(System.Environment.NewLine, string.Empty);
        return data.Split('-');
    }


    private string[] GenerateRandom(int height, int width, int tile, int moreChance)
    {
        string[] map = new string[height];
        string random;

        for (int i = 0; i < height; i++)
        {
            random = "";

            for (int j = 0; j < width; j++)
            {
                int ran = Random.Range(0, tilePrefabs.Length + moreChance);
                ran = ran >= tilePrefabs.Length ? tile : ran;
                random += "" + ran;
            }

            map[i] = random;
        }
        return map;
    }
}
