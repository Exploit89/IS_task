using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkModel : MonoBehaviour
{
    private ChunkCreator _chunkCreator;
    private MeshRenderer _meshRenderer;
    private DecorationModel _decorationModel;
    private ObstacleModel _obstacleModel;
    private Dictionary<string, ChunkModel> _neighbours;
    private Player _player;
}
