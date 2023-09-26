using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Setting")]
    [SerializeField] Vector3 m_offset;
    [SerializeField] float m_chaseSpeed;


    Transform m_player;

    Vector3 m_currentPos;


    private void Start()
    {
        m_player = GameObject.FindWithTag("Player").transform;

        m_currentPos = transform.position;
    }

    private void FixedUpdate()
    {
        m_currentPos = Vector3.Lerp(m_currentPos, m_player.position, Time.deltaTime * m_chaseSpeed);
        transform.position = m_currentPos + m_offset;
    }
}