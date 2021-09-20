using UnityEngine;
using System.Collections.Generic;

public static class GameObjectExtension
{
    public static GameObject Near<T>(this List<GameObject> self, T reference) where T : Component
    {
        if (reference == null)
            return self.FirstOrDefault();
        var objReference = reference.gameObject.transform.position;
        float minDistance = float.MaxValue;
        GameObject bestObj = null;
        foreach (var obj in self)
        {
            var dist = Vector3.Distance(objReference, obj.transform.position);
            if (dist < minDistance)
            {
                bestObj = obj;
                minDistance = dist;
            }
        }
        return bestObj;
    }

    public static GameObject Near(this List<GameObject> self, GameObject reference)
    {
        if (reference == null)
            return self.FirstOrDefault();
        var objReference = reference.gameObject.transform.position;
        float minDistance = float.MaxValue;
        GameObject bestObj = null;
        foreach (var obj in self)
        {
            var dist = Vector3.Distance(objReference, obj.transform.position);
            if (dist < minDistance)
            {
                bestObj = obj;
                minDistance = dist;
            }
        }
        return bestObj;
    }

    public static GameObject FindObject(this GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
    
    public static void ChangeCurrentAndChildrenLayers(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (var transform in gameObject.transform.GetChildren())
            transform.gameObject.ChangeCurrentAndChildrenLayers(layer);
    }
    
    public static void ChangeCurrentAndChildrenLayersWithSameLayer(this GameObject gameObject, int layer, int beforeLayer)
    {
        if(gameObject.layer == beforeLayer)
            gameObject.layer = layer;
        
        foreach (var childTransfom in gameObject.transform.GetChildren())
            childTransfom.gameObject.ChangeCurrentAndChildrenLayersWithSameLayer(layer, beforeLayer);
    }
    
    public static bool CheckCurrentAndChildrenName(this GameObject gameObject, GameObject objectName)
    {
        if (gameObject.name.Equals(objectName.name))
            return true;

        foreach (var childTransfom in gameObject.transform.GetChildren())
        {
            if (childTransfom.gameObject.CheckCurrentAndChildrenName(objectName))
                return true;
        }

        return false;
    }
}
