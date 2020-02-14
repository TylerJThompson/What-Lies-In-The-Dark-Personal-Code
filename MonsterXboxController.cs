using System.Collections;
using UnityEngine;

public class MonsterXboxController : MonoBehaviour
{
    private enum PlayerNumber { Player1, Player2, Player3, Player4};

    [SerializeField] PlayerNumber playerNumber;
    [SerializeField] float health;
    [SerializeField] float speed, rotationSpeed, fadeSpeed;
    [SerializeField] Material opaque, transparent, dissolve;
    [SerializeField] float maxLightIntensity;
    [SerializeField] AudioClip slashSound;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] GameObject minimapIcon;
    [SerializeField] GameObject healthBar;

    private string horizontal, vertical, rotation, attack, interact;
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;
    private Light areaLight;
    private CapsuleCollider capsule;
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private MonsterClaws claws;
    private bool totemStillSpawning;
    private bool totemOff;
    private ParticleSystem ps;
    private SetDeathMarker deathIcon;
    private float lastHealth;
    private bool allowTransitionToIdle;

    private bool psEnabled;
    private DissolveSurface dissolveSurface;

    private bool inCoroutine = false;
    public bool inStartMenu;

    private void Start()
    {
        horizontal = "LeftStickHorizontal" + playerNumber.ToString().Substring(playerNumber.ToString().Length - 1);
        vertical = "LeftStickVertical" + playerNumber.ToString().Substring(playerNumber.ToString().Length - 1);
        rotation = "RightStickHorizontal" + playerNumber.ToString().Substring(playerNumber.ToString().Length - 1);
        attack = "RightTrigger" + playerNumber.ToString().Substring(playerNumber.ToString().Length - 1);
        interact = "AButton" + playerNumber.ToString().Substring(playerNumber.ToString().Length - 1);
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        areaLight = GetComponentInChildren<Light>();
        capsule = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        claws = GetComponentInChildren<MonsterClaws>();
        totemStillSpawning = true;
        totemOff = false;
        ps = GetComponentInChildren<ParticleSystem>();
        DisablePS();
        dissolveSurface = GetComponent<DissolveSurface>();
        if(dissolveSurface == null)
        {
            Debug.Log("dissolve Surface is null");
        }
        GameObject deathIconObject = GameObject.Find("MonsterDeathIcon" + playerNumber.ToString().Substring(playerNumber.ToString().Length - 1));
        if (deathIconObject != null)
        {
            deathIcon = deathIconObject.GetComponent<SetDeathMarker>();
            deathIcon.SetMonsterUI(minimapIcon);
        }
        else deathIcon = null;
        lastHealth = health;
        allowTransitionToIdle = false;
    }

    public void AllowIdle()
    {
        allowTransitionToIdle = true;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad >= 3 || inStartMenu)
        {
            if (health == lastHealth && allowTransitionToIdle)
            {
                animator.SetTrigger("Idle");
                allowTransitionToIdle = false;
            }
            else lastHealth = health;

            if (cameraFollow != null) totemStillSpawning = cameraFollow.generator.StillSpawning();
            else totemStillSpawning = true;
            if (!(totemStillSpawning || totemOff))
            {
                totemOff = true;
                Color newColor = new Color(1f, 1f, 1f, 1f);
                meshRenderer.material.color = newColor;
                if (meshRenderer.material != opaque) meshRenderer.material = opaque;
                meshRenderer.material.DisableKeyword("_EMISSION");
                meshRenderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                meshRenderer.material.SetColor("_EmissionColor", Color.black);
                meshRenderer.enabled = true;
                capsule.enabled = true;
                areaLight.intensity = 1f;
                areaLight.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                speed = 5f;
                healthBar.SetActive(true);
            }

            if (rigidBody != null) rigidBody.velocity = Vector3.zero;

            float horizontalTranslation = Input.GetAxis(horizontal) * speed * Time.deltaTime;
            float verticalTranslation = Input.GetAxis(vertical) * speed * Time.deltaTime;
            Vector3 translation = new Vector3(horizontalTranslation, 0f, verticalTranslation);
            transform.Translate(translation);

            float horiztonalRotation = Input.GetAxis(rotation) * rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, horiztonalRotation, 0f);

            if (!inStartMenu)
            {
                if (horizontalTranslation == 0f && verticalTranslation == 0f) animator.SetBool("Walk", false);
                else animator.SetBool("Walk", true);
            }

            if (/*totemStillSpawning &&*/ !inStartMenu)
            {
                if (horizontalTranslation == 0f && verticalTranslation == 0f &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName("mosterAttack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("monsterHurt"))
                {
                    if (!meshRenderer.material.name.Contains("Transparent")) meshRenderer.material = transparent;
                    Color curColor = meshRenderer.material.color;
                    Color newColor = new Color(1f, 1f, 1f, Mathf.Max(0, curColor.a - (fadeSpeed * Time.deltaTime)));
                    meshRenderer.material.color = newColor;
                    if (meshRenderer.material.color.a <= 0f)
                    {
                        meshRenderer.enabled = false;
                        capsule.enabled = false;
                        healthBar.SetActive(false);
                    }
                    areaLight.intensity = meshRenderer.material.color.a * maxLightIntensity;
                }
                else
                {
                    Color newColor = new Color(1f, 1f, 1f, 1f);
                    meshRenderer.material.color = newColor;
                    if (meshRenderer.material != opaque) meshRenderer.material = opaque;
                    meshRenderer.enabled = true;
                    capsule.enabled = true;
                    areaLight.intensity = maxLightIntensity;
                    healthBar.SetActive(true);
                }
            }

            if (Input.GetAxis(attack) > 0.1f && !inStartMenu)
            {
                //if (totemStillSpawning)
                //{
                Color newColor = new Color(1f, 1f, 1f, 1f);
                meshRenderer.material.color = newColor;
                if (meshRenderer.material != opaque) meshRenderer.material = opaque;
                meshRenderer.enabled = true;
                capsule.enabled = true;
                areaLight.intensity = maxLightIntensity;
                healthBar.SetActive(true);
                //}
                // particle system 
                EnablePS();
                animator.SetTrigger("Attack");
                Invoke("DisablePS", 1);

            }
            if (health <= 0 && inCoroutine == false)
            {
                StartCoroutine(DestroySelf());
            }
            //DestroySelf();
        }
    }

    private void EnablePS()
    {
        psEnabled = true;
        if (ps != null)
        {
            var emission = ps.emission;
            emission.enabled = psEnabled;
           // ps.gameObject.SetActive(psEnabled);
        }
    }
    
    private void DisablePS()
    {        
        psEnabled = false;
        if (ps != null)
        {
            var emission = ps.emission;
            emission.enabled = psEnabled;
            //ps.gameObject.SetActive(psEnabled);
        }
    }

    public void PlaySlashSound()
    {
        audioSource.Stop();
        audioSource.clip = slashSound;
        audioSource.Play();
    }

    public void EnableAttack()
    {
        claws.EnableAttack();
    }

    public void DisableAttack()
    {
        claws.DisableAttack();
    }

    public void PlayHurtSound()
    {
        audioSource.Stop();
        audioSource.clip = hurtSounds[Random.Range(0, hurtSounds.Length)];
        audioSource.Play();
    }

    public void SetHitTrigger()
    {
        if (/*totemStillSpawning &&*/ !inStartMenu)
        {
            Color newColor = new Color(1f, 1f, 1f, 1f);
            meshRenderer.material.color = newColor;
            if (meshRenderer.material != opaque) meshRenderer.material = opaque;
            meshRenderer.enabled = true;
            areaLight.intensity = maxLightIntensity;
            healthBar.SetActive(true);
        }
        animator.SetTrigger("Hit");
    }

    public void ReduceHeath()
    {
        health-=Time.deltaTime;
    }

    public float GetHealth()
    {
        return health;
    }

    public string GetInteractButton()
    {
        return interact;
    }

    /*private void DestroySelf()
    {
        if (health <= 0)
        {
            if (deathIcon != null) deathIcon.MoveToMonsterUI();
            Destroy(gameObject);
        }
    }*/

    // Shelley's change: 
    IEnumerator DestroySelf()
    {
        if (health <= 0)
        {
            inCoroutine = true;
            if (deathIcon != null) deathIcon.MoveToMonsterUI();
            meshRenderer.material = dissolve;
            dissolveSurface.RenderEffect();
            yield return new WaitUntil(() => dissolveSurface.isEnd == true);
            Destroy(gameObject);
        }
        //Debug.Log("game object is "+gameObject);
    }
}
