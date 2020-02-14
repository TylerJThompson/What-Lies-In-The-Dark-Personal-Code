using UnityEngine;

public class SetDeathMarker : MonoBehaviour
{
    private GameObject monsterUI;

    void Start()
    {
        monsterUI = null;
    }

    public void SetMonsterUI(GameObject ui)
    {
        monsterUI = ui;
    }

    public void MoveToMonsterUI()
    {
        gameObject.transform.position = monsterUI.transform.position;
        gameObject.transform.rotation = monsterUI.transform.rotation;
    }
}
