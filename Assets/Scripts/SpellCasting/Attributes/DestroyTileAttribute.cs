using Interfaces;
using Structs;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTileAttribute : MonoBehaviour, ISpellAttribute
{
    LayerMask layerMask;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        layerMask = spell.destroyTileAttribute.layerMask;
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
        // replaceTile = Instantiate(replaceTile);

        Vector3Int tileCoordinates = TilemapUtils.GetTileCoordinatesFromCollision(tilemap, collision);
        tilemap.SetTile(tileCoordinates, null);
        SceneController.BakeNavMesh();
        
        // tilemap.SetTile(tileCoordinates, replaceTile);
    }
}
