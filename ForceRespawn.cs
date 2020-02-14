using UnityEngine;

public class ForceRespawn : MonoBehaviour
{
    [SerializeField] EnemyGenerator totem;
    [SerializeField] GameObject[] respawnAlly;
    [SerializeField] ForceRespawn[] otherRespawnAreas;

    private string[] interacts;
    private MonsterXboxController[] monsters;
    private bool[] allowTurnOff;

    private void Start()
    {
        interacts = new string[4];
        monsters = new MonsterXboxController[4];
        allowTurnOff = new bool[] { true, true, true, true };
    }

    private void Update()
    {
        foreach (ForceRespawn other in otherRespawnAreas)
            for (int h = 0; h < monsters.Length; h++)
                if (allowTurnOff[h] && other.LeaveRespawnTextOn(h)) allowTurnOff[h] = false;

        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] == null)
            {
                if (allowTurnOff[i]) respawnAlly[i].SetActive(false);
                interacts[i] = "";
            }
            else
            {
                respawnAlly[i].SetActive(true);
                if (Input.GetButtonDown(interacts[i]))
                {
                    totem.ForcedGenEnemy();
                    for (int j = 0; j < monsters.Length; j++)
                    {
                        respawnAlly[i].SetActive(false);
                        interacts[j] = "";
                        monsters[j] = null;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!totem.StillSpawning() && totem.curEnemy == null && other.gameObject.tag.Equals("Monster"))
        {
            MonsterXboxController monster = other.gameObject.GetComponent<MonsterXboxController>();
            if (monster != null)
            {
                string newInteract = monster.GetInteractButton();
                int index = int.Parse(newInteract.Substring(newInteract.Length - 1)) - 1;
                interacts[index] = newInteract;
                monsters[index] = monster;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Monster"))
        {
            MonsterXboxController monster = other.gameObject.GetComponent<MonsterXboxController>();
            if (monster != null)
            {
                string removeInteract = monster.GetInteractButton();
                int index = int.Parse(removeInteract.Substring(removeInteract.Length - 1)) - 1;
                interacts[index] = "";
                monsters[index] = null;
            }
        }
    }

    public bool LeaveRespawnTextOn(int monsterIndex)
    {
        return (monsters[monsterIndex] != null);
    }
}
