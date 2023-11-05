using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float _distanceToBoard = 30;

    public void SetAnchorPosition(GameObject anchor)
    {
        _mainAnchorPosition = anchor.transform.position;
        _anchors = new Dictionary<string, float[]>
        {
            { "up", new float[] { _distanceToBoard, 0, 0 } },
            { "up-right", new float[] { _distanceToBoard, 0, _distanceToBoard } },
            { "right", new float[] { 0, 0, _distanceToBoard } },
            { "down-right", new float[] { -_distanceToBoard, 0, _distanceToBoard } },
            { "down", new float[] { -_distanceToBoard, 0, 0 } },
            { "down-left", new float[] { -_distanceToBoard, 0, -_distanceToBoard } },
            { "left", new float[] { 0, 0, -_distanceToBoard } },
            { "up-left", new float[] { _distanceToBoard, 0, -_distanceToBoard } }
        };

        foreach (var item in _anchors)
        {
            Vector3 vector = new Vector3(item.Value[0], item.Value[1], item.Value[2]);
            var boardAnchor = Instantiate(_boardAnchorPrefab, vector, Quaternion.identity);
            boardAnchor.transform.parent = anchor.transform;
        }
    }
}
