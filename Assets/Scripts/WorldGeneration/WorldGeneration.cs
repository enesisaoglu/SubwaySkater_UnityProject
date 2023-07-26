using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    //Gameplay...
    private float chunkSpawnZ;
    /* a Queue is being used to store objects of type Chunk. The activeChunks queue will hold instances of the Chunk class,
     * and you can perform typical queue operations on it, such as adding elements to the back of the queue and 
     * removing elements from the front.
     */
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    //Configurable fields...
    [SerializeField] private int firstChunkSpawnPosition = -10; // Where it should be spawn firstly in the world...
    [SerializeField] private int chunkOnScreen = 5; //How many chunk should we have on the screen at the same time...
    [SerializeField] private float deSpawnDistance = 5.0f; // How far away do we have to go after a chunk we spawn it...
   
    [SerializeField] private List<GameObject> chunkPrefab; // Dynamic list to keep Prefabs...
    [SerializeField] private Transform cameraTransform;

    #region TO DELETE $$
    private void Awake()
    {
        ResetWorld();
    }
    #endregion

    private void Start()
    {
        //Check if we have an empty chunkPrefab list...
        if(chunkPrefab.Count == 0)
        {
            Debug.LogError("No chunk prefab found on the world generator, please assign some chunks!");
            return;
        }


        //Try to assign the cameraTransform... 
        if(!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("We have assigned cameraTransform automaticly to the Camera.main.");
        }
    }

    private void Update()
    {
        ScanPosition();
    }

    private void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z; // Set cameraZ to the position z of main camera that assigned to cameraTransform... 
        Chunk lastChunk = activeChunks.Peek(); // Returns the element at the front of the queue without removing it...


        //If we are far enough from last chunk...
        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + deSpawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }

    private void SpawnNewChunk()
    {
        // Get a random index for which prefab to spawn...
        int randomIndex = Random.Range(0, chunkPrefab.Count);

        // Does it already exist within our pool...
        //  Find an element that satisfies this condition...
        Chunk chunk = chunkPool.Find( x => !x.gameObject.activeSelf && x.name == ( chunkPrefab[randomIndex].name + "(clone)" ) );

        // Create a chunk, if were not able to find one...
        if(!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }

        // Place the object, and show it...
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.chunkLength;

        // store the value, to reuse in the pool...
        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }

    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue(); // Removes and returns the element at the front (beginning) of the queue...
        chunk.HideChunk(); // Hide chunk...
        chunkPool.Add(chunk); // Add to pool...
    }

    public void ResetWorld()
    {
        // Reset the chunkSpawnZ...
        chunkSpawnZ = firstChunkSpawnPosition;

        for (int i = activeChunks.Count; i != 0; i--)
        {
            DeleteLastChunk();
        }

        for (int i = 0; i < chunkOnScreen; i++)
        {
            SpawnNewChunk();
        }
    }
}
