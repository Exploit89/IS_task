using UnityEngine;

/// <summary>
/// Якорь границы чанка. Всего у каждого чанка 8 границ, 4 по сторонам и 4 по углам. Якорь задает направление новой
/// позиции для генерации чанка.
/// </summary>
/// 
public class BoardAnchor : MonoBehaviour
{
    private ChunkCreator _chunkCreator;
    private string _name;
    private int _anchorOffset = 40;

    public string Name => _name;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider != null && collider.TryGetComponent(out Player player))
        {
            _chunkCreator.TryCreateChunk(gameObject.GetComponent<BoardAnchor>());
        }
    }

    public void SetChunkCreator(ChunkCreator chunkCreator)
    {
        _chunkCreator = chunkCreator;
    }

    public Vector3 GetNewPosition()
    {
        Vector3 position = transform.position;

        switch (_name)
        {
            case "up":
                position += new Vector3(0,0, _anchorOffset);
                break;
            case "up-right":
                position += new Vector3(_anchorOffset, 0, _anchorOffset);
                break;
            case
                "right":
                position += new Vector3(_anchorOffset, 0, 0);
                break;
            case "down-right":
                position += new Vector3(_anchorOffset, 0, -_anchorOffset);
                break;
            case "down":
                position += new Vector3(0, 0, -_anchorOffset);
                break;
            case "down-left":
                position += new Vector3(-_anchorOffset, 0, -_anchorOffset);
                break;
            case "left":
                position += new Vector3(-_anchorOffset, 0, 0);
                break;
            case "up-left":
                position += new Vector3(-_anchorOffset, 0, _anchorOffset);
                break;
            default:
                break;
        }
        return position;
    }
    public BoardAnchor GetAnchor()
    {
        return gameObject.GetComponent<BoardAnchor>();
    }

    public void SetNames(string name)
    {
        _name = name;
    }
}
