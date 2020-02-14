using System.Collections;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] CamVigenette vigenette;
    [SerializeField] CamFade fade;
    //[SerializeField] OVRPlayerController controller;
    [SerializeField] Light flashlight;
    [SerializeField] ParticleSystem lightVolume;
    [SerializeField] Flashlight flashlightScript;
    [SerializeField] MeshRenderer flashlightMesh;
    [SerializeField] GameObject minimapIcon;

    private Animator animator;
    private AudioSource hurtSound;
    public EnemyGenerator generator;
    private int trueHealth;
    //private bool flashlightBlinking;
    private bool triggerHeld;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hurtSound = GetComponent<AudioSource>();
        trueHealth = health;
        GameObject gmObject = GameObject.FindGameObjectWithTag("GameManager");
        if (gmObject != null) gameManager = gmObject.GetComponent<GameManager>();
        if (gameManager != null)
        {
            if (gameManager.flashlightOn)
            {
                flashlight.intensity = 1f;
                lightVolume.gameObject.SetActive(true);
                flashlightScript.allowHit = true;
                minimapIcon.SetActive(true);
            }
            else
            {
                flashlight.intensity = 0f;
                lightVolume.gameObject.SetActive(false);
                flashlightScript.allowHit = false;
                minimapIcon.SetActive(false);
            }
        }
        //flashlightBlinking = false;
        triggerHeld = false;
        fade.FadeInCam();
        StartCoroutine(StartImmune());
    }

    private IEnumerator StartImmune()
    {
        vigenette.Reset();
        GetComponent<CapsuleCollider>().enabled = false;
        trueHealth = health;
        SkinnedMeshRenderer model = GetComponentInChildren<SkinnedMeshRenderer>();
        //float desiredIntensity = flashlight.intensity;
        for (int i = 0; i < 8; i++)
        {
            if (model.enabled)
            {
                model.enabled = false;
                //flashlight.intensity = 0f;
                //lightVolume.gameObject.SetActive(false);
                //flashlightMesh.enabled = false;
            }
            else
            {
                model.enabled = true;
                //flashlight.intensity = desiredIntensity;
                //lightVolume.gameObject.SetActive(true);
                //flashlightMesh.enabled = true;
            }
            yield return new WaitForSeconds(0.25f);
        }
        model.enabled = true;
        //flashlight.intensity = desiredIntensity;
        //lightVolume.gameObject.SetActive(true);
        //flashlightMesh.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private IEnumerator QuickImmune()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        SkinnedMeshRenderer model = GetComponentInChildren<SkinnedMeshRenderer>();
        float desiredIntensity = flashlight.intensity;
        for (int i = 0; i < 2; i++)
        {
            if (model.enabled)
            {
                model.enabled = false;
                flashlight.intensity = 0f;
                lightVolume.gameObject.SetActive(false);
                flashlightMesh.enabled = false;
            }
            else
            {
                model.enabled = true;
                flashlight.intensity = desiredIntensity;
                lightVolume.gameObject.SetActive(true);
                flashlightMesh.enabled = true;
            }
            yield return new WaitForSeconds(0.45f);
        }
        model.enabled = true;
        flashlight.intensity = desiredIntensity;
        lightVolume.gameObject.SetActive(true);
        flashlightMesh.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void Update()
    {
        //if (controller.IsPlayerMoving()) animator.SetBool("isRunning", true);
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > 0.1f ||
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y > 0.1f ||
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -0.1f ||
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y < -0.1f ||
            OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x > 0.1f ||
            OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y > 0.1f ||
            OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x < -0.1f ||
            OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y < -0.1f) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
        if ((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5f ||
            OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f) && !triggerHeld)
        {
            triggerHeld = true;
            if (flashlightScript.allowHit)
            {
                flashlight.intensity = 0f;
                lightVolume.gameObject.SetActive(false);
                flashlightScript.allowHit = false;
                minimapIcon.SetActive(false);
            }
            //else if (!flashlightBlinking) StartCoroutine(BlinkFlashlightOn());
            else
            {
                flashlight.intensity = 1f;
                lightVolume.gameObject.SetActive(true);
                flashlightScript.allowHit = true;
                minimapIcon.SetActive(true);
            }
        }
        else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.5f &&
          OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.5f) triggerHeld = false;
    }

    /*private IEnumerator BlinkFlashlightOn()
    {
        flashlightBlinking = true;
        for (int i = 0; i < 4; i++)
        {
            if (flashlight.intensity == 0f)
            {
                flashlight.intensity = 1f;
                lightVolume.gameObject.SetActive(true);
                minimapIcon.SetActive(true);
            }
            else
            {
                flashlight.intensity = 0f;
                lightVolume.gameObject.SetActive(false);
                minimapIcon.SetActive(false);
            }
            yield return new WaitForSeconds(0.25f);
        }
        flashlight.intensity = 1f;
        lightVolume.gameObject.SetActive(true);
        flashlightScript.allowHit = true;
        minimapIcon.SetActive(true);
        flashlightBlinking = false;
    }*/

    public void HitHuman()
    {
        trueHealth--;
        if (!hurtSound.isPlaying) hurtSound.Play();
        if (trueHealth > 0)
        {
            StartCoroutine(QuickImmune());
            //animator.SetTrigger("Hit");           Add in once hurt animation provided
            vigenette.RenderEffect();
        }
        else
        {
            animator.SetTrigger("Dead");
            fade.FadeOutCam();
        }
    }

    public void SetGenerator(EnemyGenerator humanGen)
    {
        generator = humanGen;
    }

    public int GetHealth()
    {
        return trueHealth;
    }

    public void DestroySelf()
    {
        if (gameManager != null) gameManager.flashlightOn = flashlightScript.allowHit;
        if (generator != null) generator.SetCurEnemyNull();
        Destroy(transform.parent.gameObject);
    }
}
