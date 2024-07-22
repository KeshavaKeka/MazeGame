using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeGenerator : MonoBehaviour
{
    public NavMeshSurface surface;

    public int width = 13;
    public int height = 20;

    public GameObject wall;
    public GameObject enemy; // Add a reference to the enemy prefab

    private int[,] maze;
    private Stack<Vector2Int> stack = new Stack<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
        SpawnEnemies(5,8f);
        surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
        else
        {
            Debug.Log("Surface not assigned");
        }
    }

    void GenerateMaze()
    {
        maze = new int[width, height];

        // Initialize the maze with walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1;
            }
        }

        // Ensure the starting points are not blocked
        maze[1, 10] = 0;
        maze[2, 10] = 0;

        Vector2Int start = new Vector2Int(1, 10);
        stack.Push(start);
        maze[start.x, start.y] = 0;

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count > 0)
            {
                Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];
                maze[chosen.x, chosen.y] = 0;
                maze[(current.x + chosen.x) / 2, (current.y + chosen.y) / 2] = 0;
                stack.Push(chosen);
            }
            else
            {
                stack.Pop();
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1 && HasAdjacentWall(x, y) && Random.value > 0.4f)  // Increase wall density slightly
                {
                    maze[x, y] = 1;
                }
                else if (maze[x, y] == 1)
                {
                    maze[x, y] = 0;
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    Vector3 pos = new Vector3(x - width / 2f + 0.5f, 0.85f, y - height / 2f + 1);
                    Instantiate(wall, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (cell.x > 1 && maze[cell.x - 2, cell.y] == 1)
            neighbors.Add(new Vector2Int(cell.x - 2, cell.y));
        if (cell.x < width - 2 && maze[cell.x + 2, cell.y] == 1)
            neighbors.Add(new Vector2Int(cell.x + 2, cell.y));
        if (cell.y > 1 && maze[cell.x, cell.y - 2] == 1)
            neighbors.Add(new Vector2Int(cell.x, cell.y - 2));
        if (cell.y < height - 2 && maze[cell.x, cell.y + 2] == 1)
            neighbors.Add(new Vector2Int(cell.x, cell.y + 2));

        return neighbors;
    }

    bool HasAdjacentWall(int x, int y)
    {
        return (x > 0 && maze[x - 1, y] == 1) ||
               (x < width - 1 && maze[x + 1, y] == 1) ||
               (y > 0 && maze[x, y - 1] == 1) ||
               (y < height - 1 && maze[x, y + 1] == 1);
    }

    void SpawnEnemies(int count, float minDistance)
    {
        List<Vector2Int> openPositions = new List<Vector2Int>();
        List<Vector3> spawnedEnemies = new List<Vector3>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 6; y < height - 2; y++)
            {
                if (maze[x, y] == 0)
                {
                    openPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (openPositions.Count == 0) break;

            Vector2Int pos = GetValidSpawnPosition(openPositions, spawnedEnemies, minDistance);
            if (pos == Vector2Int.zero) break; // No valid position found

            Vector3 spawnPos = new Vector3(pos.x - width / 2f + 0.5f, 0.85f, pos.y - height / 2f + 1);
            spawnedEnemies.Add(spawnPos);
            Instantiate(enemy, spawnPos, Quaternion.identity, transform);
        }
    }

    Vector2Int GetValidSpawnPosition(List<Vector2Int> openPositions, List<Vector3> spawnedEnemies, float minDistance)
    {
        for (int attempts = 0; attempts < openPositions.Count; attempts++)
        {
            int index = Random.Range(0, openPositions.Count);
            Vector2Int pos = openPositions[index];
            Vector3 spawnPos = new Vector3(pos.x - width / 2f + 0.5f, 0.85f, pos.y - height / 2f + 1);

            bool valid = true;
            foreach (Vector3 enemyPos in spawnedEnemies)
            {
                if (Vector3.Distance(spawnPos, enemyPos) < minDistance)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                openPositions.RemoveAt(index);
                return pos;
            }
        }

        return Vector2Int.zero; // No valid position found
    }
}