using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGeneration : MonoBehaviour
{
    [Header("i4")]
    [Range(10, 20)] public int width = 10;
    [Range(10, 14)] public int height = 10;
    [Range(0.5f, 2f)] public float cellSize = 1f;

    public GameObject wallPrefab;
    public Transform holer;
    public float timer = 0;
    private int[,] maze;
    private List<Vector2> directions = new List<Vector2> {
        new Vector2(0, 2), new Vector2(2, 0),
        new Vector2(0, -2), new Vector2(-2, 0)
    };
    Queue<Vector2> que = new Queue<Vector2>();
    List<GameObject> wallPrefabs = new List<GameObject>();
    void Start()
    {
        GenerateMaze();
    }
    /* private void Update()
     {
         timer += Time.deltaTime;
         DrawMaze();
     }*/
    private void Update()
    {
        DrawMaze();
    }
    void GenerateMaze()
    {
        maze = new int[width, height];
        Stack<Vector2> stack = new Stack<Vector2>();
        Vector2 startCell = new Vector2(Random.Range(0, width / 2) * 2, Random.Range(0, height / 2) * 2);
        stack.Push(startCell);
        maze[(int)startCell.x, (int)startCell.y] = 1;
        que.Enqueue(new Vector2((int)startCell.x, (int)startCell.y));
        while (stack.Count > 0)
        {
            Vector2 currentCell = stack.Pop();
            List<Vector2> unvisitedNeighbors = new List<Vector2>();

            foreach (var direction in directions)
            {
                Vector2 neighbor = currentCell + direction;
                if (IsInBounds(neighbor) && maze[(int)neighbor.x, (int)neighbor.y] == 0)
                {
                    unvisitedNeighbors.Add(neighbor);
                }
            }

            if (unvisitedNeighbors.Count > 0)
            {
                stack.Push(currentCell);
                Vector2 chosenNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                Vector2 wallBetween = (currentCell + chosenNeighbor) / 2;
                maze[(int)wallBetween.x, (int)wallBetween.y] = 1;
                maze[(int)chosenNeighbor.x, (int)chosenNeighbor.y] = 1;
                que.Enqueue(new Vector2((int)wallBetween.x, (int)wallBetween.y));
                que.Enqueue(new Vector2((int)chosenNeighbor.x, (int)chosenNeighbor.y));
                stack.Push(chosenNeighbor);
            }
        }
    }

    void DrawMaze()
    {
        /*  if (timer>0.1 && !(que.Count==0))
          {
              timer = 0f;
              Vector2 vec = que.Dequeue();
              Vector2 vecSpawn= (vec + new Vector2(-width/2, -height/2)) * new Vector2(cellSize,cellSize);
              GameObject t = Instantiate(wallPrefab,vecSpawn,Quaternion.identity);
              t.transform.SetParent(holer);
              wallPrefabs.Add(t);
          }*/
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (maze[i, j] == 1)
                {
                    Vector2 vecSpawn = (new Vector2(i, j) + new Vector2(-width / 2, -height / 2)) * new Vector2(cellSize, cellSize);
                    GameObject t = Instantiate(wallPrefab, vecSpawn, Quaternion.identity);
                    t.transform.SetParent(holer);
                    wallPrefabs.Add(t);
                }

            }
        }
    }

    bool IsInBounds(Vector2 pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }
    public void Reset()
    {
        foreach (GameObject t in wallPrefabs)
        {
            Destroy(t);
        }
        wallPrefabs.Clear();
        GenerateMaze();
        DrawMaze();
    }
}