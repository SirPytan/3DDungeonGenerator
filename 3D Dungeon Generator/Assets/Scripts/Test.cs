using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prefab = null;
    public Vector3 position = Vector3.zero;

    public BoxCollider boxcollider = null;
    public bool useLayerMask = false;
    //public LayerMask layerMask = 6;
    
    
    public void CreateObject()
    {
        if(prefab != null) Instantiate(prefab, position, Quaternion.identity);

        DebugExtension.DebugLocalCube(gameObject.transform, boxcollider.size, Color.blue, boxcollider.center, 100, false);

        bool overlapsWithColliders = false;

        if (useLayerMask)
        {
            overlapsWithColliders = Physics.CheckBox(boxcollider.center, boxcollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("RoomBoundingBox"));
        }
        else
        {
            overlapsWithColliders = Physics.CheckBox(boxcollider.center, boxcollider.bounds.extents, Quaternion.identity);
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
