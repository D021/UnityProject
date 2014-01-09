using UnityEngine;
using System.Collections;

public class CharacterControllerLogic : MonoBehaviour {

	#region Variable (private)
	
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float directionDampTime = .25f;
	[SerializeField]
	private ThirdPersonCamera gamecam;
	[SerializeField]
	private float directionSpeed = 3.0f;
	[SerializeField]
	private float rotationDegreePerSecond = 120f;


	private float speed = 0.0f;
	private float direction = 0f;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;
	private AnimatorStateInfo stateInfo;
	private CamStates camState = CamStates.Behind;
	private GameObject targetCam;
	private Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);
	private float radius = 5f;
	// Hashes 
	private int m_LocomotionId = 0;

	#endregion
	
	#region Properties (public)
	public enum CamStates
	{
		Behind,
		FirstPerson,
		Target,
		Free
	}
	#endregion
	
	#region Unity event functions

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
		if (animator.layerCount >=2) /// Does animator exists
		{
			animator.SetLayerWeight(1,1);
		}

		m_LocomotionId = Animator.StringToHash("Base Layer.Locomotion");

		//targetCam = GameObject.FindGameObjectWithTag("TargetCam");
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (animator) 
		{
			stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			// Get input
			switch (camState) 
			{
				case CamStates.Behind :
					//animator.SetBool("Strafing", false);
					
					break;
				case CamStates.Target :
					
					target();
					break;
			}
			horizontal = Input.GetAxis ("Horizontal");
			vertical = Input.GetAxis ("Vertical");

			StickToWorldSpace(this.transform, gamecam.transform, ref direction, ref speed);

			animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", direction, directionDampTime, Time.deltaTime);
		
			if (Input.GetAxis ("Target") > 0.01f) 
			{
				animator.SetBool("Strafing", true);
				camState = CamStates.Target;
			}
			else 
			{
				animator.SetBool("Strafing", false);
				camState = CamStates.Behind;
			}

		}
	}

	void FixedUpdate()
	{
		if(camState == CamStates.Behind) 
		{
			if (IsInLocomotion () && ((direction >= 0 && horizontal >= 0) || (direction < 0 && horizontal < 0)))
			{
					Vector3 rotationAmount = Vector3.Lerp (Vector3.zero, new Vector3 (0f, rotationDegreePerSecond * (horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs (horizontal));
					Quaternion deltaRotation = Quaternion.Euler (rotationAmount * Time.deltaTime);
					this.transform.rotation = (this.transform.rotation * deltaRotation);
			}
		}

	}

	#endregion
	
	#region Methods

	public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut) 
	{
		Vector3 rootDirection = root.forward;

		Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

		speedOut = stickDirection.sqrMagnitude;

		// Get cam rotation
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f;
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

		// Convert joystick
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.red);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);

		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

		angleRootToMove /= 180f;

		directionOut = angleRootToMove * directionSpeed;
	}

	public bool IsInLocomotion()
	{
		return stateInfo.nameHash == m_LocomotionId;
	}

	public void target() 
	{
			

	}

	#endregion Methods
}
