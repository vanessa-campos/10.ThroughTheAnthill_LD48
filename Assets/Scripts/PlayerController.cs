using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float Speed = 3;
    [SerializeField] float slowSpeed = 1.5f;
    [SerializeField] GameObject prefabTile1;
    [SerializeField] Text pointsText;
    [SerializeField] Text barText;
    [SerializeField] Slider bar;
    [SerializeField] Image[] ants = new Image[3];

    Rigidbody _rigidbody;
    Animator _animator;
    ParticleSystem dirtyParticle;
    float horizontalInput;
    float verticalInput;
    float originalSpeed;


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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        dirtyParticle = GetComponentInChildren<ParticleSystem>();
        Life = 3;
        Stamina = 100;
        Points = 0;
        originalSpeed = Speed;
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
        //Tirar uma vida quando a barra esvaziar e salvar o valor em PlayerPrefs
        Life = PlayerPrefs.GetInt("PPVida");
        if (stamina <= 0)
        {
            life -= 1;
            ants[Life - 1].color = new Vector4(0, 0, 0, 0);
            PlayerPrefs.SetInt("PPVida", Life);
            // GM.Jogo();
        }
        if (Life <= 0)
        {
            // GM.Fim();
        }
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
            Stamina -= 10;
            Points += 10;
            other.GetComponent<Animator>().SetTrigger("dig");
            dirtyParticle.Play();
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, .5f);
            // Destroy(other.gameObject);
            // Instantiate(prefabTile1, other.transform.position, Quaternion.identity);
        }
        if (other.gameObject.CompareTag("Candy"))
        {
            Destroy(other.gameObject);
            Stamina += 50;
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
