using UnityEngine;

public class PlayerController : GameCharacterControllerBase
{
    private int _ammo;
    private Vector3 _inputVector;

    protected override Vector3 BulletSpawnOffset => new Vector3(0.3f, 0.8f, 0.75f);
    protected override string UnderlyingPrefabName => "TT_demo_police";

    protected override void Init()
    {
        base.Init();
        _ammo = 0;
    }

    protected override void DoWhileAlive()
    {
        #region Moving & moving animation
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        _inputVector = new Vector3(horizontalInput * moveSpeed, _rigidbody.velocity.y, verticalInput * moveSpeed);

        transform.LookAt(transform.position + new Vector3(_inputVector.x, 0, _inputVector.z));

        _animator.SetFloat("f_speed", _rigidbody.velocity.magnitude);
        #endregion

        #region Shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // TODO: delete if there will be no shooting animation (along with the parameter in the animator)!
            _animator.SetTrigger("trig_shooting");
            Shoot();
        }
        #endregion
    }

    protected override void DoWhenDying()
    {
        Die();
        Debug.Log("The player is dead. GAME OVER.");
    }

    protected override void FixedUpdateImpl()
    {
        _rigidbody.velocity = _inputVector;
    }

    private void Shoot()
    {
        if (AmmoLeft())
        {
            Instantiate(bullet, transform.position + BulletSpawnOffset, transform.rotation);
            _ammo--;
        }
    }

    private bool AmmoLeft() => _ammo > 0;
    public void AddAmmo(int bulletCount) => _ammo += bulletCount;

    public bool IncreaseHealth()
    {
        if (!HasFullHealh())
        {
            _health++;
            return true;
        }
        return false;
    }
    private bool HasFullHealh() => _health == (int)strength;

    protected override void OnTriggerEnterImpl(Collider other)
    {
    }

    protected override void OnCollisionEnterImpl(Collision collision)
    {
    }
}