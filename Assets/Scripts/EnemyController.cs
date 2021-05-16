using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : GameCharacterControllerBase
{
    public EnemyType enemyType;
    public float walkDistance = 5.0f;
    public float playerScanRadius = 15.0f;
    public float shootIntervalInSeconds = 1.0f;
    public float attackRunSpeed = 6.0f;

    //TODO: change this to private!
    public float distanceFromSpawnPoint;

    protected override Vector3 BulletSpawnOffset => new Vector3(0, 0.75f, transform.forward.z);
    protected override string UnderlyingPrefabName => GetEnemyPrefabName();

    private bool _chasingPlayer;
    private Vector3? _spawnPosition;
    private Vector3 _followPlayerVector;
    private GameObject _player;
    private float _bulletDiameter;
    /// <summary>
    /// Used for turing back at the end of the walk path
    /// </summary>
    public int _directionSign;

    /// <summary>
    /// Timer used by the enemy types male and female.
    /// </summary>
    private ActionTimer _actionTimer;

    /// <summary>
    /// Forward, backward, left, right
    /// </summary>
    private Vector3[] _offsetsForLinecast = new Vector3[] {
        new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0)
    };

    protected override void Init()
    {
        base.Init();

        if (_spawnPosition == null)
        {
            _spawnPosition = transform.position;
        }

        _chasingPlayer = false;
        _player = GameObject.Find("Player");
        _actionTimer = new ActionTimer(1f);
        
        _bulletDiameter = bullet.GetComponent<MeshRenderer>().bounds.extents.magnitude;

        _directionSign = 1;
    }

    protected override void DoWhileAlive()
    {
        _animator.SetFloat("f_speed", _rigidbody.velocity.magnitude);
        _actionTimer.AddElapsedTime(Time.deltaTime);

        if (!_chasingPlayer)
        {
            distanceFromSpawnPoint = Vector3.Distance(_spawnPosition.Value, transform.position);
            if (WalkPathEnded())
            {
                transform.Rotate(new Vector3(0, 1, 0), 180.0f);
                _directionSign *= -1;
            }
            if (PlayerIsNearby() && !ObjectBetweenEnemyAndPlayer())
            {
                _chasingPlayer = true;
                if (enemyType == EnemyType.Zombie)
                {
                    StartCoroutine(ZombieShootRoutine());
                }
            }
        }
        else
        {
            _followPlayerVector = _player.transform.position - transform.position;
            transform.LookAt(transform.position + new Vector3(_followPlayerVector.x, 0, _followPlayerVector.z));
        }
    }

    protected override void DoWhenDying()
    {
        Die();
        Debug.Log("You killed an enemy.");
    }

    protected override void FixedUpdateImpl()
    {
        if (!_chasingPlayer)
        {
            // Walking the direction whre the enemy is looking at
            _rigidbody.velocity = transform.forward * moveSpeed;
        }
        else
        {
            // Moving towards the player
            _rigidbody.velocity = new Vector3(_followPlayerVector.x, _rigidbody.velocity.y, _followPlayerVector.z).normalized * attackRunSpeed;
        }
    }

    protected override void OnCollisionEnterImpl(Collision collision)
    {
        if(collision.gameObject != _player && !WalkPathEnded())
        {
            transform.Rotate(new Vector3(0, 1, 0), 180.0f);
            _directionSign *= -1;
        }
        if(collision.gameObject == _player && EnemyIsNotZombie())
        {
            if (_actionTimer.CanDoAction() && _actionTimer.TryDoAction(() => _player.GetComponent<PlayerController>().DecreaseHealth()))
            {
               Debug.Log("Player's health is decreased by 1.");
            }
        }
    }

    private IEnumerator ZombieShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootIntervalInSeconds);
            Instantiate(bullet, transform.position + BulletSpawnOffset, transform.rotation);
            Instantiate(bullet, transform.position + BulletSpawnOffset + new Vector3(_bulletDiameter, 0,  _bulletDiameter), transform.rotation);
            Instantiate(bullet, transform.position + BulletSpawnOffset + new Vector3(-_bulletDiameter, 0, _bulletDiameter), transform.rotation);
        }
    }

    private bool WalkPathEnded() => Vector3.Distance(_spawnPosition.Value, transform.position) > walkDistance;
    private bool PlayerIsNearby() => (_player?.activeSelf ?? false) ? Vector3.Distance(transform.position, _player.transform.position) <= playerScanRadius : false;
    private bool ObjectBetweenEnemyAndPlayer()
    {
        var playerTargetPosition = new Vector3(_player.transform.position.x, 0.5f, _player.transform.position.z);
        foreach (var offset in _offsetsForLinecast)
        {
            // TODO: the 0.5f value is used because of the negative y value of the player's transform (that's why the raycast finds the ground instead of the player?)
            // some other method should be chosen for the "long-term fix"
            if (Physics.Linecast(transform.position + offset, playerTargetPosition, out RaycastHit objectHit))
            {
                var blockingTags = new string[] { gameObject.tag, "Building", "Obstacle" };
                if (!blockingTags.Contains(objectHit.transform.gameObject.tag))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private string GetEnemyPrefabName()
    {
        return enemyType switch
        {
            EnemyType.Zombie => "TT_demo_zombie",
            EnemyType.Female => "TT_demo_female",
            EnemyType.Male => "TT_demo_male_A",
            _ => throw new InvalidOperationException($"Not known enemy type: {enemyType}! Cannot return the prefab name."),
        };
    }

    private bool EnemyIsNotZombie() => enemyType != EnemyType.Zombie;

    protected override void OnTriggerEnterImpl(Collider other)
    {
    }
}

public enum EnemyType
{
    Male = 1,
    Female = 2,
    Zombie = 3
}

// TODO: this could be replaced with StartCoroutine and a boolean variable
public class ActionTimer
{
    private float _elapsedTimeInSec;
    private readonly float _timeTreshold;

    public ActionTimer(float timeTreshold) => _timeTreshold = timeTreshold;

    public float AddElapsedTime(float elapsedTime) => elapsedTime > 0 ? _elapsedTimeInSec += elapsedTime : _elapsedTimeInSec;

    public bool CanDoAction() => _elapsedTimeInSec > _timeTreshold;

    public T TryDoAction<T>(Func<T> action)
    {
        T result = default;
        if (CanDoAction())
        {
            result = action();
            _elapsedTimeInSec = 0;
        }
        return result;
    }

    public void TryDoAction(Action action)
    {
        if (CanDoAction())
        {
            action();
            _elapsedTimeInSec = 0;
        }
    }
}
