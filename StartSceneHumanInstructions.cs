using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneHumanInstructions : MonoBehaviour
{
    [SerializeField] Text[] intructionText;
    [SerializeField] SpriteRenderer[] instructionImages;
    [TextArea][SerializeField] string[] instructions;
    [SerializeField] EnemyGenerator[] totems;
    [SerializeField] CapsuleCollider[] monsterColliders;
    [SerializeField] AudioClip[] instructionVO;

    private int stepsCompleted;
    private bool inCoroutine;
    private AudioSource voiceOver;

    // Start is called before the first frame update
    void Start()
    {
        stepsCompleted = 0;
        inCoroutine = false;
        voiceOver = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stepsCompleted == 0)
        {
            if (!voiceOver.isPlaying && (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5f ||
                OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f || Input.GetKeyDown(KeyCode.Space)))
            {
                for (int i = 0; i < intructionText.Length; i++) intructionText[i].text = instructions[0];
                voiceOver.clip = instructionVO[0];
                voiceOver.Play();
                stepsCompleted++;
                inCoroutine = true;
                StartCoroutine(DelayTextChange());
            }
        }
        else if (stepsCompleted == 1 && !inCoroutine)
        {
            bool totemsDestroyed = true;
            for (int i = 0; i < totems.Length; i++)
                if (totems[i].StillSpawning()) totemsDestroyed = false;
            if (!voiceOver.isPlaying && (totemsDestroyed || Input.GetKeyDown(KeyCode.Space)))
            {
                for (int i = 0; i < intructionText.Length; i++) intructionText[i].text = instructions[3];
                voiceOver.clip = instructionVO[3];
                voiceOver.Play();
                stepsCompleted++;
                inCoroutine = true;
                StartCoroutine(DelayTextChange());
            }
        }
        else if (!inCoroutine)
        {
            for (int i = 0; i < monsterColliders.Length; i++)
            {
                if (monsterColliders[i] == null)
                {
                    intructionText[i].gameObject.SetActive(false);
                    instructionImages[i].gameObject.SetActive(true);
                }
            }
        }
    }

    private IEnumerator DelayTextChange()
    {
        yield return new WaitWhile(() => voiceOver.isPlaying);
        if (stepsCompleted == 1)
        {
            for (int i = 0; i < intructionText.Length; i++) intructionText[i].text = instructions[1];
            voiceOver.clip = instructionVO[1];
            voiceOver.Play();
            yield return new WaitWhile(() => voiceOver.isPlaying);
            for (int i = 0; i < intructionText.Length; i++)
            {
                intructionText[i].text = instructions[2];
                totems[i].gameObject.SetActive(true);
            }
            voiceOver.clip = instructionVO[2];
            voiceOver.Play();
        }
        else
        {
            for (int i = 0; i < intructionText.Length; i++)
            {
                intructionText[i].text = instructions[4];
                monsterColliders[i].enabled = true;
            }
            voiceOver.clip = instructionVO[4];
            voiceOver.Play();
            yield return new WaitWhile(() => voiceOver.isPlaying);
        }
        inCoroutine = false;

    }
}
