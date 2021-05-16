using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBossController : GameCharacterControllerBase
{
    public event EventHandler BossDefeated;

    public GameObject[] enemiesToSpawn;
    /// <summary>
    /// These positions are calculated from the position of the boss.
    /// </summary>
    public List<Vector3> possibleSpawnPositions;
    public int numberOfSpawningEnemies = 3;
    public float shootInterval = 10f;
    public float enemySpawnInterval = 20f;

    private BoxCollider _myCollider;
    private Vector3 _center;
    private float _expansionX;
    private float _expansionZ;
    private ActionTimer _shootActionTimer;
    private ActionTimer _enemySpawnActionTimer;

    // TODO: do something with this!
    protected override Vector3 BulletSpawnOffset => throw new NotImplementedException();

    protected override string UnderlyingPrefabName => "TT_demo_male_B";

    protected override void Init()
    {
        base.Init();

        _myCollider = GetComponent<BoxCollider>();

        _center = _myCollider.bounds.center;
        _expansionX = _myCollider.bounds.size.x;
        _expansionZ = _myCollider.bounds.size.y;

        _shootActionTimer = new ActionTimer(shootInterval);
        _enemySpawnActionTimer = new ActionTimer(enemySpawnInterval);

        DoWhenSpawn();
    }

    protected override void DoWhenDying()
    {
        Die();
        Debug.Log("You killed the boss!");
    }

    protected override void DoWhileAlive()
    {
        _enemySpawnActionTimer.AddElapsedTime(Time.deltaTime);
        _shootActionTimer.AddElapsedTime(Time.deltaTime);

        _enemySpawnActionTimer.TryDoAction(() => SpawnEnemies(numberOfSpawningEnemies));
        _shootActionTimer.TryDoAction(Shoot);
    }

    protected override void Die()
    {
        // Killing all enemies and itself at the end
        foreach(var enemy in GameObject.FindGameObjectsWithTag("Enemy").Where(enemy => enemy != gameObject))
        {
            var enemyController = enemy.GetComponent<EnemyController>();
            enemyController.DecreaseHealth(enemyController.Health);
        }
        gameObject.SetActive(false);

        OnBossDefeated(EventArgs.Empty);
    }

    private void DoWhenSpawn()
    {
        // To be able to shhot and spwan enemies right after creation
        _shootActionTimer.AddElapsedTime(shootInterval);
        _enemySpawnActionTimer.AddElapsedTime(enemySpawnInterval);
    }

    public void SpawnEnemies(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = UnityEngine.Random.Range(0, enemiesToSpawn.Length);
            Instantiate(enemiesToSpawn[enemyIndex], transform.position + possibleSpawnPositions[i], transform.rotation);
        }
    }

    public void Shoot()
    {
        var longerSideLength = _expansionX > _expansionZ ? _expansionX : _expansionZ;
        var baseVector = new Vector3(longerSideLength / 2, 0, 0);

        for(float degree = 0.0f; degree < 360.0f; degree += 45.0f)
        {
            var bulletSpawnPosition = (Quaternion.Euler(0, degree, 0) * baseVector) + new Vector3(_center.x, 0.5f, _center.z);
            Instantiate(bullet, bulletSpawnPosition, Quaternion.Euler(0, degree, 0));
        }
    }

    protected override void FixedUpdateImpl()
    {
    }

    protected override void OnCollisionEnterImpl(Collision collision)
    {
    }

    protected override void OnTriggerEnterImpl(Collider other)
    {
    }

    protected void OnBossDefeated(EventArgs e) => BossDefeated?.Invoke(this, e);
}
