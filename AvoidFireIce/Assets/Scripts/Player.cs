using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Element CurrentElemental = Element.Fire;
    public List<GameObject> FireOrb;
    public List<GameObject> IceOrb;
    public GameObject DeathPrefab;
    public GameObject Background;
    public AudioClip MoveSound;
    public AudioClip ChangeSound;


    public float speed = 1f;

    private Vector2 direction = Vector2.zero;


    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isWall = false;
    private bool isLoopSoundPlaying = false;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = MoveSound;
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

        direction = new Vector3(h, v);
        if (!isWall)
        {
            direction = new Vector3(h, v);
            float directionMag = direction.magnitude;
            if (directionMag > 1)
            {
                direction.Normalize();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePlayerElement();
        }

        if (direction != Vector2.zero)
        {
            if (!isLoopSoundPlaying)
            {
                audioSource.Play();
                isLoopSoundPlaying = true;
            }
        }
        else
        {
            if (isLoopSoundPlaying)
            {
                audioSource.Stop();
                isLoopSoundPlaying = false;
            }
        }
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
