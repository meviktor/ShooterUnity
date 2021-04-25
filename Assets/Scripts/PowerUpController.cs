using System;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public PowerUpType powerUpType;

    private PlayerController _playerController;
    // TODO: think about making this variable configurable (public)!
    private const int AMMO_BULLET_COUNT = 25;

    private void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        switch (powerUpType)
        {
            case PowerUpType.MedKit:
                AnimateMedKit();
                break;
            case PowerUpType.Ammo:
                AnimateAmmo();
                break;
            default:
                throw new Exception($"Unkwon type of power-up (value: '{powerUpType}')! No animation set!");
        }
    }

    // TODO: if an enemy "picked up" the power-up pls ignore!!!
    private void OnTriggerEnter(Collider other)
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
            default:
                throw new Exception($"Unkwon type of power-up (value: '{powerUpType}')! Collison not handled!");
        }

        if (pickedUp)
        {
            Destroy(gameObject);
        }
    }

    private void AnimateMedKit()
    {
        // TODO: add animation
    }

    private void AnimateAmmo()
    {
        // TODO: add animation
    }

    private bool HandleMedKitPickUp() => _playerController.IncreaseHealth();
    private void HandleAmmoPickUp() => _playerController.AddAmmo(AMMO_BULLET_COUNT);
}

public enum PowerUpType
{
    MedKit = 1,
    Ammo = 2
}