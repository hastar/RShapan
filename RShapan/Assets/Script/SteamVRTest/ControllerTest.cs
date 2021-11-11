
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ControllerTest : MonoBehaviour {
	SteamVR_TrackedObject trackedObject;
	SteamVR_Controller.Device device;
    GameObject interactBox = null;
    Transform moveCube;
	// Use this for initialization
	void Start () {
        //获取被追踪的对象
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        //获取对应设备
        device = SteamVR_Controller.Input((int)trackedObject.index);
        moveCube = GameObject.Find("MoveCube").transform;
		
	}
	void Update () {

	}

    public void MoveCube()
    {
        //判断是否按着触摸盘
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            //获取触摸盘的坐标(x, y) [-1, 1]
            Vector2 dir = device.GetAxis();
            //边界处理
            if (moveCube != null)
            {
                //移动方向
                Vector3 vDir = new Vector3(dir.x, 0, dir.y);
                //移动物体
                moveCube.position += vDir * Time.deltaTime * 3;
            }
        }
    }

    public void PickObject()
    {
        //判断按键状态  按下按键
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //判断是否碰撞
            if (interactBox != null)
            {
                //通过设置父物体， 实现物体跟随
                interactBox.transform.parent = transform;
                //去掉重力， 让物体不掉落
                interactBox.GetComponent<Rigidbody>().useGravity = false;
            }
        }

        //松开按键， 状态移除
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (interactBox != null)
            {
                interactBox.transform.parent = transform;
                interactBox.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    public void ChangeColor()
    {
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
