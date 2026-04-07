using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator_Script : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap groundTilemap;
    public Tilemap obstacleTilemap;

    [Header("Tiles")]
    public TileBase snowTile;
    public TileBase treeTile;

    [Header("Map Bounds")]
    public int minX = 0;
    public int maxX = 30;
    public int minY = 0;
    public int maxY = 30;

    [Header("Border")]
    public int borderThickness = 1;

    [Header("Tree Clusters")]
    public int clusterCount = 10;
    public int minClusterSize = 1;
    public int maxClusterSize = 3;

    [Header("Player Spawn")]
    public Transform player;
    public int safeRadius = 4;

    private int seed;

    void Start()
    {
        seed = Random.Range(0, 999999);
        Random.InitState(seed);

        ClearTilemaps();
        GenerateMap();
    }

    void ClearTilemaps()
    {
        groundTilemap.ClearAllTiles();
        obstacleTilemap.ClearAllTiles();
    }

    void GenerateMap()
    {
        int width = maxX - minX;
        int height = maxY - minY;
        int centerX = minX + width / 2;
        int centerY = minY + height / 2;

        // Fill entire area with snow
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), snowTile);
            }
        }

        // Paint border
        PaintBorder();

        // Place clusters avoiding safe zone around center
        for (int i = 0; i < clusterCount; i++)
        {
            int cx, cy;
            int attempts = 0;

            do
            {
                cx = Random.Range(minX + borderThickness + 2, maxX - borderThickness - 2);
                cy = Random.Range(minY + borderThickness + 2, maxY - borderThickness - 2);
                attempts++;
            }
            while (Vector2.Distance(
                       new Vector2(cx, cy),
                       new Vector2(centerX, centerY)) < safeRadius
                   && attempts < 20);

            PlaceCluster(cx, cy, Random.Range(minClusterSize, maxClusterSize));
        }

        // Spawn player at center
        if (player != null)
            player.position = new Vector3(centerX, centerY, 0);
    }

    void PaintBorder()
    {
        for (int x = minX; x < maxX; x++)
        {
            for (int b = 0; b < borderThickness; b++)
            {
                // Bottom border
                obstacleTilemap.SetTile(new Vector3Int(x, minY + b, 0), treeTile);
                // Top border
                obstacleTilemap.SetTile(new Vector3Int(x, maxY - 1 - b, 0), treeTile);
            }
        }

        for (int y = minY; y < maxY; y++)
        {
            for (int b = 0; b < borderThickness; b++)
            {
                // Left border
                obstacleTilemap.SetTile(new Vector3Int(minX + b, y, 0), treeTile);
                // Right border
                obstacleTilemap.SetTile(new Vector3Int(maxX - 1 - b, y, 0), treeTile);
            }
        }
    }

    void PlaceCluster(int cx, int cy, int size)
    {
        int pattern = Random.Range(0, 4);

        switch (pattern)
        {
            case 0: PlaceHorizontalLine(cx, cy, size); break;
            case 1: PlaceVerticalLine(cx, cy, size); break;
            case 2: PlaceLShape(cx, cy, size); break;
            case 3: PlaceSquare(cx, cy, size); break;
        }
    }

    void PlaceHorizontalLine(int cx, int cy, int length)
    {
        for (int x = cx - length; x <= cx + length; x++)
            PlaceTile(x, cy);
    }

    void PlaceVerticalLine(int cx, int cy, int length)
    {
        for (int y = cy - length; y <= cy + length; y++)
            PlaceTile(cx, y);
    }

    void PlaceLShape(int cx, int cy, int size)
    {
        // Horizontal part
        for (int x = cx; x <= cx + size; x++)
            PlaceTile(x, cy);

        // Vertical part
        for (int y = cy; y <= cy + size; y++)
            PlaceTile(cx, y);
    }

    void PlaceSquare(int cx, int cy, int size)
    {
        for (int x = cx; x <= cx + size; x++)
        {
            PlaceTile(x, cy);
            PlaceTile(x, cy + size);
        }
        for (int y = cy; y <= cy + size; y++)
        {
            PlaceTile(cx, y);
            PlaceTile(cx + size, y);
        }
    }

    void PlaceTile(int x, int y)
    {
        // Stay inside border
        if (x < minX + borderThickness || y < minY + borderThickness ||
            x >= maxX - borderThickness || y >= maxY - borderThickness)
            return;

        obstacleTilemap.SetTile(new Vector3Int(x, y, 0), treeTile);
    }
}