using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WhiteboardPen : VRTK_InteractableObject
{
	public Whiteboard whiteboard;
	private RaycastHit touch;
	private bool lastTouch;
	private Quaternion lastAngle;
	private VRTK_ControllerReference controller;
    // Start is called before the first frame update
    void Start()
    {
        this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();
    }

    // Update is called once per frame
    void Update()
    {
        float tipHeight = transform.Find("Tip").transform.localScale.y;
	Vector3 tip =transform.Find("Tip").transform.position;
	if(lastTouch)
	{
		tipHeight *= 1.1f;
	}
	if (Physics.Raycast(tip,transform.up,out touch,tipHeight))
	{
		if(!(touch.collider.tag=="Whiteboard"))
			return;
		this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();
		VRTK_ControllerHaptics.TriggerHapticPulse(controller, 0.1f);
		whiteboard.SetColor (Color.blue);
		whiteboard.SetTouchPosition (touch.textureCoord.x, touch.textureCoord.y);
		whiteboard.ToggleTouch (true);

		Debug.Log("touching!");
		if(!lastTouch)
		{
			lastTouch=true;
			lastAngle=transform.rotation;
		}
	}
	else
	{
		lastTouch=false;
	}
	if (lastTouch)
	{
		transform.rotation=lastAngle;
	}
    }
	public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        controller = VRTK_ControllerReference.GetControllerReference(currentGrabbingObject.gameObject);
    }

}
