using Cinemachine;

using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine.Audio;
public class Player : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip coinsound;
    public string rebus = "�cole";
    public GameObject reponserebu;
    public GameObject rebusresult;
    public enum gameState
    {
        lv1,
        lv2,
        lv3
    }
    public GameObject lv1, lv2, lv3;

    public gameState myState = gameState.lv1;

    [SerializeField] private Camera cam;
    public Transform target, target2, target3;
    public GameObject field;

    [Header("cam")]
    public GameObject vcam1, vcam2, vcam3;
    public GameObject currentCam;
    public GameObject rocurrentRoomCam;
    public GameObject roomCam1, roomCam2, roomCam3;

    [SerializeField] private float distanceToTarget = 10;
    public static RoomCam instance;
    private Vector3 previousPosition;


    public GameObject light;
    public GameObject currentobject;
    public GameObject oldobject;

    public TextMeshProUGUI buttontext;


    public int score;

    public TextMeshPro scoretext;

    public static Player playerinstance;

    public GameObject diceDisplay;
    public GameObject buttonclose;

    public AudioClip amogus;
    // Start is called before the first frame update
    private void Awake()
    {
        if (playerinstance == null)
        {
            playerinstance = this;
        }
        else
        {
            Destroy(this);
        }
    }
       

    private void Start()
    {
        audioSource = GameObject.Find("audio").GetComponent<AudioSource>();
        rebusresult.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        //changecam(myState);
        scoretext.text = score.ToString();
        if(reponserebu.GetComponent<TMP_InputField>().text==rebus)
        {
            rebusresult.SetActive(true);
        }
        else { }
        if (score == 10)
        {
            scoretext.color = Color.green;
        }
        Vector3 Mousepos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
        Ray ray = Camera.main.ScreenPointToRay(Mousepos);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 1000, Color.red, 5f);
        RaycastHit hover;
        if (oldobject != null)
        {

        }
        if (Physics.Raycast(transform.position, ray.direction, out hover, 100, 3))
        {
            //light.transform.position = Vector3.Lerp(light.transform.position, hover.point, Time.deltaTime * 10);

        }
        else
        {

        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            currentCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView -= 2;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            currentCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView += 2;
        }
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, ray.direction, out hit, 100, 3))
            {
                if (hit.transform.tag == "piece")
                {
                    hit.transform.GetComponent<TablePiece>().check();
                    Debug.Log("piece");
                }
                else if (hit.transform.tag == "coin")
                {
                    score++;
                    Destroy(hit.transform.gameObject);
                    audioSource.PlayOneShot(coinsound);
                }
                else if (hit.transform.tag == "movable")
                {
                    hit.transform.GetComponent<clickandmove>().move();
                }
                else
                {
                    roomCam1.SetActive(true);
                    currentCam.SetActive(false);
                    RoomCam.instance.target = hit.transform;
                    RoomCam.instance.lookarond();
                    RoomCam.instance.GetComponent<CinemachineVirtualCamera>().Priority = 20;
                }
                if (hit.transform.tag == "code")
                {
                    // Debug.Log("oui");

                    field.SetActive(true);
                    buttonclose.SetActive(true);
                    roomCam1.SetActive(true);
                    currentCam.SetActive(false);
                    RoomCam.instance.GetComponent<CinemachineVirtualCamera>().Priority = 1;
                    currentCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 30;
                }
                if (hit.transform.tag == "amogus")
                {
                    
                    audioSource.PlayOneShot(amogus);
                }
                if (hit.transform.tag == "dice")
                {
                    // Debug.Log("oui");
                    diceDisplay.SetActive(true);
                    buttonclose.SetActive(true);
                }
                if (hit.transform.tag == "rebu")
                {
                    reponserebu.SetActive(true);
                    buttonclose.SetActive(true);
                }
            }
        }
            if (Input.GetMouseButton(2))
            {
            RoomCam.instance.gameObject.SetActive(false);
            currentCam.SetActive(true);
            RoomCam.instance.GetComponent<CinemachineVirtualCamera>().Priority = 1;
            currentCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 30;
        }
            if (Input.GetMouseButtonDown(1))
            {
                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(1))
            {
                if (currentCam.activeInHierarchy)
                {
                    Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                    Vector3 direction = previousPosition - newPosition;

                    float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
                    float rotationAroundXAxis = direction.y * 180; // camera moves vertically

                    currentCam.transform.position = target.position;

                    currentCam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                    currentCam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <� This is what makes it work!

                    currentCam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

                    previousPosition = newPosition;
                }
            }

        
    }
        public void close()
        {
            field.SetActive(false);
            diceDisplay.SetActive(false);
            buttonclose.SetActive(false);
            reponserebu.SetActive(false);
        }
        public void opendice()
        {
            diceDisplay.SetActive(true);
        }
        public void codebutton(int num)
        {
            Debug.Log(num);
            buttontext.text += num;
        }
        public void resetstring()
        {
            Debug.Log("reset");
            buttontext.text = "";
        }
        public void changecam(gameState state)
        {
            myState = state;
            switch (state)
            {
                case gameState.lv1:
                    currentCam = vcam1;
                    lv2.SetActive(false);
                    lv3.SetActive(false);
                    break;
                case gameState.lv2:
                    lv2.SetActive(true);
                    lv1.transform.DOMoveX(-100, 3f);
                    lv2.transform.DOMoveX(-1, 3f);
                    RoomCam.instance.gameObject.SetActive(false);
                    roomCam1.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 30;
                    currentCam.SetActive(true);
                break;
                case gameState.lv3:
                    lv1.SetActive(false);
                    lv3.SetActive(true);
                    lv2.transform.DOMoveX(-100, 3f);
                    lv3.transform.DOMoveX(1.81f, 3f);
                    RoomCam.instance.gameObject.SetActive(false);
                    roomCam1.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 30;
                currentCam.SetActive(true);
                //rocurrentRoomCam = roomCam1;
                break;
                default:
                    break;
            }
            //previousPosition = currentCam.transform.position;
        }
    }

