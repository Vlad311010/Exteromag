using Structs;
using UnityEngine;

public static class Extrnsions
{
    /*public static bool WrongLayer(this LayerMask layerMask, LayerMask layer)
    {
        return layerMask != (layerMask | (1 << layer));
    }

    public static bool CorrectLayer(this LayerMask layerMask, LayerMask layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }*/

    public static bool CheckLayer(this LayerMask layerMask, LayerMask layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
