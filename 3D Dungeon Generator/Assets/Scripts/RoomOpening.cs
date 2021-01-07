using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOpening : MonoBehaviour
{
    [SerializeField] private bool m_Connected = false;

    private Room m_Room = null;
    private DungeonGenerator m_DungeonGenerator = null;
    private List<GameObject> m_RoomPrefabs = null;
    
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
            Room randomRoom = randomRoomPrefab.GetComponent<Room>();
            //Step 2: Get Random room opening
            List<RoomOpening> roomOpenings = randomRoom.GetUnconnectedRoomOpenings();
            RoomOpening roomOpening = roomOpenings[Random.Range(0, roomOpenings.Count)];
            
            //Step 3: Spawn that room on that random room connected with the choosen Room opening in a way that the room opening is where this room opening is
            Vector3 localPos = roomOpening.gameObject.transform.localPosition;
            //Todo: Step 4: Rotate the spawned room so it looks in the right direction (take the rotation into account before spawning)

            Vector3 overlapBoxPosition = gameObject.transform.position - localPos +
                                         randomRoom.GetBoxCollider().gameObject.transform.localPosition;
            Collider[] colliders = Physics.OverlapBox(overlapBoxPosition, randomRoom.GetBoxCollider().bounds.extents, Quaternion.identity,
                LayerMask.NameToLayer("RoomBoundingBox"));

            if (colliders.Length == 0)
            {

                //Todo: Take roation into account when spawning
                GameObject spawnedRoomPrefab = Instantiate(randomRoomPrefab, gameObject.transform.position - localPos, Quaternion.identity,
                    gameObject.transform.parent.parent);

                
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
