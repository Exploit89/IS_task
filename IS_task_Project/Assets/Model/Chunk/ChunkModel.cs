using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkModel : MonoBehaviour
{
    private ChunkCreator _chunkCreator;
    private MeshRenderer _meshRenderer;
    private List<DecorationModel> _decorations;
    private List<ObstacleModel> _obstacles;
    private Dictionary<string, ChunkModel> _neighbours;
    private Player _player;
    private Vector3 _anchorPosition;

    public void SetAnchorPosition(GameObject anchor)
    {
        _anchorPosition = anchor.transform.position;
    }
}
