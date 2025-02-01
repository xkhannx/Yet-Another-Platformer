using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

[RequireComponent(typeof(CollisionManager))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] public PlayerParametersSO playerData;
	// Status variables
	public bool grounded;
	public bool isClimbing = false;
	public bool controlsDisabled = false;
	public bool isFacingRight = true;

	// State variables
	[HideInInspector] public float xInput;
	[HideInInspector] public float yInput;
	bool interactPressed = false;

	// References to components
	CollisionManager collisions;
	Jumper jumper;
	ContextOverlaps context;
	Climber climber;

	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public Animator anim;

	bool initComplete = false;
	void OnEnable()
	{
		if (initComplete) return;
		initComplete = true;

		collisions = GetComponent<CollisionManager>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponentInChildren<Animator>();

		jumper = GetComponent<Jumper>();
		jumper.InitJumper();

		context = GetComponent<ContextOverlaps>();
		context.InitContext();

		climber = GetComponent<Climber>();
		climber.InitClimber();

		controlsDisabled = true;

		SetSpriteLayer(-99);
		//print("Gravity: " + gravity + "  Jump Velocity: " + jumpSpeed);
	}

	void Update()
	{
		if (controlsDisabled)
		{
			return;
		}

		context.CheckContext();

		CheckGround();
		CheckInput();

		climber.CheckClimb(context.canClimb);

		jumper.CheckJump();

		if (isClimbing)
		{
			climber.AnimClimb();
			return;
		}
		
		UpdateAnim();
		CheckFlip();

		if (context.canExit && interactPressed && grounded)
		{
			ExitLevel();
		}
	}

	private void FixedUpdate()
	{
		if (controlsDisabled) return;

		if (isClimbing)
		{
			climber.DoClimb();
		}
		else
		{
			ApplyRun();
		}
	}

	float velocityXSmoothing;
	float inputVel = 0;
	private void ApplyRun()
	{
		float targetVelocityX = xInput * playerData.runSpeed;
		
		inputVel = Mathf.SmoothDamp(inputVel, targetVelocityX, ref velocityXSmoothing, grounded ? playerData.accelerationTimeGrounded : playerData.accelerationTimeInAir);
		
		rb.velocity = new Vector2(inputVel, rb.velocity.y);

		if (Mathf.Abs(rb.velocity.x) > 8)
			Debug.DrawRay(Vector3.down * 0.3f + transform.position, new Vector3(rb.velocity.x, 0, 0), Color.red);

		Debug.DrawRay(Vector3.down * 0.5f + transform.position, new Vector3(rb.velocity.x, 0, 0), Color.red);
		Debug.DrawRay(Vector3.down * 0.5f + transform.position, new Vector3(0, rb.velocity.y, 0), Color.green);
	}

	RaycastHit2D hit;
	private void CheckGround()
	{
		hit = collisions.GroundRay();
		if (rb.velocity.y <= 0)
		{
			grounded = hit;
		} else
		{
			grounded = false;
		}
	}

	private void CheckInput()
	{
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");
		interactPressed = Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.E);
	}

	void CheckFlip()
    {
		if (isFacingRight && xInput < 0 || !isFacingRight && xInput > 0)
		{
			isFacingRight = !isFacingRight;
			transform.localScale = new Vector3(xInput, 1, 1);
		}
	}

    Vector2 tempPlayerVel;
    public void FreezePlayer(bool freeze, bool resetVel = true)
    {
        if (freeze)
        {
            tempPlayerVel = rb.velocity;

            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = resetVel ? Vector2.zero : tempPlayerVel;
            if (resetVel)
                inputVel = 0;
            rb.gravityScale = 1;
        }
    }

	public int wiggleDir;
    private void UpdateAnim()
    {
        anim.SetBool("Running", Mathf.Abs(rb.velocity.x) > 0.1f);
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("VelY", Mathf.Sign(rb.velocity.y));

		if (!grounded || Mathf.Abs(rb.velocity.x) > 0.1f || collisions.edge == 0)
        {
			anim.transform.localScale = new Vector3(1, 1, 1);
			anim.SetBool("Edge", false);
			wiggleDir = 0;
			return;
		}

		anim.SetBool("Edge", true);
		if ((isFacingRight && collisions.edge == 1) || (!isFacingRight && collisions.edge == -1))
		{
			anim.transform.localScale = new Vector3(-1, 1, 1);
			wiggleDir = isFacingRight ? -1 : 1;
		} else
        {
			wiggleDir = isFacingRight ? 1 : -1;
		}
	}
	
	public void StartClimb(bool start)
    {
		isClimbing = start;
		FreezePlayer(start);
		anim.enabled = !start;
	}

    public void SetSpriteLayer(int _sortingOrder)
    {
		GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder;
    }

	void ExitLevel()
    {
		controlsDisabled = true;
		rb.velocity = Vector2.zero;

		anim.SetBool("Running", true);

		GoldenDoor exitDoor = FindObjectOfType<GoldenDoor>();
		exitDoor.OpenGoldenDoor();

		transform.localScale = new Vector3(Mathf.Sign(exitDoor.transform.position.x - transform.position.x), 1, 1);

		transform.DOMove(exitDoor.transform.position + new Vector3(0, -0.5f, 0), 0.5f).OnComplete(ExitComplete);
	}

	void ExitComplete()
    {
		FindObjectOfType<GoldenDoor>().CloseGoldenDoor();
		SetSpriteLayer(-99);
		anim.SetBool("Running", false);

		FindObjectOfType<PlayerShadows>().DestroyShadows();
	}

	public void Die()
	{
		FindObjectOfType<PlayerShadows>().DestroyShadows();
		Destroy(gameObject);
	}
}
