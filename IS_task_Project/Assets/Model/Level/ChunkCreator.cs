using System.Collections.Generic;
using UnityEngine;

public class ChunkCreator : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private List<DecorationModel> _decorations;
    [SerializeField] private List<ObstacleModel> _obstacles;

    private Dictionary<string, float[]> _offsets;
    private int _decorationCount = 5;
    private int _minDecorationCount = 1;
    private int _chunkSideSize = 20;
    private int _halfChunkSide = 10;

    private void Start()
    {
        CreateStartChunks();
    }

    private void CreateStartChunks()
    {
        _offsets = new Dictionary<string, float[]>
        {
            { "up", new float[] { _chunkSideSize, 0, 0 } },
            { "up-right", new float[] { _chunkSideSize, 0, _chunkSideSize } },
            { "right", new float[] { 0, 0, _chunkSideSize } },
            { "down-right", new float[] { -_chunkSideSize, 0, _chunkSideSize } },
            { "down", new float[] { -_chunkSideSize, 0, 0 } },
            { "down-left", new float[] { -_chunkSideSize, 0, -_chunkSideSize } },
            { "left", new float[] { 0, 0, -_chunkSideSize } },
            { "left-up", new float[] { _chunkSideSize, 0, -_chunkSideSize } }
        };

        foreach (var item in _offsets.Values)
        {
            Vector3 vector = new Vector3(item[0], item[1], item[2]);
            GameObject chunk = Instantiate(_chunkPrefab, vector, Quaternion.identity);
            CreateDecorations(chunk);
            CreateObstacles(chunk);
        }
    }

    private void CreateDecorations(GameObject chunk)
    {
        int random = Random.Range(0, _decorations.Count);
        int randomCount = Random.Range(_minDecorationCount, _decorationCount);

        for (int i = 0; i < randomCount; i++)
        {
            int randomX = Random.Range(0, _halfChunkSide);
            int randomY = Random.Range(0, _halfChunkSide);
            var decoration = Instantiate(_decorations[random], chunk.transform.position, Quaternion.identity);
            decoration.transform.parent = chunk.transform;
            decoration.transform.position += new Vector3(randomX,1, randomY);
        }
    }

    private void CreateObstacles(GameObject chunk)
    {
        int random = Random.Range(0, _obstacles.Count);
        int randomCount = Random.Range(_minDecorationCount, _decorationCount);

        for (int i = 0; i < randomCount; i++)
        {
            int randomX = Random.Range(0, _halfChunkSide);
            int randomY = Random.Range(0, _halfChunkSide);
            var obstacle = Instantiate(_obstacles[random], chunk.transform.position, Quaternion.identity);
            obstacle.transform.parent = chunk.transform;
            obstacle.transform.position += new Vector3(randomX, 1, randomY);
        }
    }
}
