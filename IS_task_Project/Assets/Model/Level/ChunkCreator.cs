using System.Collections.Generic;
using UnityEngine;

public class ChunkCreator : MonoBehaviour
{
    [SerializeField] private GameObject _chunkTilePrefab;
    [SerializeField] private List<DecorationModel> _decorations;
    [SerializeField] private List<ObstacleModel> _obstacles;
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private Player _player;

    private Dictionary<string, float[]> _offsets;
    
    private int _decorationCount = 5;
    private int _minDecorationCount = 1;
    private int _chunkSideSize = 20;
    private int _halfChunkSide = 10;
    private Dictionary<string, string> _oppositeNames = new Dictionary<string, string>
        {
            { "up", "down" },
            { "up-right", "down-left"},
            { "right", "left"},
            { "down-right", "up-left" },
            { "down", "up" },
            { "down-left", "up_right" },
            { "left", "right" },
            { "up-left", "down-right" }
        };

    public void TryCreateChunk(BoardAnchor boardAnchor)
    {
        Vector3 startVector = boardAnchor.transform.GetComponentInParent<ChunkModel>().MainAnchorPosition;
        Vector3 directionVector = startVector + boardAnchor.GetNewPosition(); //
        RaycastHit hit;
        int layer = LayerMask.NameToLayer("ChunkAnchorCollider");
        int layerMask = 1 << layer;

        if (Physics.Raycast(startVector, directionVector, out hit, 85, layerMask))
        {
            Debug.Log("HIT");
            Debug.Log(startVector + " to " + directionVector);
        }
        else
        {
            Debug.Log("NO_Hit");
            Debug.Log(startVector + " to " + directionVector);
            if (boardAnchor.IsFreeChunk())
            {
                CreateChunk(boardAnchor.GetAnchor());
                boardAnchor.SetChunkBusy();
            }
        }
    }

    private void Start()
    {
        CreateStartChunk();
    }

    private void CreateStartChunk()
    {
        _offsets = new Dictionary<string, float[]>
        {
            {"central", new float[] {0, 0, 0 } },
            { "up", new float[] { 0, 0, _chunkSideSize } },
            { "up-right", new float[] { _chunkSideSize, 0, _chunkSideSize } },
            { "right", new float[] { _chunkSideSize, 0, 0 } },
            { "down-right", new float[] { _chunkSideSize, 0, -_chunkSideSize } },
            { "down", new float[] { 0, 0, -_chunkSideSize } },
            { "down-left", new float[] { -_chunkSideSize, 0, -_chunkSideSize } },
            { "left", new float[] { -_chunkSideSize, 0, 0 } },
            { "up-left", new float[] { -_chunkSideSize, 0, _chunkSideSize } }
        };
        var offset = _offsets["central"];
        var chunk = Instantiate(_chunkPrefab, new Vector3 (offset[0], offset[1], offset[2]), Quaternion.identity );
        chunk.GetComponent<ChunkModel>().SetChunkCreator(gameObject);
        chunk.GetComponent<ChunkModel>().SetAnchorPosition(chunk);
        chunk.transform.SetParent(transform);

        foreach (var item in _offsets)
        {
            Vector3 vector = new Vector3(item.Value[0], item.Value[1], item.Value[2]);
            GameObject chunkTile = Instantiate(_chunkTilePrefab, vector, Quaternion.identity);

            if (item.Key != "central")
            {
                CreateDecorations(chunkTile);
                CreateObstacles(chunkTile);
            }
            chunkTile.transform.parent = chunk.transform;
        }
    }

    private void CreateChunk(BoardAnchor boardAnchor)
    {
        var chunk = Instantiate(_chunkPrefab, boardAnchor.GetNewPosition(), Quaternion.identity);
        Debug.Log("anchor get new pos" + boardAnchor.GetNewPosition());
        chunk.GetComponent<ChunkModel>().SetChunkCreator(gameObject);
        chunk.GetComponent<ChunkModel>().SetAnchorPosition(chunk, GetOppositeName(boardAnchor.Name));
        chunk.transform.SetParent(transform);

        foreach (var item in _offsets)
        {
            Vector3 vector = boardAnchor.GetNewPosition() + new Vector3(item.Value[0], item.Value[1], item.Value[2]);
            GameObject chunkTile = Instantiate(_chunkTilePrefab, vector, Quaternion.identity);
            CreateDecorations(chunkTile);
            CreateObstacles(chunkTile);
            chunkTile.transform.parent = chunk.transform;
        }
    }

    private string GetOppositeName(string name)
    {
        return _oppositeNames[name];
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
            decoration.transform.position += new Vector3(randomX, 1, randomY);
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
