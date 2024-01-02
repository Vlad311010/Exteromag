using Structs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public struct TileWithPosition
{
    public Tile tile;
    public Vector3Int position;

    public TileWithPosition(Tile tile, Vector3Int position)
    {
        this.tile = tile;
        this.position = position;
    }
}

public static class TilemapUtils
{
    static float contactPointNormalOffset = 0.3f;

    public static T GetTileFromCollision<T>(Tilemap tilemap, CollisionData collision) where T : Tile
    {
        Vector3Int tileCoordinates = tilemap.WorldToCell(collision.Contacts[0].point - collision.Contacts[0].normal * contactPointNormalOffset);
        return tilemap.GetTile<T>(tileCoordinates);
    }

    public static Vector3Int GetTileCoordinatesFromCollision(Tilemap tilemap, CollisionData collision)
    {
        return tilemap.WorldToCell(collision.Contacts[0].point - collision.Contacts[0].normal * contactPointNormalOffset);
    }

    public static T GetTileAt<T>(Tilemap tilemap, Vector3Int coordinates) where T : Tile
    {
        return tilemap.GetTile<T>(coordinates);
    }

    public static void SetTile(Tilemap tilemap, Vector3Int coordinates, Tile tile)
    {
        tilemap.SetTile(coordinates, tile);
    }

    public static List<T> GetAdjacentTiles<T>(Tilemap tilemap, Vector3Int coordinates) where T : Tile
    {
        List<T> adjacent = new List<T>();

        T up = GetTileAt<T>(tilemap, coordinates + new Vector3Int(0, 1));
        T down = GetTileAt<T>(tilemap, coordinates + new Vector3Int(0, -1));
        T right = GetTileAt<T>(tilemap, coordinates + new Vector3Int(1, 0));
        T left = GetTileAt<T>(tilemap, coordinates + new Vector3Int(-1, 0));

        adjacent.Add(up);
        adjacent.Add(down);
        adjacent.Add(right);
        adjacent.Add(left);

        return adjacent;
    }

    public static List<Vector3Int> GetAdjacentTilesCoordinatels(Vector3Int coordinates)
    {
        return new List<Vector3Int>
        {
            coordinates + new Vector3Int(0, 1),
            coordinates + new Vector3Int(0, -1),
            coordinates + new Vector3Int(1, 0),
            coordinates + new Vector3Int(-1, 0)
        };
    }



}