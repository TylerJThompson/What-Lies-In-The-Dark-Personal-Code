using UnityEngine;

public class StartSceneFlashlight : MonoBehaviour
{
    [SerializeField] AudioSource voiceOver;

    private Light flashlight;
    private ParticleSystem lightVolume;
    private Flashlight flashlightScript;
    private bool triggerHeld;
    private bool enteredOnce;

    // Start is called before the first frame update
    void Start()
    {
        flashlight = GetComponentInChildren<Light>(true);
        lightVolume = GetComponentInChildren<ParticleSystem>(true);
        flashlightScript = GetComponent<Flashlight>();
        triggerHeld = false;
        enteredOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!voiceOver.isPlaying || enteredOnce) && ((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5f ||
            OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f) && !triggerHeld))
        {
            enteredOnce = true;
            triggerHeld = true;
            if (flashlightScript.allowHit)
            {
                flashlight.intensity = 0f;
                flashlightScript.allowHit = false;
                lightVolume.gameObject.SetActive(false);
            }
            else
            {
                flashlight.intensity = 1f;
                flashlightScript.allowHit = true;
                lightVolume.gameObject.SetActive(true);
            }
        }
        else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.5f &&
          OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.5f) triggerHeld = false;
    }
}
