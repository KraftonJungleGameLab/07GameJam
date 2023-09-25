using UnityEngine;
using System.Collections;
//using UnityEditor.Rendering.LookDev;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D _myRigidBody;
    private float _moveSpeed = 10.0f;

    private Vector2 inputVector;

    public ItemInfo _nowItem;

    private bool _canGetItem;

    void Start()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        _myRigidBody.velocity = inputVector * _moveSpeed;

        //UpdateAim();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnGetItem(InputAction.CallbackContext context) 
    {
        if(_canGetItem && context.started) 
        {
            Inventory.Instance.CheckItem(_nowItem);
        }
    }

    /*
    void UpdateAim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = transform.position.y;
        //mousePointer.transform.position = mousePos;
        float deltaY = mousePos.z - transform.position.z;
        float deltaX = mousePos.x - transform.position.x;
        float angleInDegrees = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
        transform.eulerAngles = new Vector3(0, -angleInDegrees, 0);
    }
    */
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Item"))
        {
            _canGetItem = true;
            _nowItem = collision.gameObject.GetComponent<ItemInfo>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            _canGetItem = false;
            _nowItem = null;
        }
    }
}
