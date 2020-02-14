using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public bool allowHit;
    [SerializeField] float hitDistance;
    [SerializeField] bool inStartMenu;
    [SerializeField] Text shrinesLeft;

    private int layerMask;
    private GameManager gameManager;

    private void Start()
    {
        //allowHit = true;
        if (inStartMenu) layerMask = ~(1 << 11);
        else layerMask = ~(1 << 9);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (allowHit)
        {
            Vector3 backward = -Vector3.forward;
            Vector3 left = Quaternion.AngleAxis(5f, Vector3.right) * backward;
            Vector3 right = Quaternion.AngleAxis(5f, -Vector3.right) * backward;
            Vector3 up = Quaternion.AngleAxis(5f, Vector3.up) * backward;
            Vector3 down = Quaternion.AngleAxis(5f, -Vector3.up) * backward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(backward), out hit, hitDistance, layerMask))
            {
                if (hit.collider.tag.Equals("Monster"))
                {
                    MonsterXboxController monster = hit.collider.gameObject.GetComponent<MonsterXboxController>();
                    if (monster != null)
                    {
                        monster.SetHitTrigger();
                        monster.ReduceHeath();
                    }
                }
                else if (hit.collider.tag.Equals("Totem"))
                {
                    EnemyGenerator totem = hit.collider.gameObject.GetComponent<EnemyGenerator>();
                    if (totem != null)
                    {
                        if (totem.StopSpawning())
                        {
                            gameManager.shrinesHit++;
                            StartCoroutine(ShowShrinesLeft());
                        }
                    }
                }
            }
            else if (Physics.Raycast(transform.position, transform.TransformDirection(left), out hit, hitDistance, layerMask))
            {
                if (hit.collider.tag.Equals("Monster"))
                {
                    MonsterXboxController monster = hit.collider.gameObject.GetComponent<MonsterXboxController>();
                    if (monster != null)
                    {
                        monster.SetHitTrigger();
                        monster.ReduceHeath();
                    }
                }
                else if (hit.collider.tag.Equals("Totem"))
                {
                    EnemyGenerator totem = hit.collider.gameObject.GetComponent<EnemyGenerator>();
                    if (totem != null)
                    {
                        if (totem.StopSpawning())
                        {
                            gameManager.shrinesHit++;
                            StartCoroutine(ShowShrinesLeft());
                        }
                    }
                }
            }
            else if (Physics.Raycast(transform.position, transform.TransformDirection(right), out hit, hitDistance, layerMask))
            {
                if (hit.collider.tag.Equals("Monster"))
                {
                    MonsterXboxController monster = hit.collider.gameObject.GetComponent<MonsterXboxController>();
                    if (monster != null)
                    {
                        monster.SetHitTrigger();
                        monster.ReduceHeath();
                    }
                }
                else if (hit.collider.tag.Equals("Totem"))
                {
                    EnemyGenerator totem = hit.collider.gameObject.GetComponent<EnemyGenerator>();
                    if (totem != null)
                    {
                        if (totem.StopSpawning())
                        {
                            gameManager.shrinesHit++;
                            StartCoroutine(ShowShrinesLeft());
                        }
                    }
                }
            }
            else if (Physics.Raycast(transform.position, transform.TransformDirection(up), out hit, hitDistance, layerMask))
            {
                if (hit.collider.tag.Equals("Monster"))
                {
                    MonsterXboxController monster = hit.collider.gameObject.GetComponent<MonsterXboxController>();
                    if (monster != null)
                    {
                        monster.SetHitTrigger();
                        monster.ReduceHeath();
                    }
                }
                else if (hit.collider.tag.Equals("Totem"))
                {
                    EnemyGenerator totem = hit.collider.gameObject.GetComponent<EnemyGenerator>();
                    if (totem != null)
                    {
                        if (totem.StopSpawning())
                        {
                            gameManager.shrinesHit++;
                            StartCoroutine(ShowShrinesLeft());
                        }
                    }
                }
            }
            else if (Physics.Raycast(transform.position, transform.TransformDirection(down), out hit, hitDistance, layerMask))
            {
                if (hit.collider.tag.Equals("Monster"))
                {
                    MonsterXboxController monster = hit.collider.gameObject.GetComponent<MonsterXboxController>();
                    if (monster != null)
                    {
                        monster.SetHitTrigger();
                        monster.ReduceHeath();
                    }
                }
                else if (hit.collider.tag.Equals("Totem"))
                {
                    EnemyGenerator totem = hit.collider.gameObject.GetComponent<EnemyGenerator>();
                    if (totem != null)
                    {
                        if (totem.StopSpawning())
                        {
                            gameManager.shrinesHit++;
                            StartCoroutine(ShowShrinesLeft());
                        }
                    }
                }
            }
        }
    }

    private IEnumerator ShowShrinesLeft()
    {
        int shrines = 4 - gameManager.shrinesHit;
        if (shrines == 1) shrinesLeft.text = shrines + " Shrine left";
        else shrinesLeft.text = shrines + " Shrines left";
        shrinesLeft.enabled = true;
        yield return new WaitForSeconds(2f);
        shrinesLeft.enabled = false;
    }
}
