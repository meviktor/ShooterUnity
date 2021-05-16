using System;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public BulletType bulletType;

    private float _playerBulletLifeTime = 1.5f;
    private float _enemyBulletLifeTime = 3.0f;
    private float _lifeTimeCounter = 0.0f;

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        _lifeTimeCounter += Time.deltaTime;

        if(_lifeTimeCounter > (bulletType == BulletType.Enemy ? _enemyBulletLifeTime : _playerBulletLifeTime))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (bulletType)
        {
            case BulletType.Enemy:
                HandleBulletCollisionWithGameCharacter(other, "Player");
                break;
            case BulletType.Player:
                HandleBulletCollisionWithGameCharacter(other, "Enemy");
                break;
            default:
                throw new Exception($"Unknown type of bullet collided with the game object named '{other.gameObject.name}'.");
        }
    }

    private void HandleBulletCollisionWithGameCharacter(Collider other, string enemyCharacterTag)
    {
        if (other.gameObject.CompareTag(enemyCharacterTag))
        {
            IGameCharacterControllerBase gameCharacterController = enemyCharacterTag switch
            {
                "Player" => other.gameObject.GetComponent<PlayerController>(),
                "Enemy"  => (IGameCharacterControllerBase)other.gameObject.GetComponent<EnemyController>() ?? other.gameObject.GetComponent<EnemyBossController>(),
                _ => throw new ArgumentException($"Unknown type of character ({enemyCharacterTag}), cannot handle bullet collision!", nameof(enemyCharacterTag)),
            };

            if (gameCharacterController.DecreaseHealth())
            {
                Debug.Log($"{enemyCharacterTag}'s health decreased by 1.");
            }
        }

        var parentGameObj = other.gameObject.transform.parent?.gameObject;

        if (new string[] { other.gameObject.tag, parentGameObj?.tag ?? string.Empty }.Any(tag => new string[] { enemyCharacterTag, "Building", "Obstacle" }.Contains(tag)))
        {
            Destroy(gameObject);
        }
    }
}