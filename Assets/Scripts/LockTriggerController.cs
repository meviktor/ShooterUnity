using System;
using UnityEngine;

public class LockTriggerController : MonoBehaviour
{
    public event EventHandler PlayerEnteredInArena;

    private GameObject _triggeredLock;
    private GameObject _player;
    private bool _triggeredLockActive = false;

    void Start()
    {
        _triggeredLock = GameObject.Find("TriggeredLock");
        _triggeredLock.SetActive(false);

        _player = GameObject.Find("Player");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player && !_triggeredLockActive)
        {
            _triggeredLock.SetActive(true);
            _triggeredLockActive = true;
            OnPlayerEnteredInArena(EventArgs.Empty);
        }
    }

    protected void OnPlayerEnteredInArena(EventArgs e) => PlayerEnteredInArena?.Invoke(this, e);
}
