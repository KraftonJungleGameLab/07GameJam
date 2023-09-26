using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _keySprite;
    [SerializeField] private float _moveSpeed;

    private Rigidbody2D _myRigidBody;
    private PlayerLight _playerLight;

    private Vector2 inputVector;

    private Volume _volume;
    private bool _isSkillCT = false;
    private bool _isSkillUse = false;
    private float _fdt;
    [SerializeField] private float _skillMaxCT;


    public ItemInfo _playerInven;

    void Start()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _playerLight = GetComponentInChildren<PlayerLight>();
        _volume = GetComponent<Volume>();
    }

    private void Update()
    {
        if(_isSkillCT && !_isSkillUse)
        {
            _fdt += Time.deltaTime;

            if(_fdt > _skillMaxCT) 
            {
                _isSkillCT = false;
                _fdt = 0;
            }
        }
        
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

    public void OnColorDiff(InputAction.CallbackContext context)
    {
        if(context.started && !_isSkillCT && !_isSkillUse) 
        {
            StartCoroutine(ColorDiffOn());
        }
    }

    IEnumerator ColorDiffOn()
    {
        _volume.enabled = false;
        _isSkillUse = true;
        yield return new WaitForSeconds(2f);
        _isSkillUse = false;
        _volume.enabled = true;
        _isSkillCT = true;
    }

    

    public void OnGetItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("F");
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
                            _keySprite.color = _playerInven.GetItem()._itemColor;
                            hit.collider.gameObject.GetComponent<ItemInfo>().SetItem(temp);
                            //Debug.Log($"After Swap Item : {hit.collider.gameObject.GetComponent<ItemInfo>().GetItem()._itemName}");
                            return;
                        }

                        else if (_playerInven == null)
                        {
                            _playerInven = hit.collider.gameObject.GetComponent<ItemInfo>();
                            _keySprite.gameObject.SetActive(true);
                            _keySprite.color = _playerInven.GetItem()._itemColor;
                            hit.collider.gameObject.SetActive(false);
                            return;
                        }
                    }
                    
                    else
                    {
                        _playerLight.ResetLight();
                        return;
                        //hit.collider.gameObject.SetActive(false);
                    }
                }

                if(hit.collider != null && hit.collider.gameObject.CompareTag("OpenZone"))
                {
                    Item needItem = hit.collider.gameObject.GetComponent<CheckItem>()._needItemInfo;
                    if(needItem  == _playerInven.GetItem()) 
                    {
                        hit.collider.gameObject.SetActive(false);
                        _keySprite.gameObject.SetActive(false);
                        _playerInven = null;
                        return;
                    }
                }
            }
        }

    }


    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2f);

    }
}
