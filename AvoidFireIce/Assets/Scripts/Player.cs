using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Element CurrentElemental = Element.Fire;
    public List<GameObject> FireOrb;
    public List<GameObject> IceOrb;
    public GameObject DeathPrefab;
    public GameObject Background;
    public AudioClip ChangeSound;
	private bl_Joystick Joystick;

	public float speed = 1f;

	private Vector2 direction = Vector2.zero;


    private Rigidbody2D rb;
    private AudioSource audioSource;
    public bool isWall = false;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
		Joystick = GameObject.FindGameObjectWithTag("Joy").GetComponent<bl_Joystick>();

	}
    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }

    private void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
		v += Mathf.Clamp(Joystick.Vertical, -1f,1f);
		h += Mathf.Clamp(Joystick.Horizontal, -1f, 1f);

		direction = new Vector3(h, v);
        if (!isWall && direction.magnitude > 1)
		{
			direction.Normalize();
		}

		if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0) && Input.mousePosition.x >= Screen.width / 2f && !IsPointerOverUIObject()))
        {
            ChangePlayerElement();
        }
    }
	bool IsPointerOverUIObject()
	{
		List<RaycastResult> results = new List<RaycastResult>();
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

		return results.Count > 0;
	}
	public void ChangePlayerElement()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        CurrentElemental = (Element)(((int)CurrentElemental + 1) % 2);
        foreach (var fo in FireOrb)
        {
            fo.SetActive((CurrentElemental == Element.Fire));
        }
        foreach (var fo in IceOrb)
        {
            fo.SetActive((CurrentElemental == Element.Ice));
        }
        GameManager.instance.SwipeBG();
        audioSource.PlayOneShot(ChangeSound);
    }

    public void Ouch()
    {
        Instantiate(DeathPrefab, transform.position, Quaternion.identity);
        GameManager.instance.AutoRestart();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Glass") || collision.gameObject.CompareTag("SmallWall"))
        {
            isWall = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Glass") || collision.gameObject.CompareTag("SmallWall"))
        {
            isWall = false;
        }
    }
}
