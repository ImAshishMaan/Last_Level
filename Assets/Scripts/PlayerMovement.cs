using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 8f;


    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;

    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }


    private void Update() {
        Run();
        FlipSprite();
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
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }

        if (value.isPressed) {
            //do stuff
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void FlipSprite() {
        bool platerHasHorizantalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if(platerHasHorizantalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
    }
}
