using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3;
    public bool leaf;
    public int leavesTotal, totalPoints;
    public Transform pos;
    public GameObject tilePrefab, pointsPopup;
    public AudioClip eatSound = null, digSound = null, leafSound = null;

    Rigidbody _rigidbody;
    Animator _animator;
    ParticleSystem dirtyParticle;
    AudioSource bugSound;
    Vector3 initialPos;
    Quaternion initialRot;
    GameObject leafPrefab;
    BlackAnts blackAnt;
    LevelManager level;
    float horizontalInput, verticalInput, originalSpeed, slowSpeed;
    int stamina;

    public int Stamina{
        get { return stamina; }
        set{ stamina = value;
            if (stamina < 0){
                stamina = 0;
            }
            if (stamina > 100){
                stamina = 100;
            }
            level.barText.text = stamina.ToString();
        }
    }

    int points;
    public int Points { get { return points; } set { points = value; level.pointsText.text = "POINTS: " + points; } }
    int life;
    public int Life { get { return life; } set { life = value; } }
    int leaves;
    public int Leaves { get { return leaves; } set { leaves = value; level.leafText.text = "x" + leaves; } }


    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        dirtyParticle = GetComponentInChildren<ParticleSystem>();
        bugSound = GetComponent<AudioSource>();
        level = FindObjectOfType<LevelManager>();
        blackAnt = FindObjectOfType<BlackAnts>();
    }

    void Start(){
        Life = 3;
        Stamina = 100;
        originalSpeed = Speed;
        slowSpeed = 0.5f * Speed;
        initialPos = transform.position;
        initialRot = transform.rotation;
        Points = PlayerPrefs.GetInt("Points");
        leavesTotal = GameObject.FindGameObjectsWithTag("leaf").Length;
        Leaves = leavesTotal;
        StartCoroutine(BugSound());
    }

    IEnumerator BugSound(){
        yield return new WaitForSeconds(5);
        bugSound.Play();
        StartCoroutine(BugSound());
    }
    IEnumerator PointsPopup(string value){
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), 1, transform.position.z + Random.Range(-1.5f, 1.5f));
        GameObject pointsPopupPrefab = Instantiate(pointsPopup, pos, Quaternion.Euler(90, 0, 0));
        pointsPopupPrefab.GetComponent<TextMeshPro>().text = value;
        yield return new WaitForSeconds(.5f);                       
        Destroy(pointsPopupPrefab);
    }
    IEnumerator ShowTextLeaves(){
        level.leavesText.SetActive(true);
        yield return new WaitForSeconds(3);
        level.leavesText.SetActive(false);
    }
    IEnumerator ShowTextTip(){
        level.tipText.SetActive(true);
        yield return new WaitForSeconds(3);
        level.tipText.SetActive(false);
    }

    void Update(){

        // Move      
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

#if UNITY_ANDROID
        horizontalInput = level.floatingJoystick.Horizontal;
        verticalInput = level.floatingJoystick.Vertical;
#endif

        _rigidbody.velocity = new Vector3(horizontalInput * Speed, 0, verticalInput * Speed);
        // Check rotation
        Rotation();

        //Animation
        bool move;
        if (horizontalInput != 0 || verticalInput != 0){
            move = true;
        }else{
            move = false;
        }
        _animator.SetBool("move", move);

        // StaminaBar
        level.bar.value = Stamina * .01f;
        if (stamina <= 0){
            Life -= 1;
            level.ants[Life].color = new Vector4(0, 0, 0, 0);
            // transform.position = initialPos;
            // transform.rotation = initialRot;
            Stamina = 100;
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        // Set game over
        if (Life <= 0){
            level.gameOver = true;
        }

        // Set total points
        if (Points > PlayerPrefs.GetInt("Points")){
            PlayerPrefs.SetInt("Points", Points);
            totalPoints = PlayerPrefs.GetInt("Points");
        }
    }

    private void OnTriggerEnter(Collider other){

        Vector3 newTilePos = new Vector3(other.transform.position.x, -.5f, other.transform.position.z);

        switch (other.tag){     
            case "tile":
                Speed = slowSpeed;
            dirtyParticle.Play();
            if (digSound != null){
                AudioSource.PlayClipAtPoint(digSound, transform.position, 1f);
            }
            if(other.transform.position != newTilePos){                
                other.GetComponent<Animator>().SetTrigger("dig");
                Stamina -= 5;
                StartCoroutine(PointsPopup("-5"));
                other.transform.position = newTilePos;
            }
                break;
            case "tile0":
                Speed = slowSpeed;
                dirtyParticle.Play();
                if (digSound != null){
                    AudioSource.PlayClipAtPoint(digSound, transform.position, 1f);
                }
                if(other.transform.position != newTilePos){
                    other.GetComponent<Animator>().SetTrigger("dig");
                    other.transform.position = newTilePos;
                }
                break;
            case "leaf":
                if (!leaf){
                    leaf = true;
                    other.transform.parent = transform;
                    leafPrefab = other.gameObject;
                    if (GameObject.FindGameObjectsWithTag("blackAnt").Length > 0){
                        blackAnt.leaf = false;
                    }
                }
                break;
            case "ant":
                if (leaf){
                    Leaves -= 1;
                    Points += 50;
                    StartCoroutine(PointsPopup("+50")); if (leafSound != null){
                        AudioSource.PlayClipAtPoint(leafSound, transform.position, 1f);
                    }
                    leafPrefab.transform.parent = other.transform;
                    leafPrefab.transform.position = other.GetComponent<Ants>().pos.position;
                    leafPrefab.tag = "Untagged";
                    leaf = false;
                    Destroy(other.gameObject, 2);
                }
                if (leaves == 0){
                    StartCoroutine(ShowTextLeaves());
                    #if UNITY_ANDROID
                        Handheld.Vibrate();
                    #endif
                }
                break;            
            case "blackAnt":                
                leaf = false;
                other.GetComponent<BlackAnts>().leaf = true;
                leafPrefab.transform.parent = other.transform;
                leafPrefab.transform.position = other.GetComponent<Ants>().pos.position;
                break;
            case "candyGood":
                if (Stamina < 100){
                    if (eatSound != null){
                        AudioSource.PlayClipAtPoint(eatSound, other.transform.position, 5);
                    }
                Stamina += 100;
                StartCoroutine(PointsPopup("+30"));
                Destroy(other.gameObject);
                }
                break;
            case "candyBad":
                if (eatSound != null){
                    AudioSource.PlayClipAtPoint(eatSound, other.transform.position, 5);
                }
                Stamina -= 30;
                StartCoroutine(PointsPopup("-30"));
                Destroy(other.gameObject);
                break;
            case "Finish":
                if (Leaves == 0){
                    PlayerPrefs.SetInt("PlayerPoints", Points);
                    level.levelCompleted = true;
                    #if UNITY_ANDROID
                        Handheld.Vibrate();
                    #endif
                }else{
                    StartCoroutine(ShowTextTip());
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other){
        if ((other.gameObject.CompareTag("tile")) || (other.gameObject.CompareTag("tile0"))){
            Speed = originalSpeed;
        }
    }

    void Rotation(){
        if (horizontalInput > 0){
            _rigidbody.MoveRotation(Quaternion.Euler(0, 270, 0));
        }
        if (horizontalInput < 0){
            _rigidbody.MoveRotation(Quaternion.Euler(0, 90, 0));
        }
        if (verticalInput > 0){
            _rigidbody.MoveRotation(Quaternion.Euler(0, 180, 0));
        }
        if (verticalInput < 0){
            _rigidbody.MoveRotation(Quaternion.Euler(0, 0, 0));
        }
        if (horizontalInput > 0 && verticalInput > 0){
            _rigidbody.MoveRotation(Quaternion.Euler(0, 225, 0));
        }
        if (horizontalInput < 0 && verticalInput < 0){
            _rigidbody.MoveRotation(Quaternion.Euler(0, 45, 0));
        }
        if (horizontalInput > 0 && verticalInput < 0){
            _rigidbody.MoveRotation(Quaternion.Euler(0, 315, 0));
        }
        if (horizontalInput < 0 && verticalInput > 0){
            _rigidbody.MoveRotation(Quaternion.Euler(0, 135, 0));
        }
    }
}
