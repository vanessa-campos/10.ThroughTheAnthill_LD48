using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float Speed = 3;
    [SerializeField] float slowSpeed = 1.5f;
    [SerializeField] GameObject PointsPopup;
    [SerializeField] Text pointsText;
    [SerializeField] Text barText;
    [SerializeField] Slider bar;
    [SerializeField] Image[] ants;
    [SerializeField] AudioClip eatSound = null;
    [SerializeField] AudioClip digSound = null;
    [SerializeField] AudioClip leafSound = null;
    [SerializeField] AudioClip completedSound = null;

    Rigidbody _rigidbody;
    Animator _animator;
    ParticleSystem dirtyParticle;
    AudioSource bugSound;
    GameManager game;
    float horizontalInput;
    float verticalInput;
    float originalSpeed;
    Vector3 initialPos;
    Quaternion initialRot;
    TextMesh textPointsPopup;
    GameObject leafPrefab;
    bool leaf;
    int leaves;


    int stamina;
    public int Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            if (stamina < 0)
            {
                stamina = 0;
            }
            if (stamina > 100)
            {
                stamina = 100;
            }
            barText.text = stamina.ToString();
        }
    }

    int points;
    public int Points { get { return points; } set { points = value; pointsText.text = "POINTS: " + points; } }
    int life;
    public int Life { get { return life; } set { life = value; } }



    IEnumerator BugSound()
    {
        yield return new WaitForSeconds(5);
        bugSound.Play();
        StartCoroutine(BugSound());
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        dirtyParticle = GetComponentInChildren<ParticleSystem>();
        bugSound = GetComponent<AudioSource>();
        game = FindObjectOfType<GameManager>();
        textPointsPopup = PointsPopup.GetComponent<TextMesh>();
        Life = 3;
        Stamina = 100;
        Points = 0;
        originalSpeed = Speed;
        initialPos = transform.position;
        initialRot = transform.rotation;
        leaves = 9;
        StartCoroutine(BugSound());
    }

    void Update()
    {
        // Move
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        _rigidbody.velocity = new Vector3(horizontalInput * Speed, verticalInput * Speed, 0);
        // Check rotation
        Rotation();

        //Animation
        bool move;
        if (horizontalInput != 0 || verticalInput != 0)
        {
            move = true;
        }
        else
        {
            move = false;
        }
        _animator.SetBool("move", move);

        // StaminaBar
        bar.value = Stamina * .01f;
        if (stamina <= 0)
        {
            Life -= 1;
            ants[Life].color = new Vector4(0, 0, 0, 0);
            transform.position = initialPos;
            transform.rotation = initialRot;
            Stamina = 100;
        }
        if (Life <= 0)
        {
            game.gameOver = true;
        }

        // Set record value
        // PlayerPrefs.GetInt("PPRecord", game.Record);
        // if (points > game.Record)
        // {
        //     game.Record = points;
        //     PlayerPrefs.SetInt("PPRecord", game.Record);
        // }
    }

    IEnumerator HidePointsPopup()
    {
        yield return new WaitForSeconds(1);
        PointsPopup.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("tile1"))
        {
            Speed = originalSpeed;
        }
        if (other.gameObject.CompareTag("tile2"))
        {
            Speed = slowSpeed;
            if (digSound != null)
            {
                AudioSource.PlayClipAtPoint(digSound, transform.position, 1f);
            }
            Stamina -= 5;
            other.GetComponent<Animator>().SetTrigger("dig");
            dirtyParticle.Play();
            textPointsPopup.text = "-5";
            PointsPopup.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, -1);
            PointsPopup.SetActive(true);
            StartCoroutine(HidePointsPopup());
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, .5f);
        }
        if (other.gameObject.CompareTag("tile3"))
        {
            if (leaf)
            {
                Destroy(leafPrefab);
                if (leafSound != null)
                {
                    AudioSource.PlayClipAtPoint(leafSound, transform.position, 1f);
                }
                Points += 50;
                textPointsPopup.text = "+50";
                PointsPopup.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, -1);
                PointsPopup.SetActive(true);
                StartCoroutine(HidePointsPopup());
                other.transform.Rotate(0, 90, 90);
                leaf = false;
            }

        }
        if (other.gameObject.CompareTag("candyGood"))
        {
            if (eatSound != null)
            {
                AudioSource.PlayClipAtPoint(eatSound, other.transform.position, 5);
            }
            Stamina += 100;
            textPointsPopup.text = "+100";
            PointsPopup.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, -1);
            PointsPopup.SetActive(true);
            StartCoroutine(HidePointsPopup());
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("candyBad"))
        {
            if (eatSound != null)
            {
                AudioSource.PlayClipAtPoint(eatSound, other.transform.position, 5);
            }
            Stamina -= 30;
            textPointsPopup.text = "-30";
            PointsPopup.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, -1);
            PointsPopup.SetActive(true);
            StartCoroutine(HidePointsPopup());
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("leaf"))
        {
            if (!leaf)
            {
                leaf = true;
                leaves -= 1;
                other.transform.parent = transform;
                other.GetComponent<SphereCollider>().radius = 0;
                leafPrefab = other.gameObject;
            }
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            if (leaves <= 0)
            {
                game.levelCompleted = true;
                if (completedSound != null)
                {
                    AudioSource.PlayClipAtPoint(completedSound, transform.position, 1f);
                }
            }
        }
    }

    void Rotation()
    {
        if (horizontalInput > 0)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 90));
        }
        if (horizontalInput < 0)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 270));
        }
        if (verticalInput > 0)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 180));
        }
        if (verticalInput < 0)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 0));
        }
        if (horizontalInput > 0 && verticalInput > 0)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 145));
        }
        if (horizontalInput < 0 && verticalInput < 0)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 315));
        }
        if (horizontalInput > 0 && verticalInput < 0)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 45));
        }
        if (horizontalInput < 0 && verticalInput > 0)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 225));
        }
    }

}
