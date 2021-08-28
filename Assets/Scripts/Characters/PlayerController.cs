using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public GameObject cube;
    public Interactable focus; // focusable object
    public LayerMask excludeMask; // raycast mask

    public int playerSpeed = 4;

    Camera cam;
    NavMeshAgent agent;
    Transform target;

    Pathfinding pathfinding;
    List<Vector3> pathList;
    int pathIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (Input.GetMouseButtonDown(0)) // if left click (move)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, ~excludeMask)) // if raycast is successfull
            {
                pathList = Pathfinding.Instance.FindVectorPath(SnapToGrid(transform.position), SnapToGrid(hit.point));
                pathIndex = 0;
                /*Vector3Int point = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z));
                List<Tile> path = Pathfinding.Instance.FindPath(0, 0, point.x, point.z);
                if (path != null)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        Vector3 pos = new Vector3(path[i].pos.x, 0, path[i].pos.z);
                        Instantiate(cube, pos, Quaternion.identity);
                    }
                }*/

                // snap to grid and move to point
                //MoveToPoint(new Vector3(point.x + 0.5f, point.y, point.z + 0.5f));
                //RemoveFocus(); // remove focus, if any
            }
        }

        if (Input.GetMouseButtonDown(1)) // if right click (interact)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, ~excludeMask))
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

    void Movement()
    {
        if (pathList != null)
        {
            Vector3 targetPosition = pathList[pathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDirection * playerSpeed * Time.deltaTime;
            }
            else
            {
                pathIndex++;
                if (pathIndex >= pathList.Count)
                {
                    pathList = null;
                    transform.position = SnapToCenter(transform.position);
                }
            }
        }
    }

    Vector3 SnapToGrid(Vector3 pos) // y is round not floor to avoid rounding to negative digits
    {
        return new Vector3(Mathf.FloorToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.FloorToInt(pos.z));
    }

    Vector3 SnapToCenter(Vector3 pos) // y is round not floor to avoid rounding to negative digits
    {
        return new Vector3(Mathf.FloorToInt(pos.x) + 0.5f, Mathf.RoundToInt(pos.y), Mathf.FloorToInt(pos.z) + 0.5f);
    }

    /*void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point); // go to destination
    }

    void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius; // sets stopping distance
        agent.updateRotation = false; // do not update rotation
        target = newTarget.transform; // target is target's transform
    }

    void UnFollowTarget()
    {
        agent.stoppingDistance = 0f; // reset stopping distance
        agent.updateRotation = true; // update rotation
        target = null; // remove target
    }*/

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x + 0.001f, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus) // if focus is different from current focus
        {
            if (focus != null) // if focused on something else
                focus.OnDefocused();

            focus = newFocus; // sets new focus
            //FollowTarget(newFocus); // sets target to follow
        }

        newFocus.OnFocused(transform); // provides player's transform
    }

    void RemoveFocus()
    {
        if (focus != null) // if focused, defocus
            focus.OnDefocused();

        focus = null; // remove focus
        //UnFollowTarget();
    }
}
