using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private GameObject _player;
    private PlayerController _playerController;
    private GameObject _enemyBoss;
    private GameObject _crazyHorse;
    private GameObject _bossAreaLockTrigger;

    private Text _ui_health;
    private Text _ui_ammo;
    private GameObject _ui_key;

    private GameObject _ui_msgYouWon;
    private GameObject _ui_msgGameOver;
    private GameObject _ui_msgFindKey;
    private GameObject _ui_msgKillBoss;
    private GameObject _ui_msgRetry;

    void Start()
    {
        #region Game objects

        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _enemyBoss = GameObject.Find("EnemyBoss");
        _crazyHorse = GameObject.Find("CrazyHorse");
        _bossAreaLockTrigger = GameObject.Find("LockTrigger");

        _enemyBoss.SetActive(false);
        _crazyHorse.SetActive(false);

        #endregion

        #region UI info panel

        _ui_health = GameObject.Find("UI_Health").GetComponent<Text>();
        _ui_ammo = GameObject.Find("UI_Ammo").GetComponent<Text>();
        _ui_key = GameObject.Find("UI_KeyIcon");

        _ui_key.SetActive(false);
        _ui_ammo.text = _playerController.Ammo.ToString();
        _ui_health.text = ((int)_playerController.strength).ToString();

        #endregion

        #region UI messages

        _ui_msgYouWon = GameObject.Find("Msg_YouWon");
        _ui_msgGameOver = GameObject.Find("Msg_GameOver");
        _ui_msgFindKey = GameObject.Find("Msg_FindKey");
        _ui_msgKillBoss = GameObject.Find("Msg_KillBoss");
        _ui_msgRetry = GameObject.Find("Msg_Retry");

        _ui_msgYouWon.SetActive(false);
        _ui_msgGameOver.SetActive(false);
        _ui_msgFindKey.SetActive(true);
        _ui_msgKillBoss.SetActive(false);
        _ui_msgRetry.SetActive(false);
        StartCoroutine(ChangeActiveState(_ui_msgFindKey, false, 3.5f));

        #endregion

        #region event handlers

        _enemyBoss.GetComponent<EnemyBossController>().BossDefeated += (sender, args) =>
        {
            _crazyHorse.SetActive(true);

            _ui_msgYouWon.SetActive(true);
            _ui_msgRetry.SetActive(true);
        };
        _bossAreaLockTrigger.GetComponent<LockTriggerController>().PlayerEnteredInArena += (sender, args) => _enemyBoss.SetActive(true);

        _playerController.KeyFound += (sender, args) => {
            if (_playerController.HasKey)
            {
                _ui_key.SetActive(true);

                _ui_msgKillBoss.SetActive(true);
                StartCoroutine(ChangeActiveState(_ui_msgKillBoss, false, 3.5f));
            }
        };
        _playerController.AmmoAmountChanged += (sender, args) => _ui_ammo.text = _playerController.Ammo.ToString();
        _playerController.HealthChanged += (sender, args) => {
            _ui_health.text = _playerController.Health.ToString();
            if (_playerController.Health == 0)
            {
                _ui_msgGameOver.SetActive(true);
                _ui_msgRetry.SetActive(true);
            }
        };

        #endregion
    }

    private IEnumerator ChangeActiveState(GameObject targetObj, bool targetState, float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds);
        targetObj.SetActive(targetState);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && (_ui_msgGameOver.activeInHierarchy || _ui_msgYouWon.activeInHierarchy))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}