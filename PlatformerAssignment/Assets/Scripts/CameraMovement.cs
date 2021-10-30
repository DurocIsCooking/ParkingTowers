using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private float _cameraPositionBuffer = 5; // How far the player can move in a given direction before the camera follows

    // Singleton pattern
    private static CameraMovement _instance;

    public static CameraMovement Instance
    {
        get
        {
            if (_instance == null)
            {
                CameraMovement singleton = GameObject.FindObjectOfType<CameraMovement>();
                if (singleton == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<CameraMovement>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if(_player != null)
        {
            // Gather position values and follow player
            float cameraPosX = transform.position.x;
            float playerPosX = _player.transform.position.x;
            ManagePosition(cameraPosX, playerPosX, true);
            float cameraPosY = transform.position.y;
            float playerPosY = _player.transform.position.y;
            ManagePosition(cameraPosY, playerPosY, false);
        }
        
    }

    // Move with the player
    private void ManagePosition(float cameraPosition, float playerPosition, bool xAxis)
    {
        if(Mathf.Abs(cameraPosition - playerPosition) > _cameraPositionBuffer)
        {
            // Move in negative direction
            if(cameraPosition > playerPosition)
            {
                if(xAxis)
                {
                    transform.position = new Vector3(playerPosition + _cameraPositionBuffer, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, playerPosition + _cameraPositionBuffer, transform.position.z);
                }
            }
            // Move in positive direction
            else
            {
                if (xAxis)
                {
                    transform.position = new Vector3(playerPosition - _cameraPositionBuffer, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, playerPosition - _cameraPositionBuffer, transform.position.z);
                }
            }
        }
    }

    // Attach to new player object after respawn
    public void AttachCameraToPlayer(GameObject newPlayer)
    {
        transform.position = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, transform.position.z);
        _player = newPlayer;
    }

}
