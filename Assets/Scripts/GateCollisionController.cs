using UnityEngine;

public class GateCollisionController : MonoBehaviour
{
    private GameObject _player;
    private PlayerController _playerController;

    private GameObject _bossGate;
    
    void Start()
    {
        _bossGate = GameObject.Find("BossGate");
        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _player && _playerController.HasKey)
        {
            _bossGate.GetComponent<BossGateController>().OpenGate();
        }
    }
}
