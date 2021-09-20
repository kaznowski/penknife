using UnityEngine;

public static class LayerMaskExtension
{
    public static int ToSingleLayer(this LayerMask routinePrefabLayer)
    {
        return Mathf.Max(0, (int)Mathf.Log(routinePrefabLayer.value, 2));
    }

    public static bool Contains(this LayerMask layerMask, int layer)
    {
        return ( ( (1 << layer) & layerMask ) == 0);
    }

    public static bool CompareToLayer(this int intLayer, LayerMask mask)
    {
        return mask == (mask | (1 << intLayer));
    }

    public static void TurnLayerOn(this LayerMask layerMask, int layer)
    {
        layerMask |= (1 << layer);
    }

    public static void TurnLayerOff(this LayerMask layerMask, int layer)
    {
        layerMask &= ~(1 << layer);
    }
}