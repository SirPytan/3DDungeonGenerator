using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RoomOpening : MonoBehaviour
{
    [SerializeField] private bool m_Connected = false;

    private Room m_Room = null;
    private DungeonGenerator m_DungeonGenerator = null;
    private List<GameObject> m_RoomPrefabs = null;

    void OnDrawGizmos()
    {
        DebugExtension.DrawArrow(gameObject.transform.position, gameObject.transform.forward, Color.yellow);
    }


    private void Awake()
    {
        m_DungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<DungeonGenerator>();
        if (m_DungeonGenerator != null)
        {
            m_RoomPrefabs = m_DungeonGenerator.GetRoomPrefabList();
        }
    }

    public void SetRoom(in Room room)
    {
        m_Room = room;
    }
    
    public void SpawnAdjacentRoom()
    {
        bool fittingRoomFound = false;

        while (!fittingRoomFound)
        {
            //Step 1: Get Random room
            GameObject randomRoomPrefab = m_RoomPrefabs[Random.Range(0, m_RoomPrefabs.Count)];
            Debug.Log("Try to spawn room: " + randomRoomPrefab.gameObject.name);
            Room randomRoom = randomRoomPrefab.GetComponent<Room>();
            //Step 2: Get Random room opening
            List<RoomOpening> roomOpenings = randomRoom.GetUnconnectedRoomOpenings();
            RoomOpening roomOpening = roomOpenings[Random.Range(0, roomOpenings.Count)];
            
            //Step 3: Spawn that room on that random room connected with the choosen Room opening in a way that the room opening is where this room opening is
            Vector3 localPos = roomOpening.gameObject.transform.localPosition;
            //Todo: Step 4: Rotate the spawned room so it looks in the right direction (take the rotation into account before spawning)

            BoxCollider roomBoxCollider = randomRoom.GetBoxCollider();


            Vector3 overlapBoxPosition = gameObject.transform.position - localPos +
                                         roomBoxCollider.gameObject.transform.localPosition;

            Vector3 overlapBoxPosition2 =  -localPos + roomBoxCollider.gameObject.transform.localPosition;
            DebugExtension.DebugLocalCube(gameObject.transform, roomBoxCollider.size, Color.blue, overlapBoxPosition2, 1000,false);
            //Collider[] colliders = Physics.OverlapBox(overlapBoxPosition, roomBoxCollider.bounds.extents, Quaternion.identity, LayerMask.NameToLayer("RoomBoundingBox"));

            //bool overlapsWithColliders = Physics.CheckBox(overlapBoxPosition, roomBoxCollider.bounds.extents, Quaternion.identity, LayerMask.NameToLayer("RoomBoundingBox"));

            bool overlapsWithColliders = Physics.CheckBox(overlapBoxPosition, roomBoxCollider.bounds.extents, Quaternion.identity);

            //Debug.Log("Collider Amount: " + colliders.Length);

            //if (colliders.Length == 0)
            if (!overlapsWithColliders)    
            {

                //Todo: Take roation into account when spawning
                GameObject spawnedRoomPrefab = Instantiate(randomRoomPrefab, gameObject.transform.position - localPos, Quaternion.identity, gameObject.transform.parent.parent);
                Debug.Log("Spawned room: " + spawnedRoomPrefab.gameObject.name); 

                
                //Step 6: Check which room openings from the spawned room are now connected with other room openings and mark them as connected

                fittingRoomFound = true;
            }
            else
            {
                Debug.Log("Could not spawn room");
            }
            
        }
        
        
        

        
        //(Step 7: Notify original room, that you are done)
        
        // spawnedRoomPrefab.GetComponent<Room>();
    }

    public bool IsConnected()
    {
        return m_Connected;
    }
}
