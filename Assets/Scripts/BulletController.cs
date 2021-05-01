using System;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public BulletType bulletType;

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
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

        if(new string[] {enemyCharacterTag, "Building", "Obstacle"}.Contains(other.gameObject.tag))
        {
            Destroy(gameObject);
        }
    }
}