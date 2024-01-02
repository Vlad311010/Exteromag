using Interfaces;
using Structs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTileAttribute : MonoBehaviour, ISpellAttribute
{
    LayerMask layerMask;
    int radius;
    GameObject destroyEffect;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        layerMask = spell.destroyTileAttribute.layerMask;
        radius = spell.destroyTileAttribute.radius;
        destroyEffect = spell.destroyTileAttribute.destroyEffect;
    }

    public void OnCastEvent() { }

    public void OnHitEvent(CollisionData collisionData)
    {
        if (layerMask.CheckLayer(collisionData.GameObject.layer) && collisionData.GameObject.TryGetComponent(out Tilemap tilemap))
        {
            DestroyTile(tilemap, collisionData);
        }
    }

    private void DestroyTile(Tilemap tilemap, CollisionData collision)
    {
        Vector3Int tileCoordinates = TilemapUtils.GetTileCoordinatesFromCollision(tilemap, collision);
        tilemap.SetTile(tileCoordinates, null);
        Instantiate(destroyEffect, tilemap.CellToWorld(tileCoordinates), Quaternion.identity);

        List<Vector3Int> tilesToDestroy = new List<Vector3Int>();
        GetTilesToDestroy(tileCoordinates, radius, ref tilesToDestroy);
        
        foreach (Vector3Int coordinates in tilesToDestroy)
        {
            if (TilemapUtils.GetTileAt<Tile>(tilemap, coordinates) != null)
            {
                Instantiate(destroyEffect, tilemap.CellToWorld(coordinates), Quaternion.identity);
            }
            tilemap.SetTile(coordinates, null);
        }

        SceneController.BakeNavMesh();
    }

    private List<Vector3Int> GetTilesToDestroy(Vector3Int origin, int deepth, ref List<Vector3Int> tiles)
    {
        if (deepth == 0) return tiles;

        List<Vector3Int> adjacentTiles = TilemapUtils.GetAdjacentTilesCoordinatels(origin);
        foreach (Vector3Int coordinates in adjacentTiles)
        {
            tiles.Add(coordinates);
            GetTilesToDestroy(coordinates, deepth - 1, ref tiles);
        }

        return tiles;
    }
}
