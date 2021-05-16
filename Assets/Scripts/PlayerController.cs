using System;
using UnityEngine;

public class PlayerController : GameCharacterControllerBase
{
    public event EventHandler KeyFound;
    public event EventHandler AmmoAmountChanged;
    public event EventHandler HealthChanged;

    public bool HasKey
    {
        get { return _keyFound; }
        set
        {
            if(_keyFound == false)
            {
                _keyFound = value;
                OnKeyFound(EventArgs.Empty);
            }
        }
    }

    public int Ammo { get { return _ammo; } }

    private int _ammo;
    private Vector3 _inputVector;
    private bool _keyFound;

    protected override Vector3 BulletSpawnOffset => new Vector3(0.3f, 0.8f, 0.75f);
    protected override string UnderlyingPrefabName => "TT_demo_police";

    protected override void Init()
    {
        base.Init();
        _ammo = 0;
        _keyFound = false;
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

            OnAmmoAmountChanged(EventArgs.Empty);
        }
    }

    private bool AmmoLeft() => _ammo > 0;
    public void AddAmmo(int bulletCount)
    {
        _ammo += bulletCount;
        OnAmmoAmountChanged(EventArgs.Empty);
    }

    public bool IncreaseHealth(int unit = 1)
    {
        if (!HasFullHealh())
        {
            if ((int)strength < _health + unit)
            {
                _health = (int)strength;
            }
            else _health += unit;

            OnHealthChanged(EventArgs.Empty);

            return true;
        }
        return false;
    }

    public override bool DecreaseHealth(int unit = 1)
    {
        var res = base.DecreaseHealth(unit);
        OnHealthChanged(EventArgs.Empty);
        return res;
    }

    private bool HasFullHealh() => _health == (int)strength;

    protected override void OnTriggerEnterImpl(Collider other)
    {
    }

    protected override void OnCollisionEnterImpl(Collision collision)
    {
    }

    protected void OnKeyFound(EventArgs e) => KeyFound?.Invoke(this, e);

    protected void OnAmmoAmountChanged(EventArgs e) => AmmoAmountChanged?.Invoke(this, e);

    protected void OnHealthChanged(EventArgs e) => HealthChanged?.Invoke(this, e);
}