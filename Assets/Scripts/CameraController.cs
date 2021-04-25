using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offsetFromPlayer;

    private GameObject _player;
   
    private void Start()
    {
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        transform.position = _player.transform.position + offsetFromPlayer;
    }
}
