using System;
using UnityEngine;

public interface IGameCharacterControllerBase
{
    bool DecreaseHealth(int unit = 1);
}

public abstract class GameCharacterControllerBase : MonoBehaviour, IGameCharacterControllerBase
{
    public float moveSpeed;
    public CharacterStrength strength;
    public GameObject bullet;
    public int Health { get { return _health; } }

    /// <summary>
    /// The prefab from TonnyTinyPeople package (policeman, zombie, female, male).
    /// </summary>
    protected GameObject _underlyingGameObject;
    protected Animator _animator;
    /// <summary>
    /// The Rigidbody belongs to the actual character (player or enemy).
    /// </summary>
    protected Rigidbody _rigidbody;
    protected int _health;

    private bool _initHappend = false;

    protected abstract Vector3 BulletSpawnOffset { get; }
    protected abstract string UnderlyingPrefabName { get; }

    /// <summary>
    /// You can init the fields of the script here.
    /// When overriding it in a derived class, don't forget to call the base class implementation to init the fields of the base class as well!
    /// </summary>
    protected virtual void Init()
    {
        _underlyingGameObject = gameObject.transform.Find(UnderlyingPrefabName).gameObject;

        if (_underlyingGameObject != null)
        {
            _animator = _underlyingGameObject.GetComponent<Animator>();
        }
        else throw new ArgumentException($"Could not find the underlying game object named '{UnderlyingPrefabName}' under '${gameObject.name}'", nameof(UnderlyingPrefabName));

        _health = (int)strength;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// You can specify here what the character have to do if he/she is alive. This method will be called by every frame (by the Update method of the base).
    /// </summary>
    protected abstract void DoWhileAlive();

    /// <summary>
    /// You can specify here what the character have to do if he/she is about to die. This method will be called by every frame (by the Update method of the base).
    /// </summary>
    protected abstract void DoWhenDying();

    /// <summary>
    /// Here comes the code you would write in the FixedUpdate() Unity message.
    /// </summary>
    protected abstract void FixedUpdateImpl();

    /// <summary>
    /// Here comes the code you would write in the OnTriggerEnter() Unity message.
    /// </summary>
    protected abstract void OnTriggerEnterImpl(Collider other);

    /// <summary>
    /// Here comes the code you would write in the OnCollisionEnter() Unity message.
    /// </summary>
    protected abstract void OnCollisionEnterImpl(Collision collision);

    private void Update()
    {
        if (!_initHappend)
        {
            Init();
            _initHappend = true;
        }
        else
        {
            if (!IsDead())
            {
                DoWhileAlive();
            }
            else DoWhenDying();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_initHappend)
        {
            OnTriggerEnterImpl(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_initHappend)
        {
            OnCollisionEnterImpl(collision);
        }
    }

    private void FixedUpdate()
    {
        if (_initHappend)
        {
            FixedUpdateImpl();
        }
    }

    protected virtual void Die()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        _animator.SetBool("b_isDead", true);
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            if (gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
            }
            else gameObject.SetActive(false);
        }
    }

    public virtual bool DecreaseHealth(int unit = 1)
    {
        if (!IsDead())
        {
            if (_health >= unit)
            {
                _health -= unit;
            }
            else _health -= 0;
            return true;
        }
        return false;
    }

    protected bool IsDead() => _health == 0;
}

public enum CharacterStrength
{
    Weak = 3,
    Normal = 5,
    Strong = 7,
    Boss = 15
}

public enum BulletType
{
    Player = 1,
    Enemy = 2
}
