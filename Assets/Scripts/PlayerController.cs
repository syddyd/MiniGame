using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


//Player controller by Sydney Dacks COMP 521
public class PlayerController : MonoBehaviour
{
    //components, gameObjects, etc to be set in editor 
    public GameObject player;
    public GameObject projectile;

    public GameObject winScreen;
    public GameObject loseScreen;

    //constants to be changed in editor for testing
    [SerializeField] public float moveSpeed = 20;
    [SerializeField] public float cameraSensitivity = 180;

    //private stuff to be used internally to the script
    private Vector2 moveDirection;
    private Rigidbody rb; 
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 relativeFor;
    private Vector3 relativeRight;
    private int fireCooldown = 60;
    private float jumpPower = 50;

    public void Start(){
        rb = player.GetComponent<Rigidbody>();
        winScreen.SetActive(false);
        loseScreen.SetActive(false);    
    }

    //Move, Jump, and Fire are each called directly by the new unity input system 
    //@pre InputAction.CallbackContext is passed to us by the input system and tells us about the button press

    //manages movement, applied in Update()
    public void Move(InputAction.CallbackContext context){
        moveDirection.x = context.ReadValue<Vector2>().x;
        moveDirection.y = context.ReadValue<Vector2>().y;
    }

    //sets us up to jump in Update()
    public void Jump(InputAction.CallbackContext context){
        print("jump attempt");
        if(context.performed && isGrounded()){
            rb.velocity += Vector3.up * jumpPower; 
        }

    }
    
    //fires a projectile if cooldown is ok, and only does when button is unpressed
    public void Fire(InputAction.CallbackContext context){
        if (fireCooldown == 110 && context.canceled){
            fireCooldown = 0;
            StartCoroutine(LaunchProjectile());
        }
    }

    //coroutine for handling the projectiles movement 
    private IEnumerator LaunchProjectile(){
        GameObject pro = Instantiate(projectile, transform.position, transform.rotation);
        Vector3 forwardAtFireTime = transform.forward;
        for (int i = 0; i < 100; i ++){
            pro.transform.position += forwardAtFireTime;
            yield return null;
        }
        Destroy(pro);
    }

    //handles win and loss states by checking if we collided with the lake or goal 
    void OnCollisionEnter(Collision other){
        if (other.gameObject.CompareTag("Lake"))
        {
            loseScreen.SetActive(true);
            EndGame();
        }

        if (other.gameObject.CompareTag("Goal")){
            winScreen.SetActive(true);
            EndGame();
        }
    }


    //gives us a few seconds to savor victory or defeat before ending the game 
    public IEnumerator EndGame(){
        yield return new WaitForSeconds(5);
        Application.Quit();
    }

    //helper for checking if its ok to jump 
    private bool isGrounded(){
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }

    void Update()
    {
        //this is our stuff to make the camera follow the mouse
        Vector3 localFor = transform.forward; //getting where the cameras facing 
        Vector3 localRight = transform.right;

        localFor.y = 0;
        localRight.y = 0;

        relativeFor = localFor * moveDirection.y; //making a new vector with where we want to go 
        relativeRight = localRight * moveDirection.x;

        //setting the actual velocity this frame taking into account the buttons pressed, the cameras existing angle, and the jump
        rb.velocity = ((relativeFor + relativeRight)*moveSpeed) + new Vector3(0, rb.velocity.y, 0);

        xRotation -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraSensitivity;
        yRotation += Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraSensitivity;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);             // to stop us from looking above/below 90

        transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);
        
        //for the projectile (easier here than a coroutine)
        if (fireCooldown < 110){
            fireCooldown++;
        }
    }
}
