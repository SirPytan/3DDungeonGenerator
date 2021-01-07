using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private BoxCollider m_BoxCollider = null;
    [SerializeField] private List<RoomOpening> m_UnconnectedRoomOpenings = new List<RoomOpening>();
    private List<RoomOpening> m_ConnectedRoomOpenings = new List<RoomOpening>();
    
    private void Awake()
    {
        foreach (RoomOpening roomOpening in m_UnconnectedRoomOpenings)
        {
            roomOpening.SetRoom(this);
        }
    }

    public void DisableSpawnCollider()
    {
        m_BoxCollider.enabled = false;
    }
    
    public BoxCollider GetBoxCollider()
    {
        return m_BoxCollider;
    }
    
    public List<RoomOpening> GetUnconnectedRoomOpenings()
    {
        return m_UnconnectedRoomOpenings;
    }

    public void MoveRoomOpeningToConnected(in RoomOpening roomOpening)
    {
        m_UnconnectedRoomOpenings.Remove(roomOpening);
        m_ConnectedRoomOpenings.Add(roomOpening);
    }

    public void SpawnAdjacentRooms()
    {
        //Test setup
        m_UnconnectedRoomOpenings[0].SpawnAdjacentRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
