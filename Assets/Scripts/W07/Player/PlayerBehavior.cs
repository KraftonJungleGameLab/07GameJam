using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

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
    public float _fdt;
    public float _skillMaxCT;

    public Image _textBox;
    public TextMeshProUGUI _infoText;

    public ItemInfo _playerInven;

    void Start()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _playerLight = GetComponentInChildren<PlayerLight>();
        _volume = GetComponent<Volume>();
    }

    private void Update()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                GameManager.Instance.PlusDieFdt();
                return;
            }                
        }
        GameManager.Instance.ResetDieFdt();



        if (_isSkillCT && !_isSkillUse)
        {
            _fdt += Time.deltaTime;
            UIManager.Instance.UpdateCoolTime(_fdt);
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

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.started) 
        {
            if(Time.timeScale > 0) 
            {
                UIManager.Instance.PauseOn();
            }
            else
            {
                UIManager.Instance.PauseOff();
            }
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if(context.started) 
        {
            GameManager.Instance.ReloadScene();
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
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, 0.5f, Vector2.zero);
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
                            PrintInfo("열쇠 교체.");
                            return;
                        }

                        else if (_playerInven == null)
                        {
                            _playerInven = hit.collider.gameObject.GetComponent<ItemInfo>();
                            _keySprite.gameObject.SetActive(true);
                            _keySprite.color = _playerInven.GetItem()._itemColor;
                            hit.collider.gameObject.SetActive(false);
                            PrintInfo("열쇠를 집었다.");
                            return;
                        }
                    }
                    
                    else
                    {
                        _playerLight.ResetLight();
                        PrintInfo("횃불을 집었다.");
                        return;
                        //hit.collider.gameObject.SetActive(false);
                    }
                }

                if(hit.collider != null && hit.collider.gameObject.CompareTag("OpenZone"))
                {
                    Item needItem = hit.collider.gameObject.GetComponent<CheckItem>()._needItemInfo;
                    if(_playerInven != null && needItem == _playerInven.GetItem()) 
                    {
                        hit.collider.gameObject.SetActive(false);
                        _keySprite.gameObject.SetActive(false);
                        _playerInven = null;
                        PrintInfo("자물쇠가 열렸다");
                        return;
                    }
                    else
                    {
                        PrintInfo("열리지 않는다.");
                        return;
                    }
                }

                if (hit.collider != null && hit.collider.gameObject.CompareTag("Switch"))
                {
                    hit.collider.gameObject.GetComponent<Switch>().OpenDoor();
                    PrintInfo("문이 열렸다");
                    return;
                }

                if (hit.collider != null && hit.collider.gameObject.CompareTag("Switch"))
                {
                    hit.collider.gameObject.GetComponent<Switch>().OpenDoor();
                    PrintInfo("문이 열렸다");
                    return;
                }
            }
        }

    }

    private void PrintInfo(string text)
    {
        //StopCoroutine(InfoFade());
        _infoText.text = text;
        StartCoroutine(InfoFade());
    }

    IEnumerator InfoFade()
    {
        Color tempColor = _textBox.color;
        tempColor.a += Time.deltaTime * 10f;
        _textBox.color = tempColor;
        yield return new WaitForSeconds(0.1f);
        tempColor.a = 1f;
        _textBox.color = tempColor;
        _infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        tempColor.a = 0f;
        _textBox.color = tempColor;
        _infoText.gameObject.SetActive(false);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
