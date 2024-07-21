using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;

    public bool canMove = true;

    public TextMeshProUGUI Inst;

    public GameManager gameManager;

    public Animator anim;
    public Animator anim1;
    public Animator anim2;

    private void Start()
    {
        anim = GameObject.Find("PlayerCharacter").GetComponent<Animator>();
        anim1 = GameObject.Find("Persian").GetComponent<Animator>();
        anim2 = GameObject.Find("Persian2").GetComponent<Animator>();
        Inst.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            Inst.gameObject.SetActive(false);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            anim2.SetBool("IsWalking", true);
            anim1.SetBool("IsWalking", true);
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim2.SetBool("IsWalking", false);
            anim1.SetBool("IsWalking", false);
            anim.SetBool("IsWalking", false);
        }

        if (transform.position.z >13)
        {
            gameManager.GameOver();
        }
    }
}