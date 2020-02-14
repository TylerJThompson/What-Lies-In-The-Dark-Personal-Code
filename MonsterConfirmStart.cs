using UnityEngine;

public class MonsterConfirmStart : MonoBehaviour
{
    private enum PlayerNumber { Player1, Player2, Player3, Player4 };

    [SerializeField] PlayerNumber playerNumber;
    [SerializeField] Animator monster;
    [SerializeField] SpriteRenderer title, start, rightTrigger;
    [SerializeField] Color color;

    private bool ready;
    private string trigger;

    private void Start()
    {
        ready = false;
        trigger = "RightTrigger" + playerNumber.ToString().Substring(playerNumber.ToString().Length - 1);
    }

    private void Update()
    {
        if (Input.GetAxis(trigger) > 0.1f && !ready)
        {
            if (monster!= null) monster.SetTrigger("Start");
            start.color = color;
            rightTrigger.color = color;
            ready = true;
        }
    }

    public bool IsReadyToStart()
    {
        return ready;
    }

    public bool MonsterWasDestroyed()
    {
        return (monster == null);
    }

    public void SetSpritesToBlack()
    {
        title.color = Color.black;
        start.color = Color.black;
        rightTrigger.color = Color.black;
    }
}
