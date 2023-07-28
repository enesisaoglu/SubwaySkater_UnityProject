using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float chunkLength;

    public Chunk ShowChunk()
    {
        // Set chunk instance active...
        gameObject.SetActive(true);

        return this;
    }

    public Chunk HideChunk()
    {
        // Set chunk instance inactive...
        gameObject.SetActive(false);

        return this;
    }


}
