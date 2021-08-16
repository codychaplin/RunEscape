using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    public Interactable focus; // focusable object
    public LayerMask excludeMask; // raycast mask

    Camera cam;
    NavMeshAgent agent;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0)) // if left click (move)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, ~excludeMask)) // if raycast is successfull
            {
                // snap to grid and move to point
                MoveToPoint(new Vector3Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z)));
                RemoveFocus(); // remove focus, if any
            }
        }

        if (Input.GetMouseButtonDown(1)) // if right click (interact)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                
                if (interactable != null) // if focus is interactable, set focus
                    SetFocus(interactable);
            }
        }

        if (target != null) // if focused
        {
            agent.SetDestination(target.position); // go to target position
            FaceTarget(); // align body with target direction
        }
    }

    void MoveToPoint(Vector3Int point)
    {
        agent.SetDestination(point); // go to destination
    }

    void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius; // sets stopping distance
        agent.updateRotation = false; // do not update rotation
        target = newTarget.interactionTransform; // target is target's transform
    }

    void UnFollowTarget()
    {
        agent.stoppingDistance = 0f; // reset stopping distance
        agent.updateRotation = true; // update rotation
        target = null; // remove target
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus) // if focus is different from current focus
        {
            if (focus != null) // if focused on something else
                focus.OnDefocused();

            focus = newFocus; // sets new focus
            FollowTarget(newFocus); // sets target to follow
        }

        newFocus.OnFocused(transform); // provides player's transform
    }

    void RemoveFocus()
    {
        if (focus != null) // if focused, defocus
            focus.OnDefocused();

        focus = null; // remove focus
        UnFollowTarget();
    }
}
