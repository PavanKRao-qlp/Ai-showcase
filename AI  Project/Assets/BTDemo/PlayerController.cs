using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour , IBlackBoardEntity
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Animator animator;
    [SerializeField] TextMesh label;
    [SerializeField] AIManager aiManagerRef;
    private float health;
    public string Id => "player";

    void Start()
    {
        health = 100;
        aiManagerRef.BlackBoard.AddEntity(Id);
        aiManagerRef.BlackBoard.GetEntity(Id).health = health;
    }

    // Update is called once per frame
    void Update()
    {
        label.text = $"[Player] Hp:{health}";
        label.transform.LookAt(Camera.main.transform);

        aiManagerRef.BlackBoard.GetEntity(Id).pos = transform.position;
        aiManagerRef.BlackBoard.GetEntity(Id).health = health;


        if (health <= 0)
        {
            animator.Play("Dead");
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray,out hitInfo,1000,layerMask,QueryTriggerInteraction.Ignore))
            {
                navMeshAgent.SetDestination(hitInfo.point);
            }
            Debug.DrawRay(ray.origin, ray.direction*100,Color.red);
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hitInfo;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 1000, layerMask, QueryTriggerInteraction.Ignore))
            {
              transform.LookAt(hitInfo.point);
              if (Random.value > 0.5f) animator.Play("Punch1"); else animator.Play("Punch2");
                for (int i = 0; i <= 10; i++)
                {
                    var atkRay = new Ray(this.transform.position + new Vector3(0, 2, 0) + (transform.forward * 0.75f), Quaternion.AngleAxis(-90 / 2 + ((i / 10f) * 90), Vector3.up) * (transform.forward.normalized * 15));
                    RaycastHit hitinfo;
                    if (Physics.Raycast(atkRay, out hitinfo, 15, layerMask) && hitInfo.transform.gameObject.GetComponent<EnemyAi>() != null)
                    {
                        var enemyAi = hitInfo.transform.gameObject.GetComponent<EnemyAi>();
                        enemyAi.GetAttacked();
                        Debug.DrawLine(this.transform.position + new Vector3(0, 2, 0), hitinfo.point, Color.red,0.2f);
                        break;
                    }
                    else
                    {
                        Debug.DrawRay(this.transform.position + new Vector3(0, 2, 0) + (transform.forward * 0.75f), Quaternion.AngleAxis(-90 / 2 + ((i / 10f) * 90), Vector3.up) * (transform.forward.normalized * 15), Color.green, 0.25f);
                    }
                }
            }           
        }

        animator.SetFloat("dirX", navMeshAgent.velocity.x);
        animator.SetFloat("dirY", navMeshAgent.velocity.z);
        animator.SetBool("isWalking", navMeshAgent.velocity.magnitude > 0f);
        animator.SetBool("isIdle", navMeshAgent.velocity.sqrMagnitude <= 0.1f);
    }

    public void GetAttacked()
    {
        health -= 10;
        animator.SetTrigger("hit");
        aiManagerRef.BlackBoard.GetEntity(Id).health = health;
    }

}
