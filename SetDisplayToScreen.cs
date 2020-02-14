using UnityEngine;

public class SetDisplayToScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().targetDisplay = Display.displays.Length - 1;
        Display.displays[Display.displays.Length - 1].Activate();
    }
}
