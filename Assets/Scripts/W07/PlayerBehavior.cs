using UnityEngine;
using System.Collections;
//using UnityEditor.Rendering.LookDev;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D _myRigidBody;
    [SerializeField]private float _moveSpeed;

    private Vector2 inputVector;

    public ItemInfo _playerInven;

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
        if (context.started)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, 2f, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Item"))
                {
                    if(hit.collider.gameObject.GetComponent<ItemInfo>().GetItem()._isKey)
                    {
                        if (_playerInven != null)
                        {
                            Item temp = _playerInven.GetItem();

                            _playerInven.SetItem(hit.collider.gameObject.GetComponent<ItemInfo>().GetItem());
                            //Debug.Log($"Drop Item : {hit.collider.gameObject.GetComponent<ItemInfo>().GetItem()._itemName}");

                            hit.collider.gameObject.GetComponent<ItemInfo>().SetItem(temp);
                            //Debug.Log($"After Swap Item : {hit.collider.gameObject.GetComponent<ItemInfo>().GetItem()._itemName}");
                            return;
                        }

                        else if (_playerInven == null)
                        {
                            _playerInven = hit.collider.gameObject.GetComponent<ItemInfo>();
                            hit.collider.gameObject.SetActive(false);
                            return;
                        }
                    }
                    
                    else
                    {
                        hit.collider.gameObject.SetActive(false);
                    }
                }
            }
        }




        /*
        if(_canGetItem && context.started) 
        {
            if (_playerInven == null)
            {
                _playerInven = _nowItem;              
            }
            else if(_playerInven != null) 
            {
                ItemInfo temp = _playerInven;
                _playerInven = _nowItem;
                _nowItem = temp;
            }
        }*/
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
    /*
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
    */

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2f);

    }
}
