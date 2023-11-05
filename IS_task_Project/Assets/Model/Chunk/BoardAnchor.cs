using System.Collections.Generic;
using UnityEngine;

public class BoardAnchor : MonoBehaviour
{
    private string _name;
    private ChunkCreator _chunkCreator;
    private Player _player;
    private bool _freeChunk = true;
    private int _anchorOffset = 40;

    public string Name => _name;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void SetChunkCreator(ChunkCreator chunkCreator)
    {
        _chunkCreator = chunkCreator;
    }

    public void SetChunkBusy()
    {
        _freeChunk = false;
    }

    public bool IsFreeChunk()
    {
        return _freeChunk;
    }

    public Vector3 GetNewPosition()
    {
        Vector3 position = transform.position;
        Debug.Log(_name + " anchor current pos = " + transform.position);

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

    public void SetColliderPosition()
    {
        gameObject.GetComponent<Collider>().transform.position *= -1;
    }

    public void SetNames(string name)
    {
        _name = name;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider != null && collider.TryGetComponent(out Player player))
        {
            _chunkCreator.TryCreateChunk(gameObject.GetComponent<BoardAnchor>());
        }
    }
}
