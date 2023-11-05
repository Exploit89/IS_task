using System.Collections.Generic;
using UnityEngine;

public class ChunkModel : MonoBehaviour
{
    [SerializeField] private GameObject _boardAnchorPrefab;

    private ChunkCreator _chunkCreator;
    private MeshRenderer _meshRenderer;
    private List<DecorationModel> _decorations;
    private List<ObstacleModel> _obstacles;
    private Dictionary<string, ChunkModel> _neighbours;
    private Player _player;
    private Vector3 _mainAnchorPosition;
    private Dictionary<string, float[]> _anchors;
    private float _distanceToBoard = 20;

    public Vector3 MainAnchorPosition => _mainAnchorPosition;

    public void SetAnchorPosition(GameObject anchor, string anchorName = "empty")
    {
        _mainAnchorPosition = anchor.transform.position;
        _anchors = new Dictionary<string, float[]>
        {
            { "up", new float[] { 0, 0, _distanceToBoard } },
            { "up-right", new float[] { _distanceToBoard, 0, _distanceToBoard } },
            { "right", new float[] { _distanceToBoard, 0, 0 } },
            { "down-right", new float[] { _distanceToBoard, 0, -_distanceToBoard } },
            { "down", new float[] { 0, 0, -_distanceToBoard } },
            { "down-left", new float[] { -_distanceToBoard, 0, -_distanceToBoard } },
            { "left", new float[] { -_distanceToBoard, 0, 0 } },
            { "up-left", new float[] { -_distanceToBoard, 0, _distanceToBoard } }
        };

        foreach (var item in _anchors)
        {
            if (item.Key != anchorName)
            {
                Vector3 vector = new Vector3(item.Value[0], item.Value[1], item.Value[2]);
                vector += anchor.transform.position;
                var boardAnchor = Instantiate(_boardAnchorPrefab, vector, Quaternion.identity);
                boardAnchor.transform.parent = anchor.transform;
                boardAnchor.GetComponent<BoardAnchor>().SetChunkCreator(_chunkCreator);
                boardAnchor.GetComponent<BoardAnchor>().SetNames(item.Key);
            }
        }
    }

    public void SetChunkCreator(GameObject chunkCreator)
    {
        _chunkCreator = chunkCreator.GetComponent<ChunkCreator>();
    }
}
