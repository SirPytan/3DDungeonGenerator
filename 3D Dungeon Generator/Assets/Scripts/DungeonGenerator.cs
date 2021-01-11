using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    
    [SerializeField] private Vector3 m_DungeonStartPosition = Vector3.zero;
    [SerializeField] private List<GameObject> m_RoomPrefabs = new List<GameObject>();
    [SerializeField] private GameObject m_BlockedEntrancePrefab = null;
    
    private List<Room> m_GeneratedRooms = new List<Room>();
    private List<Room> m_NewGeneratedRooms = new List<Room>();

    private bool m_Active = false;

    private GameObject m_DungeonParent = null;

    public GameObject GetBlockedEntrancePrefab()
    {
        return m_BlockedEntrancePrefab;
    }
    
    public ref List<GameObject> GetRoomPrefabList()
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
        GameObject firstRoomPrefab = m_RoomPrefabs[Random.Range(0, m_RoomPrefabs.Count)];
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
        while (m_Active)
        {
            if (m_NewGeneratedRooms.Count > 0)
            {
                if (m_NewGeneratedRooms[m_NewGeneratedRooms.Count - 1].AreAllOpeningsConnected())
                {
                    Debug.Log(m_NewGeneratedRooms[m_NewGeneratedRooms.Count - 1].name + " is fully connected.");
                    m_GeneratedRooms.Add(m_NewGeneratedRooms[m_NewGeneratedRooms.Count - 1]);
                    m_NewGeneratedRooms.RemoveAt(m_NewGeneratedRooms.Count - 1);
                }
                else
                {
                    Debug.Log("Before: Amount of new generated rooms: " + m_NewGeneratedRooms.Count);
                    yield return StartCoroutine(m_NewGeneratedRooms[m_NewGeneratedRooms.Count - 1].SpawnAdjacentRooms(m_DungeonParent));
                    Debug.Log("After: Amount of new generated rooms: " + m_NewGeneratedRooms.Count);
                }
            }
            else
            {
                m_Active = false;
            }
                
            //Version 01: Did not work so well
            //Room room = m_GeneratedRooms.Find(room => room.AreAllOpeningsConnected() == false);

            //if (room != null)
            //{
            //    yield return StartCoroutine(room.SpawnAdjacentRooms(m_DungeonParent));
            //}
            //else
            //{
            //    m_Active = false;
            //}
        }
    }
}
