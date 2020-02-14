using UnityEngine;

public class HumanHealthBar : MonoBehaviour
{
    private enum HeartNumber { Heart1, Heart2, Heart3 };

    [SerializeField] HumanController human;
    [SerializeField] HeartNumber number;

    private int healthRequired;

    // Start is called before the first frame update
    void Start()
    {
        healthRequired = int.Parse(number.ToString().Substring(number.ToString().Length - 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (human.GetHealth() < healthRequired) gameObject.SetActive(false);
    }
}
