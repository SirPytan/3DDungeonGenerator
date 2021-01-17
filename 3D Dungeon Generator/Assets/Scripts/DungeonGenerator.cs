using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    
    [SerializeField] private Vector3 m_DungeonStartPosition = Vector3.zero;
    [SerializeField] private ERoomType m_FirstRoomType = ERoomType.Room3x3;
    [SerializeField] private List<RoomTypeList> m_RoomPrefabs = new List<RoomTypeList>();
    [SerializeField] private GameObject m_BlockedEntrancePrefab = null;
    
    private List<Room> m_GeneratedRooms = new List<Room>();
    private List<Room> m_NewGeneratedRooms = new List<Room>();

    private bool m_Active = false;
    private GameObject m_DungeonParent = null;

    public GameObject GetBlockedEntrancePrefab()
    {
        return m_BlockedEntrancePrefab;
    }
    
    public ref List<RoomTypeList> GetRoomPrefabList()
    {
        return ref m_RoomPrefabs;
    }

    public ref List<Room> GetGeneratedRoomsList()
    {
        return ref m_GeneratedRooms;
    }

    public ref List<Room> GetNewGeneratedRoomsList()
    {
        return ref m_NewGeneratedRooms;
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
        m_NewGeneratedRooms.Clear();

        //Create first room
        List<GameObject> rooms = m_RoomPrefabs.Find(room => room.RoomType == m_FirstRoomType).m_Rooms;
        GameObject firstRoomPrefab = rooms[Random.Range(0, rooms.Count)];
        if (firstRoomPrefab != null)
        {
            float angle = 90.0f * Random.Range(0, 4);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            GameObject firstGeneratedRoom = Instantiate(firstRoomPrefab, m_DungeonStartPosition, rotation, m_DungeonParent.transform);
            Debug.Log("Spawned first room: " + firstGeneratedRoom.gameObject.name);
            Room firstRoom = firstGeneratedRoom.GetComponent<Room>();
            m_NewGeneratedRooms.Add(firstRoom);
            m_Active = true;
        }
        else
        {
            Debug.LogWarning("Room invalid.");
            return;
        }

        StartCoroutine(GenerateRooms());
    }

    IEnumerator GenerateRooms()
    {
        //Debug.Log("Generator started");
        int index = 0;
        while (m_Active)
        {
            if (m_NewGeneratedRooms.Count > 0)
            {
                index = m_NewGeneratedRooms.Count - 1;
                if (m_NewGeneratedRooms[index].AreAllOpeningsConnected())
                {
                    //Debug.Log(m_NewGeneratedRooms[index].name + " is fully connected.");
                    m_GeneratedRooms.Add(m_NewGeneratedRooms[index]);
                    m_NewGeneratedRooms.RemoveAt(index);
                }
                else
                {
                    //Debug.Log("Before: Amount of new generated rooms: " + m_NewGeneratedRooms.Count);
                    yield return StartCoroutine(m_NewGeneratedRooms[index].SpawnAdjacentRooms(m_DungeonParent));
                    //Debug.Log("After: Amount of new generated rooms: " + m_NewGeneratedRooms.Count);
                }
            }
            else
            {
                m_Active = false;
            }
        }
    }
}
