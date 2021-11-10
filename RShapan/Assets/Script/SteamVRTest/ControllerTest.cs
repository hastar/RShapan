
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ControllerTest : MonoBehaviour {
	SteamVR_TrackedObject trackedObject;
	SteamVR_Controller.Device device;
    GameObject interactBox = null;
	// Use this for initialization
	void Start () {
        //获取被追踪的对象
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        //获取对应设备
        device = SteamVR_Controller.Input((int)trackedObject.index);
		
	}
	void Update () {
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            SetInteractBoxColor(Color.red);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            SetInteractBoxColor(Color.yellow);
        }
	}

    void SetInteractBoxColor(Color r){
        if(interactBox != null)
        {
            MeshRenderer mr = interactBox.GetComponent<MeshRenderer>();
            mr.material.color = r;
        }
    }
	void OnTriggerEnter(Collider other){
        Debug.Log(other.gameObject.name);
        interactBox = other.gameObject;
        SetInteractBoxColor(Color.yellow);
        device.TriggerHapticPulse(5000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
        device.TriggerHapticPulse(5000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        device.TriggerHapticPulse(5000, Valve.VR.EVRButtonId.k_EButton_Grip);
        device.TriggerHapticPulse(5000, Valve.VR.EVRButtonId.k_EButton_ApplicationMenu);
        device.TriggerHapticPulse(5000, Valve.VR.EVRButtonId.k_EButton_System);

    }
	void OnTriggerExit(Collider other){
        SetInteractBoxColor(Color.white);
        interactBox = null;
    }








    void PickOrNoBox(bool isPick)
    {
        if (interactBox != null)
        {
            interactBox.transform.parent = (isPick) ? transform : null;
            interactBox.GetComponent<Rigidbody>().useGravity = !isPick;
        }
    }
}
