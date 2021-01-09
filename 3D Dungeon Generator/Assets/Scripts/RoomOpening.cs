using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RoomOpening : MonoBehaviour
{
    [SerializeField] private bool m_IsConnected = false;
    [SerializeField] private BoxCollider m_Trigger = null;

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

    //https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    public void SetRoom(in Room room)
    {
        m_Room = room;
    }
    public void SpawnAdjacentRoom(in GameObject parent)
    {
        //If multiple dungeons get generated one after another this delivers more working results 
        //and does not fail, because the colliders got not flushed yet
        StartCoroutine(SpawnAdjacentRoom(parent));
        //If I implement the skip one frame, after deletion of the current dungeon then this method should work as well
        //But currently the Coroutine is more reliable
        //SpawnRoom(parent);
    }

    private const string m_Tagname = "opening";
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == m_Tagname)
        {
            //Debug.Log("Trigger: " + other.gameObject.name);
            other.GetComponent<RoomOpening>().SetConnected(true);
            //Debug.Log("Set other to connected.");
            m_Trigger.enabled = false;
            //if (other.gameObject.GetComponent<RoomOpening>().IsConnected())
            //{
            //    Debug.Log("Trigger other is connected");
            //}
        }
    }
    
    IEnumerator SpawnAdjacentRoom(GameObject parent)
    {
        //yield return new WaitForFixedUpdate();
        yield return null;
        //SpawnRoom(parent);

        #region SpawnRoom
        int counter = 0;
        bool fittingRoomFound = false;

        //Todo: For the future: To speed up the process of finding a fitting room, I could put the different rooms in a list and each room that got tested gets removed, same for the possible openings
        while (!fittingRoomFound)
        {
            //Step 1: Get Random room
            GameObject randomRoomPrefab = m_RoomPrefabs[Random.Range(0, m_RoomPrefabs.Count)];
            //Debug.Log("Try to spawn room: " + randomRoomPrefab.gameObject.name);
            Room randomRoom = randomRoomPrefab.GetComponent<Room>();
            BoxCollider roomBoxCollider = randomRoom.GetBoxCollider();

            //Step 2: Get Random room opening
            List<RoomOpening> roomOpenings = randomRoom.GetUnconnectedRoomOpenings();
            RoomOpening roomOpening = roomOpenings[Random.Range(0, roomOpenings.Count)];

            //Step 3: Calculate localPos of the roomOpening of the not spawned room
            Vector3 localPos = roomOpening.gameObject.transform.localPosition;
            //Step 4: Calculate the needed rotation, between the current room opening and the other room opening
            float angleBetweenBothOpenings = Vector3.SignedAngle(gameObject.transform.forward, roomOpening.gameObject.transform.forward * -1, Vector3.up) * -1;
            //Debug.Log("Angle between both vectors: " + angleBetweenBothOpenings);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angleBetweenBothOpenings, 0));

            //Step 5: Calculate the position of the box for check overlap
            Vector3 roomOverlapBoxCenter = gameObject.transform.position + (localPos * -1) + roomBoxCollider.gameObject.transform.localPosition + roomBoxCollider.center;

            //Step 6: Rotate the roomOverlapBox around this room opening position, to align the room correctly to this entrance 
            Vector3 rotatedRoomPivot = RotatePointAroundPivot(roomOverlapBoxCenter, gameObject.transform.position,
                new Vector3(0, angleBetweenBothOpenings, 0));


            Bounds roomBoxBounds = new Bounds(rotatedRoomPivot, roomBoxCollider.size);
            //Optional: Draw Box that checks for overlap
            DebugExtension.DebugBounds(roomBoxBounds, Color.blue, 15, false);

            //Step 7: Check if collider are in the way, to spawn this room
            bool overlapsWithColliders = Physics.CheckBox(rotatedRoomPivot, roomBoxBounds.extents, Quaternion.identity, LayerMask.GetMask("RoomBoundingBox"));

            //Step 8: Spawn the room, if no colliders are in the way 
            if (!overlapsWithColliders)
            {
                GameObject spawnedRoomPrefab = Instantiate(randomRoomPrefab, rotatedRoomPivot - roomBoxCollider.gameObject.transform.localPosition, rotation, parent.transform);
                Debug.Log("Spawned room: " + spawnedRoomPrefab.gameObject.name);

                m_IsConnected = true;
                yield return null; //Important: It gives the trigger a chance to update and marks the room openings as connected, if the collider is in the triggerbox of another opening
                
                //Step 9: Check which room openings from the spawned room are now connected with other room openings and mark them as connected
                //This actually gets done by the OnTriggerStay() function, and this function does check the connection on all entrances. 
                
                //Room spawnedRoom = spawnedRoomPrefab.GetComponent<Room>();
                //List<RoomOpening> spawnedRoomOpenings = spawnedRoom.GetUnconnectedRoomOpenings();
                //foreach (RoomOpening opening in spawnedRoomOpenings)
                //{
                //    if (opening.IsConnected())
                //    {
                //        Debug.Log("Other Connected");
                //    }
                //    else
                //    {
                //        Debug.Log("Other not connected");
                //    }
                //}

                fittingRoomFound = true;
            }
            else
            {
                Debug.Log("Could not spawn room");
            }

            if (counter > 49)
            {
                Debug.Log("Could not spawn a room, broke up after " + counter + " attemps.");
                break;
            }
            counter++;
        }

        //(Step 7: Notify original room, that you are done)


        #endregion

    }

    private void SpawnRoom(in GameObject parent)
    {
        int counter = 0;
        bool fittingRoomFound = false;

        while (!fittingRoomFound)
        {
            //Step 1: Get Random room
            //GameObject randomRoomPrefab = m_RoomPrefabs[0];//-----------------------------------------------------change
            GameObject randomRoomPrefab = m_RoomPrefabs[Random.Range(0, m_RoomPrefabs.Count)];
            Debug.Log("Try to spawn room: " + randomRoomPrefab.gameObject.name);
            Room randomRoom = randomRoomPrefab.GetComponent<Room>();
            BoxCollider roomBoxCollider = randomRoom.GetBoxCollider();

            //Step 2: Get Random room opening
            List<RoomOpening> roomOpenings = randomRoom.GetUnconnectedRoomOpenings();
            RoomOpening roomOpening = roomOpenings[Random.Range(0, roomOpenings.Count)];

            //Step 3: Calculate localPos of the roomOpening of the not spawned room
            Vector3 localPos = roomOpening.gameObject.transform.localPosition;
            //Step 4: Calculate the needed rotation, between the current room opening and the other room opening
            float angleBetweenBothOpenings = Vector3.SignedAngle(gameObject.transform.forward, roomOpening.gameObject.transform.forward * -1, Vector3.up) * -1;
            //Debug.Log("Angle between both vectors: " + angleBetweenBothOpenings);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angleBetweenBothOpenings, 0));

            //Step 5: Calculate the position of the box for check overlap
            Vector3 roomOverlapBoxCenter = gameObject.transform.position + (localPos * -1) + roomBoxCollider.gameObject.transform.localPosition + roomBoxCollider.center;

            //Step 6: Rotate the roomOverlapBox around this room opening position, to align the room correctly to this entrance 
            Vector3 rotatedRoomPivot = RotatePointAroundPivot(roomOverlapBoxCenter, gameObject.transform.position,
                new Vector3(0, angleBetweenBothOpenings, 0));

            
            Bounds roomBoxBounds = new Bounds(rotatedRoomPivot, roomBoxCollider.size);
            //Optional: Draw Box that checks for overlap
            DebugExtension.DebugBounds(roomBoxBounds, Color.blue, 15, false);

            //Step 7: Check if collider are in the way, to spawn this room
            bool overlapsWithColliders = Physics.CheckBox(rotatedRoomPivot, roomBoxBounds.extents, Quaternion.identity, LayerMask.GetMask("RoomBoundingBox"));

            //Step 8: Spawn the room, if no colliders are in the way 
            if (!overlapsWithColliders)
            {
                GameObject spawnedRoomPrefab = Instantiate(randomRoomPrefab, rotatedRoomPivot - roomBoxCollider.gameObject.transform.localPosition, rotation, parent.transform);
                Debug.Log("Spawned room: " + spawnedRoomPrefab.gameObject.name);

                //Commented for testing
                //m_IsConnected = true;
                //roomOpening.SetConnected(true);

                foreach (RoomOpening opening in roomOpenings)
                {
                    if (!opening.IsConnected())
                    {

                    }
                }
                //Step 9: Check which room openings from the spawned room are now connected with other room openings and mark them as connected

                
                
                fittingRoomFound = true;
            }
            else
            {
                Debug.Log("Could not spawn room");
            }

            if (counter > 49)
            {
                Debug.Log("Could not spawn a room, broke up after " + counter + " attemps.");
                break;
            }
            counter++;
        }

        //(Step 7: Notify original room, that you are done)
    }

    
    
    public bool IsConnected()
    {
        return m_IsConnected;
    }

    public void SetConnected(bool isConnected)
    {
        m_IsConnected = isConnected;
    }
}
