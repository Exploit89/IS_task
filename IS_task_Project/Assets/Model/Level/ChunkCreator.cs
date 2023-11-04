using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCreator : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    
    private List<DecorationModel> _decorations;
    private List<ObstacleModel> _obstacles;

    public ChunkModel CreateChunk()
    {
        return null;
    }
}
