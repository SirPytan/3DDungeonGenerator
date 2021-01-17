using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prefab = null;
    public Vector3 position = Vector3.zero;

    public void CreateObject()
    {
        if (prefab != null)
        {
            GameObject spawnedObject = Instantiate(prefab, position, Quaternion.identity);
            Test2 test2 = spawnedObject.GetComponent<Test2>();
            test2.TestIfSpawnAreaIsFree();
        }
    }
}
