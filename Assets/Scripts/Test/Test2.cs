using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public bool useLayerMask = true;
    public Vector3 pos = Vector3.zero;
    public Vector3 size = Vector3.one;
    

    public void TestIfSpawnAreaIsFree()
    {
        bool overlapsWithColliders = false;

        Bounds bounds = new Bounds(pos, size);
        
        DebugExtension.DebugBounds(bounds, Color.blue, 100, false);
        
        if (useLayerMask)
        {
            overlapsWithColliders = Physics.CheckBox(pos, bounds.extents, Quaternion.identity, LayerMask.GetMask("RoomBoundingBox"));
        }
        else
        {
            overlapsWithColliders = Physics.CheckBox(pos, bounds.extents, Quaternion.identity);
        }


        if (overlapsWithColliders)
        {
            Debug.Log("Overlaps with colliders.");
        }
        else
        {
            Debug.Log("Does not overlap with colliders.");
        }
    }
    
}
