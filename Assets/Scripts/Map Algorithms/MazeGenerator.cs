using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    [Range(5, 100)]
    public int mazeWidth = 10;
    [Range(5, 100)]
    public int mazeHeight = 10;
    public int startX = 0;
    public int startY = 0;

    [Header("Cell Settings")]
    [SerializeField] private GameObject cellPrefab;
    public float cellSize = 2f;

    private MazeCell[,] maze;

    void Start()
    {
        GenerateMaze();
        RenderMaze();
    }

    void GenerateMaze()
    {
        // Initialize the maze grid
        maze = new MazeCell[mazeWidth, mazeHeight];
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }

        // Start the maze generation
        Stack<Vector2Int> cellStack = new Stack<Vector2Int>();
        Vector2Int currentCell = new Vector2Int(startX, startY);
        maze[currentCell.x, currentCell.y].visited = true;
        cellStack.Push(currentCell);

        while (cellStack.Count > 0)
        {
            // Get unvisited neighbors
            List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(currentCell);

            if (unvisitedNeighbors.Count > 0)
            {
                // Choose a random neighbor
                Vector2Int chosenCell = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];

                // Remove wall between current cell and chosen cell
                RemoveWalls(currentCell, chosenCell);

                // Mark chosen cell as visited and push onto stack
                maze[chosenCell.x, chosenCell.y].visited = true;
                cellStack.Push(chosenCell);

                // Move to chosen cell
                currentCell = chosenCell;
            }
            else
            {
                // Backtrack
                currentCell = cellStack.Pop();
            }
        }
    }

    void RenderMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                // Calculate position
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);

                // Instantiate cell prefab
                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, transform);

                // Get Cell script component
                Cell cellScript = cellObj.GetComponent<Cell>();

                // Get the corresponding MazeCell
                MazeCell mazeCell = maze[x, y];

                // Initialize the cell with wall states
                cellScript.Init(
                    floorActive: true,
                    top: mazeCell.topWall,
                    bottom: mazeCell.bottomWall,
                    left: mazeCell.leftWall,
                    right: mazeCell.rightWall
                );
            }
        }
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Up
        if (cell.y + 1 < mazeHeight && !maze[cell.x, cell.y + 1].visited)
            neighbors.Add(new Vector2Int(cell.x, cell.y + 1));

        // Down
        if (cell.y - 1 >= 0 && !maze[cell.x, cell.y - 1].visited)
            neighbors.Add(new Vector2Int(cell.x, cell.y - 1));

        // Right
        if (cell.x + 1 < mazeWidth && !maze[cell.x + 1, cell.y].visited)
            neighbors.Add(new Vector2Int(cell.x + 1, cell.y));

        // Left
        if (cell.x - 1 >= 0 && !maze[cell.x - 1, cell.y].visited)
            neighbors.Add(new Vector2Int(cell.x - 1, cell.y));

        return neighbors;
    }

    private void RemoveWalls(Vector2Int current, Vector2Int next)
    {
        Vector2Int delta = next - current;

        if (delta.x == 1)
        {
            // Next cell is to the right of current
            maze[current.x, current.y].rightWall = false;
            maze[next.x, next.y].leftWall = false;
        }
        else if (delta.x == -1)
        {
            // Next cell is to the left of current
            maze[current.x, current.y].leftWall = false;
            maze[next.x, next.y].rightWall = false;
        }
        else if (delta.y == 1)
        {
            // Next cell is above current
            maze[current.x, current.y].topWall = false;
            maze[next.x, next.y].bottomWall = false;
        }
        else if (delta.y == -1)
        {
            // Next cell is below current
            maze[current.x, current.y].bottomWall = false;
            maze[next.x, next.y].topWall = false;
        }
    }

    // Nested MazeCell class
    private class MazeCell
    {
        public bool visited = false;
        public bool topWall = true;
        public bool bottomWall = true;
        public bool leftWall = true;
        public bool rightWall = true;
        public int x, y;

        public MazeCell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
