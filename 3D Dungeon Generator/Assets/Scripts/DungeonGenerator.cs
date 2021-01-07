using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    
    [SerializeField] private Vector3 m_DungeonStartPosition = Vector3.zero;
    [SerializeField] private List<GameObject> m_RoomPrefabs = new List<GameObject>();

    private List<Room> m_GeneratedRooms = new List<Room>();

    private bool m_Active = false;

    public List<GameObject> GetRoomPrefabList()
    {
        return m_RoomPrefabs;
    }
    
    public void GenerateDungeon()
    {
        GameObject dungeonParent = Instantiate(new GameObject("GeneratedDungeon"));
        
        //Create first room
        GameObject firstRoomPrefab = m_RoomPrefabs[Random.Range(0, m_RoomPrefabs.Count)];
        if (firstRoomPrefab != null)
        {
            //Todo: Random Rotation in 90 Degree steps
            GameObject firstGeneratedRoom  = Instantiate(firstRoomPrefab, m_DungeonStartPosition, Quaternion.identity, dungeonParent.transform);
            Debug.Log("Spawned room: " + firstGeneratedRoom.gameObject.name);
            Room firstRoom = firstGeneratedRoom.GetComponent<Room>();
            m_GeneratedRooms.Add(firstRoom);
            m_Active = true;
        }
        else
        {
            Debug.LogWarning("Room invalid.");
            return;
        }

        //Test setup
        m_GeneratedRooms[0].SpawnAdjacentRooms();
        
        //while (m_Active)
        //{
        //    foreach (Room room in m_GeneratedRooms)
        //    {
                
        //    }
        //}
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
