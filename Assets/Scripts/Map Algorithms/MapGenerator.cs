using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private string mapFileName = "map.txt";
    [SerializeField] private float cellSize = 2f;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // Define the path to your map file
        string filePath = Path.Combine(Application.streamingAssetsPath, mapFileName);

        if (File.Exists(filePath))
        {
            // Read all lines from the map file
            string[] lines = File.ReadAllLines(filePath);
            int mapHeight = lines.Length;
            int mapWidth = 0;
            List<List<string>> mapData = new List<List<string>>();

            // Parse the map data
            for (int y = 0; y < mapHeight; y++)
            {
                string line = lines[y].Trim();

                // Remove leading '(' and trailing ')'
                if (line.StartsWith("(")) line = line.Substring(1);
                if (line.EndsWith(")")) line = line.Substring(0, line.Length - 1);

                // Split the line into cells
                string[] cells = line.Split(new string[] { ")(" }, System.StringSplitOptions.None);
                List<string> row = new List<string>(cells);

                mapData.Add(row);

                if (row.Count > mapWidth)
                    mapWidth = row.Count;
            }

            // Generate cells based on the map data
            for (int y = 0; y < mapHeight; y++)
            {
                List<string> row = mapData[y];
                for (int x = 0; x < mapWidth; x++)
                {
                    if (x >= row.Count)
                        continue;

                    string cellData = row[x];

                    if (cellData == "#")
                        continue; // Skip empty spaces

                    // Calculate cell position
                    Vector3 position = new Vector3(x * cellSize, 0, (mapHeight - y - 1) * cellSize);

                    // Instantiate the cell prefab
                    GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, transform);

                    // Get the Cell script component
                    Cell cellScript = cellObj.GetComponent<Cell>();

                    // Determine wall activation based on cell data
                    bool floorActive = true;
                    bool topWallActive = false;
                    bool bottomWallActive = false;
                    bool leftWallActive = false;
                    bool rightWallActive = false;

                    if (cellData == "F")
                    {
                        // Only the floor is active
                    }
                    else if (cellData == "O")
                    {
                        // All walls are active
                        topWallActive = bottomWallActive = leftWallActive = rightWallActive = true;
                    }
                    else
                    {
                        // Activate specific walls
                        if (cellData.Contains("T"))
                            topWallActive = true;
                        if (cellData.Contains("B"))
                            bottomWallActive = true;
                        if (cellData.Contains("L"))
                            leftWallActive = true;
                        if (cellData.Contains("R"))
                            rightWallActive = true;
                    }

                    // Initialize the cell with the determined wall states
                    cellScript.Init(floorActive, topWallActive, bottomWallActive, leftWallActive, rightWallActive);
                }
            }
        }
        else
        {
            Debug.LogError("Map file not found at: " + filePath);
        }
    }
}
