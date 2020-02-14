using UnityEngine;

public class MonsterClaws : MonoBehaviour
{
    private bool attackEnabled;

    private void Start()
    {
        attackEnabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (attackEnabled && other.gameObject.tag.Equals("Player"))
        {
            HumanController human = other.gameObject.GetComponent<HumanController>();
            if (human != null) human.HitHuman();
        }
    }

    public void EnableAttack()
    {
        attackEnabled = true;
    }

    public void DisableAttack()
    {
        attackEnabled = false;
    }
}
