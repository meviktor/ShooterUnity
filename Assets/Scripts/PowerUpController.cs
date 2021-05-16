using System;
using UnityEngine;
using UnityEngine.Animations;

public class PowerUpController : MonoBehaviour
{
    public PowerUpType powerUpType;
    /// <summary>
    /// For ammo means the number of bullets, for medkit means the health unit recoverd
    /// </summary>
    public int power;

    private GameObject _player;
    private PlayerController _playerController;
    // TODO: think about making this variable configurable (public)!
    

    private void Start()
    {
        _player = GameObject.Find("Player");
        _playerController =_player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        switch (powerUpType)
        {
            case PowerUpType.MedKit:
                AnimateMedKit(Time.deltaTime);
                break;
            case PowerUpType.Ammo:
                AnimateAmmo(Time.deltaTime);
                break;
            case PowerUpType.Key:
                AnimateKey(Time.deltaTime);
                break;
            default:
                throw new Exception($"Unkwon type of power-up (value: '{powerUpType}')! No animation set!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _player)
        {
            bool pickedUp = true;

            switch (powerUpType)
            {
                case PowerUpType.MedKit:
                    pickedUp = HandleMedKitPickUp();
                    break;
                case PowerUpType.Ammo:
                    HandleAmmoPickUp();
                    break;
                case PowerUpType.Key:
                    HandleKeyPickUp();
                    break;
                default:
                    throw new Exception($"Unkwon type of power-up (value: '{powerUpType}')! Collison not handled!");
            }

            if (pickedUp)
            {
                Destroy(gameObject);
            }
        }
    }

    private void AnimatePowerUp(float animateLoopIsSec, float deltaTime)
    {
        var rotationDegree = (deltaTime / animateLoopIsSec) * 360.0f;
        switch(GetRotationAxis())
        {
            case Axis.X:
                gameObject.transform.Rotate(rotationDegree, 0f, 0f);
                break;
            case Axis.Y: 
                gameObject.transform.Rotate(0f, rotationDegree, 0f);
                break;
            case Axis.Z:
                gameObject.transform.Rotate(0f, 0f, rotationDegree);
                break;
            case Axis.None:
                break;
        };
    }

    private void AnimateMedKit(float deltaTime) => AnimatePowerUp(10f, deltaTime);

    private void AnimateAmmo(float deltaTime) => AnimatePowerUp(6f, deltaTime);

    private void AnimateKey(float deltaTime) => AnimatePowerUp(12f, deltaTime);

    private bool HandleMedKitPickUp() => _playerController.IncreaseHealth(power);
    private void HandleAmmoPickUp() => _playerController.AddAmmo(power);
    private void HandleKeyPickUp() => _player.GetComponent<PlayerController>().HasKey = true;

    private Axis GetRotationAxis() => powerUpType switch
    {
        PowerUpType.MedKit => Axis.Y,
        PowerUpType.Key => Axis.Y,
        PowerUpType.Ammo => Axis.Z,
        _ => throw new Exception($"Unkwon type of power-up (value: '{powerUpType}')! Cannot get axis for animation!")
    };
        
}

public enum PowerUpType
{
    MedKit = 1,
    Ammo = 2,
    Key = 3
}   