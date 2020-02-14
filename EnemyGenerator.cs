using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemy;
    public GameManager gm;
    public GameObject curEnemy;
    private bool spawnMonster = true;
    private bool showTotemText = true;

    [SerializeField] GameObject totemText, respawnText;
    [SerializeField] Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownText != null && countdownText.enabled == true)
        {
            if (Time.timeSinceLevelLoad >= 4.5f) countdownText.enabled = false;
            else if (Time.timeSinceLevelLoad >= 3f) countdownText.text = "Start!";
            else if (Time.timeSinceLevelLoad >= 1.5f) countdownText.text = "Set!";
        }
        if (gm != null)
        {
            if (gm.levelReady) GenEnemy();
            if (!spawnMonster && curEnemy == null) respawnText.SetActive(true);
            else if (respawnText != null) respawnText.SetActive(false);
        }
    }

    private void GenEnemy()
    {
        if (spawnMonster && curEnemy == null && enemy != null)
        {
            curEnemy = Instantiate(enemy);
            CameraFollow follow = curEnemy.GetComponentInChildren<CameraFollow>();
            if (follow != null) follow.SetGenerator(gameObject.GetComponent<EnemyGenerator>());
            else
            {
                HumanController human = curEnemy.GetComponentInChildren<HumanController>();
                if (human != null) human.SetGenerator(gameObject.GetComponent<EnemyGenerator>());
            }
            curEnemy.transform.position = transform.position;
            curEnemy.transform.rotation = transform.rotation;
        }
    }

    public void ForcedGenEnemy()
    {
        if (curEnemy == null && enemy != null)
        {
            curEnemy = Instantiate(enemy);
            CameraFollow follow = curEnemy.GetComponentInChildren<CameraFollow>();
            if (follow != null) follow.SetGenerator(gameObject.GetComponent<EnemyGenerator>());
            else
            {
                HumanController human = curEnemy.GetComponentInChildren<HumanController>();
                if (human != null) human.SetGenerator(gameObject.GetComponent<EnemyGenerator>());
            }
            curEnemy.transform.position = transform.position;
            curEnemy.transform.rotation = transform.rotation;
        }
    }

    public bool StopSpawning()
    {
        if (!spawnMonster) return false;
        Light totemLight = GetComponentInChildren<Light>(true);
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null) ps.Stop();
        if (totemLight != null) totemLight.gameObject.SetActive(false);
        spawnMonster = false;
        if (showTotemText && totemText != null) StartCoroutine(WarnTotemDestroyed());
        return true;
    }

    public void StartSpawning()
    {
        Light totemLight = GetComponentInChildren<Light>(true);
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null) ps.Play();
        if (totemLight != null) totemLight.gameObject.SetActive(true);
        spawnMonster = true;
    }

    public bool StillSpawning()
    {
        return spawnMonster;
    }

    public void SetCurEnemyNull()
    {
        curEnemy = null;
    }

    private IEnumerator WarnTotemDestroyed()
    {
        showTotemText = false;
        totemText.SetActive(true);
        yield return new WaitForSeconds(3f);
        totemText.SetActive(false);
    }
}
