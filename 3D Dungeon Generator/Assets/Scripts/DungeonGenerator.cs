using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    
    [SerializeField] private Vector3 m_DungeonStartPosition = Vector3.zero;
    [SerializeField] private List<GameObject> m_RoomPrefabs = new List<GameObject>();

    private List<Room> m_GeneratedRooms = new List<Room>();

    private bool m_Active = false;

    private GameObject m_DungeonParent = null;

    public List<GameObject> GetRoomPrefabList()
    {
        return m_RoomPrefabs;
    }
    
    public void GenerateDungeon()
    {

        //Faster way of deleting all generated objects
        //if (m_DungeonParent != null)
        //{
        //    Destroy(m_DungeonParent);
        //    m_DungeonParent = null;
        //}
        //m_DungeonParent = new GameObject("GeneratedDungeon");

        if (m_DungeonParent == null)
        {
            //Instantiate new empty GameObject
            m_DungeonParent = new GameObject("GeneratedDungeon");
        }
        else
        {
            //Delete all children
            foreach (Transform child in m_DungeonParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        m_GeneratedRooms.Clear();

        //Create first room
        //GameObject firstRoomPrefab = m_RoomPrefabs[Random.Range(0, m_RoomPrefabs.Count)];
        GameObject firstRoomPrefab = m_RoomPrefabs[3];
        if (firstRoomPrefab != null)
        {
            //Todo: Random Rotation in 90 Degree steps
            GameObject firstGeneratedRoom = Instantiate(firstRoomPrefab, m_DungeonStartPosition, Quaternion.identity, m_DungeonParent.transform);
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
        m_GeneratedRooms[0].SpawnAdjacentRooms(m_DungeonParent);

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
