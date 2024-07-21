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

    private void Start()
    {
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
        if(transform.position.z >13)
        {
            gameManager.GameOver();
        }
    }
}