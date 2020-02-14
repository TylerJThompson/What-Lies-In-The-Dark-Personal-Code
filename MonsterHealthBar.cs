using UnityEngine;

public class MonsterHealthBar : MonoBehaviour
{
    private enum BarNumber { HealthBar1, HealthBar2, HealthBar3, HealthBar4 };

    [SerializeField] MonsterXboxController monster;
    [SerializeField] BarNumber number;
    [SerializeField] float modifier;

    private float maxHP;
    private string monsterName;

    // Start is called before the first frame update
    void Start()
    {
        if (monster != null) maxHP = monster.GetHealth();
        monsterName = "XboxMonster" + number.ToString().Substring(number.ToString().Length - 1) + "(Clone)";
    }

    // Update is called once per frame
    void Update()
    {
        if (monster != null) transform.localScale = new Vector3(Mathf.Max(0f, monster.GetHealth() / maxHP), 1f, 1f) * modifier;
        else
        {
            GameObject monsterObject = GameObject.Find(monsterName);
            if (monsterObject != null)
            {
                monster = monsterObject.GetComponentInChildren<MonsterXboxController>();
                if (monster != null) maxHP = monster.GetHealth();
            }
        }
    }
}
