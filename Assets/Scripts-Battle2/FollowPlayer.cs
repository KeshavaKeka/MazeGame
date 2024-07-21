//using UnityEngine;
//using UnityEngine.AI;

//public class FollowerPlayer : MonoBehaviour
//{
//    public Transform player; // Reference to the player object
//    public NavMeshAgent agent; // Reference to the NavMeshAgent component
//    public Animator anim; // Reference to the Animator component

//    private NavMeshAgent playerAgent; // Reference to the player's NavMeshAgent

//    void Start()
//    {
//        // Ensure the agent component is attached
//        if (agent == null)
//        {
//            agent = GetComponent<NavMeshAgent>();
//        }

//        // Ensure the animator component is attached
//        if (anim == null)
//        {
//            anim = GetComponent<Animator>();
//        }

//        // Ensure the player object has a NavMeshAgent component
//        if (player != null)
//        {
//            playerAgent = player.GetComponent<NavMeshAgent>();
//        }
//    }

//    void Update()
//    {
//        if (playerAgent != null)
//        {
//            // Check if the player is moving
//            if (playerAgent.velocity.sqrMagnitude > 0.1f)
//            {
//                // Set the destination of the agent to the player's position
//                agent.SetDestination(player.position);

//                // Trigger the walking animation
//                anim.SetBool("isWalking", true);
//            }
//            else
//            {
//                // Stop the follower from moving
//                agent.ResetPath();

//                // Stop the walking animation
//                anim.SetBool("isWalking", false);
//            }
//        }
//    }
//}


using UnityEngine;
using UnityEngine.AI;

public class FollowerPlayer : MonoBehaviour
{
    public Animator anim;

    private NavMeshAgent agent;

    void Start()
    {
        if (anim == null || agent == null)
        {
            anim = GetComponent<Animator>();
            agent = GameObject.Find("Player").GetComponent<NavMeshAgent>();
        }
    }

    void Update()
    {
        if (agent != null)
        {
            if (agent.velocity.sqrMagnitude > 0.1f)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
    }
}
