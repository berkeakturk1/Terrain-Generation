using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using Random = System.Random;

public class Grid : MonoBehaviour
{
    [SerializeField] private int size;
    Tile[,] tiles = new Tile[25, 25];
    [SerializeField] Tile[] tile_types;
    
    [SerializeField] private Tile startTile;
    void Start()
    {
        SetPositions();
        StartCoroutine(TraverseGridBFS());
    }

    public void button()
    {
        DeleteAllTiles();
        SetPositions();
        StartCoroutine(TraverseGridBFS());
        
    }

    
    public void DeleteAllTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
    }

    public void SetPositions()
    {
        
        float startX = 0, startY = 0; // Starting positions, adjust as needed
        float xOffset = 0.16f, yOffset = 0.16f; // Offset for each tile, adjust as needed

        for (int i = 0; i < tiles.GetLength(0); i++) // Iterate through rows
        {
            float x = startX; // Reset x-coordinate to starting value for each row
            for (int j = 0; j < tiles.GetLength(1); j++) // Iterate through columns
            {
                Tile tileToInstantiate = tile_types[0];
                Vector3 position = new Vector3(x, startY, 0);
                Tile instantiatedTile = Instantiate(tileToInstantiate, position, Quaternion.identity);
                tiles[i, j] = instantiatedTile; // Store the reference to the instantiated tile
                tiles[i, j].setPosition(x, startY);
                x += xOffset; // Move to the next column position
            }
            startY -= yOffset; // Move to the next row position
        }
    }


    public void SetSprites(Vector2Int index)
    {
        // Process the current tile
        Tile tileToReplace = tiles[index.x, index.y];
        
        Vector3 position = new Vector3(tileToReplace.getX(), tileToReplace.getY(), 0);

        // Destroy the old tile
        Destroy(tileToReplace.gameObject); // Make sure to destroy the GameObject, not just the Tile component

        // Check if selectable_tiles has elements and the type index is valid
        int tileType = tiles[index.x, index.y].type;
        Tile curr = tiles[index.x, index.y];
        printTiles(curr.selectable_tiles);
        
        Tile newTile = Instantiate(tile_types[tileType], position, Quaternion.identity);

            // Update the reference in the tiles array
        tiles[index.x, index.y] = newTile;
        
       
    }



    
    

    public IEnumerator TraverseGridBFS()
    {
        bool[,] visited = new bool[25, 25];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int start = new Vector2Int(12, 12);

        InitializeTile(start.x, start.y, 1);
        queue.Enqueue(start);
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            yield return new WaitForSeconds(0.02f); // Wait for 2 seconds
            SetSprites(current);

            EnqueueIfNotVisited(current.x, current.y, current.x + 1, current.y, visited, queue); // Right
            EnqueueIfNotVisited(current.x, current.y, current.x, current.y + 1, visited, queue); // Down
            EnqueueIfNotVisited(current.x, current.y, current.x - 1, current.y, visited, queue); // Left
            EnqueueIfNotVisited(current.x, current.y, current.x, current.y - 1, visited, queue); // Up
        }
    }



    void EnqueueIfNotVisited(int x_prev, int y_prev, int x, int y, bool[,] visited, Queue<Vector2Int> queue)
    {
        if (x >= 0 && y >= 0 && x < tiles.GetLength(0) && y < tiles.GetLength(1) && !visited[x, y])
        {
            queue.Enqueue(new Vector2Int(x, y));
            visited[x, y] = true;

           setSelectableTiles(x_prev, y_prev, x, y);
            Debug.Log( tiles[x,y].selectable_tiles.Count);
            Random random = new Random();
            int type = random.Next(1, tiles[x,y].selectable_tiles.Count + 1); // Random type based on selectable_tiles
            InitializeTile(x, y, type);
        }
    }

    
    void InitializeTile(int x, int y, int type)
    {
        tiles[x, y].type = type;
        // Additional initialization logic for tiles if needed
    }

    void setSelectableTiles(int x_prev, int y_prev, int x_curr, int y_curr)
    {
        //selectable_tiles.Clear(); // Clear the list before adding new tiles

        Tile prevTile = tiles[x_prev, y_prev];
        Tile currentTile = tiles[x_curr, y_curr];
        
        foreach (var var in prevTile.adj_rules)
        {
            //Debug.Log(var + "==" + tile_types[index].sprite.name  + "==");
            //Debug.Log(tile_types[index].sprite.name.Equals(var));
            foreach (var type in tile_types)
            {
                if (type.sprite.name.Equals(var))
                {
                    //Debug.Log(type + "ADDED!");
                    currentTile.selectable_tiles.Add(type);
                   
                }
            }
            

            
        }

        // Now selectable_tiles contains only the tiles allowed adjacent to the current tile
    }





    void printTiles(List<Tile> l)
    {
        foreach (var VARIABLE in l)
        {
            Debug.Log(VARIABLE);
        }
    }
    }


   

    
   

