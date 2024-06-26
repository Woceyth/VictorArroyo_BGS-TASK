using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private int i_Money;
    public InputActionReference iar_Movement;
    public InputActionReference iar_Inventory;
    public GameObject go_backgroundImage;
    public GameObject go_closeButton;
    public GameObject go_inventoryFrame;
    public GameObject go_bagFrame;
    public GameObject go_coinsLabel;
    public Animator a_animator;
    private readonly int i_RunBool = Animator.StringToHash("Run");

    private Vector2 v2_movementDirection;
    private float f_directionRecoded = 1f;
    private bool b_active = false;
    private readonly float f_speed = 13f;
    private new Rigidbody2D rigidbody2D;

    private void OnEnable()
    {
        Instance = this;
        iar_Inventory.action.started += Inventory;
        i_Money = 60;
    }

    /// <summary>
    /// Enables the player movement
    /// </summary>
    public void EnablePlayerMovement()
    {
        iar_Movement.action.Enable();
    }

    /// <summary>
    /// Enables the player interaction
    /// </summary>
    public void EnablePlayerInteraction()
    {
        iar_Inventory.action.started += Inventory;
    }

    /// <summary>
    /// Set active boolan false, this is used for the input handling
    /// </summary>
    public void SetActiveFalse()
    {
        b_active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Inventory(InputAction.CallbackContext obj)
    {
        //Debug.Log("Inventory");
        b_active = !b_active;
        go_inventoryFrame.SetActive(b_active);
        go_backgroundImage.SetActive(b_active);
        go_closeButton.SetActive(b_active);
        go_coinsLabel.SetActive(b_active);
        go_bagFrame.SetActive(b_active);

        //Debug.Log("b_active: " + b_active);
        if (b_active == true)
        {
            DisablePlayerMovement();
        }
        else
        {
            EnablePlayerMovement();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        v2_movementDirection = iar_Movement.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(v2_movementDirection.x * f_speed, v2_movementDirection.y * f_speed);
        if (v2_movementDirection.x != 0 )
        { 
            f_directionRecoded = Mathf.Round(v2_movementDirection.x);
            a_animator.SetBool(i_RunBool, true);
        }
        else
        {
            a_animator.SetBool(i_RunBool, false);
        }
        transform.localScale = new Vector3( f_directionRecoded, 1f, 1f );
    }

    /// <summary>
    /// Get the current quantity you have at the moment
    /// </summary>
    /// <returns></returns>
    public int GetMoneyQuantity()
    {
        return i_Money;
    }

    /// <summary>
    /// Set the new quantity you have
    /// </summary>
    /// <param name="newQuantity"></param>
    public void SetMoneyQuantity( int newQuantity)
    {
        i_Money = newQuantity;
        Debug.Log("i_Money: " + i_Money);
    }

    /// <summary>
    /// Disables the character movement
    /// </summary>
    public void DisablePlayerMovement()
    {
        iar_Movement.action.Disable();
    }

    /// <summary>
    /// Enables the player interaction
    /// </summary>
    public void DisablePlayerInteraction()
    {
        iar_Inventory.action.started -= Inventory;
    }

    private void OnDestroy()
    {
        Instance = null;
        iar_Inventory.action.started -= Inventory;
    }
}
