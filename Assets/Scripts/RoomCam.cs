using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RoomCam : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public Transform target;
    [SerializeField] private float distanceToTarget = 10;
    public static RoomCam instance;
    private Vector3 previousPosition;
    Vector3 newPosition, direction;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        transform.position = new Vector3(-5.91f, 4.49f, -6.26f);
    }
    void Update()
    {
        
        Vector3 Mousepos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
        Ray ray = Camera.main.ScreenPointToRay(Mousepos);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView -= 2;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView += 2;
        }
        


        if (Input.GetMouseButtonDown(1))
        {
            previousPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            //lookarond();
            //transform.DOLookAt(target.transform.position, 2f);
        }
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, ray.direction, out hit, 100))
            {
                if (hit.transform.tag == "todestroy")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        else if (Input.GetMouseButton(1))
        {
            lookarond();
        }
    }
    public void lookarond()
    {
        newPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        direction = previousPosition - newPosition;

        float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
        float rotationAroundXAxis = direction.y * 180; // camera moves vertically

        transform.position = target.position;

        transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
        transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <� This is what makes it work!

        transform.Translate(new Vector3(0, 0, -distanceToTarget));

        previousPosition = newPosition;
    }
}
