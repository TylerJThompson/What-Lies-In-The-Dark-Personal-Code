using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] Light pointLight;
    [SerializeField] MonsterConfirmStart[] monsters;
    [SerializeField] float transitionLength;
    [SerializeField] Canvas[] startCanvas, monsterCanvas;

    private bool coroutineStarted = false;
    private bool monstersPrepared = false;

    // Update is called once per frame
    void Update()
    {
        bool humanReady = true;
        bool allMonstersReady = true;
        for (int i = 0; i < monsters.Length; i++)
        {
            if (!monsters[i].MonsterWasDestroyed()) humanReady = false;
            if (!monsters[i].IsReadyToStart()) allMonstersReady = false;
        }

        if (allMonstersReady && !monstersPrepared)
        {
            monstersPrepared = true;
            for (int i = 0; i < 4; i++)
            {
                startCanvas[i].gameObject.SetActive(false);
                monsterCanvas[i].gameObject.SetActive(true);
                for (int j = 0; j < monsters.Length; j++) monsters[i].SetSpritesToBlack();
            }
        }

        if (!coroutineStarted && humanReady && allMonstersReady)
        {
            coroutineStarted = true;
            StartCoroutine(ToMainScene());
        }
    }

    private IEnumerator ToMainScene()
    {
        for (int j = 500; j >= 0; j--)
        {
            pointLight.range = j;
            yield return new WaitForSeconds(transitionLength / 501f);
        }
        SceneManager.LoadScene(1);
    }
}
