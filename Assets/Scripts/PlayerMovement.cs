using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float climbSpeed = 5f;
    private float gravityAtStartScale;


    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityAtStartScale = myRigidbody.gravityScale;
    }


    private void Update() {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool platerHasHorizantalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
       
        myAnimator.SetBool("isRunning", platerHasHorizantalSpeed);

    }

    private void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    private void OnJump(InputValue value) {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }

        if (value.isPressed) {
            //do stuff
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void FlipSprite() {
        bool playerHasHorizantalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizantalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
    }

    private void ClimbLadder() {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            myRigidbody.gravityScale = gravityAtStartScale;
            myAnimator.SetBool("isClimbing", false);
            return; 
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myRigidbody.gravityScale = 0f;
        
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
}
