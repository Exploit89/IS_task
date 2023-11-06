using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модель создателя чанков. Содержит логику формирования чанков и наполнения их объектами.
/// Ниже подробнее по коду оставлены комментарии.
/// </summary>
/// 

public class ChunkCreator : MonoBehaviour
{
    [SerializeField] private GameObject _chunkTilePrefab;
    [SerializeField] private List<DecorationModel> _decorations;
    [SerializeField] private List<ObstacleModel> _obstacles;
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private Player _player;

    private Dictionary<string, float[]> _offsets;
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
    private int _decorationCount = 3;
    private int _minDecorationCount = 1;
    private int _chunkSideSize = 20;
    private int _halfChunkSide = 10;
    private float _decorationYOffset = 1;
    private float _raycastOffsetHeight = 5;
    private float raycastLength = 85;

    private void Start()
    {
        CreateStartChunk();
    }

    /// <summary>
    /// TryCreateChunk() вызывается предварительно при пересечении игроком коллайдера якоря.
    /// Логика: проверяет, можно ли вообще по напралению создать новый чанк.
    /// Пускает луч из центра чанка, на котором располагается коллайдер якоря в сторону якоря.
    /// Если луч сталкивается с заданным по слою ChunkAnchorCollider коллайдером другого чанка, на расстоянии
    /// в пределах ближайших чанков, то не создает новый чанк.
    /// В противном случае, если луч ничего не находит - там создается боижайший чанк.
    /// </summary>
    public void TryCreateChunk(BoardAnchor boardAnchor)
    {
        RaycastHit hit;
        Vector3 startVector = boardAnchor.transform.GetComponentInParent<ChunkModel>().MainAnchorPosition;
        Vector3 directionVector = boardAnchor.gameObject.transform.localPosition;
        int layer = LayerMask.NameToLayer("ChunkAnchorCollider");
        int layerMask = 1 << layer;
        startVector.y += _raycastOffsetHeight;
        directionVector.y += _raycastOffsetHeight;

        if (Physics.Raycast(startVector, directionVector, out hit, raycastLength, layerMask)) { }
        else
            CreateChunk(boardAnchor.GetAnchor());
    }

    /// <summary>
    /// CreateStartChunk() создает базовый набор чанков на старт.
    /// Логика: создает 9 тайлов чанка в форме квадрата, из заданных смещений.
    /// Вызывет методы создания декораций и препятствий. Во всех кроме центрального - там спавнится игрок.
    /// </summary>
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

    /// <summary>
    /// CreateChunk() создает рандомный чанк, в месте получаемом от якоря, коллайдер которого пересек игрок.
    /// Логика: создает 9 тайлов чанка в форме квадрата, из заданных смещений.
    /// Вызывет методы создания декораций и препятствий.
    /// Задаёт рандомные цвета тайлам.
    /// Группирует чанки к одному родителю в иерархии на сцене.
    /// </summary>
    private void CreateChunk(BoardAnchor boardAnchor)
    {
        var chunk = Instantiate(_chunkPrefab, boardAnchor.GetNewPosition(), Quaternion.identity);
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
            chunkTile.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        }
    }

    /// <summary>
    /// GetOppositeName() получает название смещения противоположное пересекаемому игроком.
    /// Необходим для корректного рамещения основного якоря для нового чанка.
    /// </summary>
    private string GetOppositeName(string name)
    {
        return _oppositeNames[name];
    }

    /// <summary>
    /// CreateDecorations() содает пересекаемые декорации на тайле чанка, по рандомным из заданных параметров.
    /// Тайловое создание объектов позволяет снизить пересечение декораций или вовсе избавиться от них,
    /// если задать 1 шт на один тайл.
    /// </summary>
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
            decoration.transform.position += new Vector3(randomX, _decorationYOffset, randomY);
        }
    }

    /// <summary>
    /// CreateObstacles() содает непересекаемые перпятствия на тайле чанка, по рандомным из заданных параметров.
    /// Тайловое создание объектов позволяет снизить пересечение декораций или вовсе избавиться от них,
    /// если задать 1 шт на один тайл.
    /// </summary>
    private void CreateObstacles(GameObject chunk)
    {
        int random = Random.Range(0, _obstacles.Count);
        int randomCount = Random.Range(_minDecorationCount, _decorationCount);

        for (int i = 0; i < randomCount; i++)
        {
            int randomX = Random.Range(0, _halfChunkSide);
            int randomY = Random.Range(0, _halfChunkSide);
            int randomRotation = Random.Range(0, 360);
            var obstacle = Instantiate(_obstacles[random], chunk.transform.position, Quaternion.identity);
            obstacle.transform.parent = chunk.transform;
            obstacle.transform.position += new Vector3(randomX, _decorationYOffset, randomY);
            obstacle.transform.rotation = Quaternion.Euler(new Vector3(0, randomRotation, 0));
        }
    }
}
