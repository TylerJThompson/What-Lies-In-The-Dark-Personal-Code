using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private enum CameraNumber { Camera1, Camera2, Camera3, Camera4 };

    [SerializeField] CameraNumber cameraNumber;
    [SerializeField] Transform positionTarget, lookTarget;
    [SerializeField] float smoothSpeed = 0.125f;

    public EnemyGenerator generator;

    private bool alreadyCalledRespawn;

    private void Start()
    {
        Camera thisCamera = GetComponent<Camera>();

        if (cameraNumber == CameraNumber.Camera1) thisCamera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
        else if (cameraNumber == CameraNumber.Camera2) thisCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        else if (cameraNumber == CameraNumber.Camera3) thisCamera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        else if (cameraNumber == CameraNumber.Camera4) thisCamera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);

        thisCamera.targetDisplay = Display.displays.Length - 1;
        Display.displays[Display.displays.Length - 1].Activate();

        alreadyCalledRespawn = false;
    }

    private void LateUpdate()
    {
        if (positionTarget != null)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, positionTarget.position, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(lookTarget);
        }
        else if (!alreadyCalledRespawn)
        {
            alreadyCalledRespawn = true;
            StartCoroutine(DestroySelfAndRespawn());
        }
    }

    private IEnumerator DestroySelfAndRespawn()
    {
        yield return new WaitForSeconds(3f);
        if (generator != null) generator.SetCurEnemyNull();
        Destroy(transform.parent.gameObject);
    }

    public void SetGenerator(EnemyGenerator enemyGen)
    {
        generator = enemyGen;
    }
}
