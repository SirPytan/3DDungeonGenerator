using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private BoxCollider m_BoxCollider = null;
    [SerializeField] private List<RoomOpening> m_RoomOpenings = new List<RoomOpening>();

    public bool AreAllOpeningsConnected()
    {
        bool openingsConnected = true;
        foreach (RoomOpening roomOpening in m_RoomOpenings)
        {
            if (!roomOpening.IsConnected())
            {
                openingsConnected = false;
                break;
            }
        }
        
        return openingsConnected;
    }
    
    private void Awake()
    {
        foreach (RoomOpening roomOpening in m_RoomOpenings)
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
    
    public List<RoomOpening> GetRoomOpenings()
    {
        return m_RoomOpenings;
    }

    public IEnumerator SpawnAdjacentRooms(GameObject parent)
    {
        int amountOfRoomOpenings = 0;
        
        foreach (RoomOpening roomOpening in m_RoomOpenings)
        {
            if (!roomOpening.IsConnected())
            {
                amountOfRoomOpenings++;
                //roomOpening.SpawnAdjacentRoom(parent);
                StartCoroutine(roomOpening.SpawnAdjacentRoomEnumerator(parent));
            }
        }

        bool active = true;

        while (active)
        {
            int counter = 0;
            yield return null;
            //Todo: To reduce the amount of checking and improve performance, I could make a second list with only the ones that still need to be checked instead,
            foreach (RoomOpening roomOpening in m_RoomOpenings)
            {
                if (roomOpening.IsCoroutineDone())
                {
                    counter++;
                }
            }

            if (counter == amountOfRoomOpenings)
            {
                active = false;
            }
        }
    }

}
