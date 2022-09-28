using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour , IAgentBT
{
    [SerializeField] private string agentId;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int health = 100;
    [SerializeField] private TextMesh label;
    public string Id => GetAgentId();
    public BehaviorTree ActiveBehaviorTree => activeBehaviorTree;   
    public AIManager AIManagerRef { private get; set; }
    public GameObject GameObject => this.gameObject;
    private BehaviorTree activeBehaviorTree;
    private string GetAgentId()
    {
        if (string.IsNullOrEmpty(agentId)) agentId = Guid.NewGuid().ToString();
        return agentId;
    }

    void Start()
    {
        activeBehaviorTree = BuildBT();
        AIManagerRef.BlackBoard.GetEntity(Id).health = health;
        AIManagerRef.BlackBoard.GetEntity(Id).canSeeTarget = false;
        AIManagerRef.BlackBoard.GetEntity(Id).targetId = "";
    }

    public BehaviorTree BuildBT()
    {
        var bt = new BehaviorTree() {
            Agent = this,
            Blackboard = AIManagerRef.BlackBoard
        };
        bt.RootNode.SetChild(
            new SelectorBTNode()
            {
                BT = bt,
                ChildNodes = new List<IBTNode>()
                {
                    new SequenceBTNode()
                    {
                        BT = bt,
                        ChildNodes = new List<IBTNode>()
                        {
                            new CheckHealthBelowX(0) {BT = bt}, //todo check if health < 0
                            new PlayAnimation("Dead"){BT = bt},
                            new RepeatBTNode(){
                                BT = bt,
                                ChildNode = new AlwasySucceedlBTNode(){BT = bt}
                            }
                        }
                    },
                    new SequenceBTNode
                    {
                        BT = bt,
                        ChildNodes = new List<IBTNode>()
                        {
                            new CheckIfPlayerIsVisible() {BT = bt},// check if player is visible
                            new SelectorBTNode()
                            {
                                BT = bt,
                                ChildNodes = new List<IBTNode>()
                                {
                                    new SequenceBTNode()
                                    {
                                        BT = bt,
                                        ChildNodes = new List<IBTNode>()
                                        {
                                            new CheckHealthBelowX(20) { BT = bt }, //check if enemy is in critical health
                                            new FindSafeArea() {BT = bt},
                                            new GoToPosition() {BT = bt}
                                        }
                                    },
                                    new SequenceBTNode()
                                    {
                                        BT = bt,
                                        ChildNodes = new List<IBTNode>()
                                        { 
                                           new GoToPosition(){ BT = bt },
                                           new ParallelBTNode()
                                            {
                                                BT = bt,
                                                ChildNodes = new List<IBTNode>()
                                                {
                                                   // new selectorRandom->punch1/2 
                                                    //new attackTarget
                                                }
                                            }
                                            // new get position to 
                                            // get away run animation 
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new SequenceBTNode()
                    {
                        BT = bt,
                        ChildNodes =  new List<IBTNode>(){
                            new SequenceBTNode() // Wander 
                            {
                                BT = bt,
                                ChildNodes = new List<IBTNode>
                                {
                                    new SetRandomPositionNearby(){ DistanceRange = 50 , BT = bt },
                                    new GoToPosition(){ BT = bt },
                                    new RepeatBTNode(2) {
                                         BT = bt,
                                         ChildNode = new PlayAnimation("Idle"){BT = bt}
                                    }
                                }
                            }
                        }
                    }
                }
            });
        return bt;
    }

    public void GetAttacked()
    {
        health -= 10;
        animator.SetTrigger("hit");
        AIManagerRef.BlackBoard.GetEntity(Id).health = health;
    }

    // Update is called once per frame
    void Update()
    {
        label.transform.LookAt(Camera.main.transform);
        label.text = $"[AI] Hp:{ AIManagerRef.BlackBoard.GetEntity(Id).health }";

        AIManagerRef.BlackBoard.GetEntity(Id).health = health;
        CheckCanSeeTarget();
        animator.SetFloat("dirX", navMeshAgent.velocity.x);
        animator.SetFloat("dirY", navMeshAgent.velocity.z);
        animator.SetBool("isWalking", navMeshAgent.velocity.magnitude > 0f);
        animator.SetBool("isIdle", navMeshAgent.velocity.sqrMagnitude <= 0.1f);
    }

    public void CheckCanSeeTarget() {
        var foundTarget = false;
        for (int i = 0; i <= 10; i++)
        {
            var ray = new Ray(this.transform.position + new Vector3(0, 2, 0) + (transform.forward * 0.75f), Quaternion.AngleAxis(-45 / 2 + ((i / 10f) * 45), Vector3.up) * (transform.forward.normalized * 20));
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, 20, layerMask) && hitinfo.collider.tag != "Untagged")
            {
                string otherId = hitinfo.collider.GetComponent<IBlackBoardEntity>()?.Id ?? null;
                if (!string.IsNullOrEmpty(otherId) && otherId == "player")
                {
                    if (AIManagerRef.BlackBoard.GetEntity(otherId).health > 0)
                    {
                        AIManagerRef.BlackBoard.GetEntity(Id).canSeeTarget = true;
                        AIManagerRef.BlackBoard.GetEntity(Id).targetPos = hitinfo.collider.transform.position;
                        AIManagerRef.BlackBoard.GetEntity(Id).targetId = otherId;
                        foundTarget = true;
                        Debug.DrawLine(this.transform.position + new Vector3(0, 2, 0), hitinfo.point, Color.red);
                        break;
                    }
                }
            }
            else
            {
                Debug.DrawRay(this.transform.position + new Vector3(0, 2, 0) + (transform.forward * 0.75f), Quaternion.AngleAxis(-45 / 2 + ((i / 10f) * 45), Vector3.up) * (transform.forward.normalized * 20), Color.green);
            }
            if (!foundTarget)
            {
                AIManagerRef.BlackBoard.GetEntity(Id).canSeeTarget = false;
                AIManagerRef.BlackBoard.GetEntity(Id).targetId = "";
            }
        }
    }
}
