using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour 
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    public GameObject[] availableObjects;    
    public List<GameObject> objects;
    public float objectsMinDistance = 5.0f;    
    public float objectsMaxDistance = 10.0f;
    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;
    public float objectsMinRotation = -45.0f;
    public float objectsMaxRotation = 45.0f; 
    private float screenWidthInPoints;

    void Start () 
	{
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
    }

    void FixedUpdate () 
	{
        GenerateRoomIfRequred();
        GenerateObjectsIfRequired();    
	}

    void AddRoom(float farhtestRoomEndX) 
    {
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);
        float roomWidth = room.transform.Find("Floor").localScale.x;
        float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;
        room.transform.position = new Vector3(roomCenter, 0, 0);
        currentRooms.Add(room);			
    } 

    void GenerateRoomIfRequred() 
    {
        List<GameObject> roomsToRemove = new List<GameObject>();
        bool addRooms = true;        
        float playerX = transform.position.x;
        float removeRoomX = playerX - screenWidthInPoints;        
        float addRoomX = playerX + screenWidthInPoints;
        float farhtestRoomEndX = 0;
		
        foreach(var room in currentRooms) 
        {
            float roomWidth = room.transform.Find("Floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);    
            float roomEndX = roomStartX + roomWidth;                            

            if (roomStartX > addRoomX)
            {
                addRooms = false;
            }

            if (roomEndX < removeRoomX) 
            {
                roomsToRemove.Add(room);
            }
            farhtestRoomEndX = Mathf.Max(farhtestRoomEndX, roomEndX);
        }

        foreach(var room in roomsToRemove) 
        {
            currentRooms.Remove(room);
            Destroy(room);            
        }

        if (addRooms)
        {
            AddRoom(farhtestRoomEndX);
        }
	}

    void AddObject(float lastObjectX) 
    {
        int randomIndex = Random.Range(0, availableObjects.Length);
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);
        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomY = Random.Range(objectsMinY, objectsMaxY);
        obj.transform.position = new Vector3(objectPositionX,randomY,0); 
        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
        objects.Add(obj);            
    }

    void GenerateObjectsIfRequired() 
    {
        float playerX = transform.position.x;        
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;
        List<GameObject> objectsToRemove = new List<GameObject>();
		
        foreach (var obj in objects) 
        {
            float objX = obj.transform.position.x;
            farthestObjectX = Mathf.Max(farthestObjectX, objX);
            if (objX < removeObjectsX)
            {
                objectsToRemove.Add(obj);
            }
        }
        foreach (var obj in objectsToRemove) 
        {
            objects.Remove(obj);
            Destroy(obj);
        }
        if (farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }
	}
}
