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
    
    public AIManager AIManagerRef {
        private get; set; 
    }

    #region IAgentBT
    public string Id => GetAgentId();
    public BehaviorTree ActiveBehaviorTree => activeBehaviorTree;
    public GameObject GameObject => this.gameObject;
    private BehaviorTree activeBehaviorTree;
    private string GetAgentId()
    {
        if (string.IsNullOrEmpty(agentId)) agentId = Guid.NewGuid().ToString();
        return agentId;
    }
    #endregion

    void Start()
    {
        this.AIManagerRef.BlackBoard.AddEntity(Id);
        var BBEntity = AIManagerRef.BlackBoard.GetEntity(Id);
        BBEntity.canSeeTarget = false;
        BBEntity.targetId = "";
        BBEntity.health = health;
        BBEntity.condtition = 0;
        activeBehaviorTree = BuildBTTreeV2();
    }

    public BehaviorTree BuildBTTreeV2()
    {
        var btBuilder = new BehaviourTreeBuilder()
            .AttachAgent(this)
            .AttachBlackBoard(AIManagerRef.BlackBoard)
            .Selector()
                .Sequence()
                    .Monitor(new CheckHealthBelowX(0))
                    .AttachTask(new PlayAnimation("Dead"))
                    .AttachDecorater(new RepeatBTNode())
                        .AttachTask(new AlwasySucceedlBTNode())
                    .End()
                .End()
                .Sequence()
                    .Monitor(new CheckIfPlayerIsVisible())
                    .Conditional(new CheckIfTargetIsAlive())
                    .Selector()
                        .Sequence()
                             .Invert()
                                .Monitor(new CheckIfPlayerIsInRange(2f))
                             .End()
                             .Parallel()
                                .AttachDecorater(new RepeatBTNode())
                                    .AttachTask(new SetPositionNearTarget())
                                .End()
                                .AttachTask(new GoToPosition())
                            .End()
                        .End()
                        .Parallel()
                            .Composite(new SelectorRandomBTNode())
                                .AttachTask(new PlayAnimation("Punch1"))
                                .AttachTask(new PlayAnimation("Punch2"))
                            .End()
                            .Sequence()
                                .AttachTask(new WaitXSec(0.4f))
                                .AttachTask(new AttackTarget())
                            .End()
                        .End()
                    .End()
                .End()
                .Sequence()
                    .AttachTask(new SetRandomPositionNearby() { DistanceRange = 50})
                    .AttachTask(new GoToPosition())
                    .AttachDecorater(new RepeatBTNode(2))
                        .AttachTask(new PlayAnimation("Idle"))
                    .End()
                .End()
            .End();
        return btBuilder.BuildTree();
    }
    public BehaviorTree BuildBTTreeTest()
    {
        var btBuilder = new BehaviourTreeBuilder()
            .AttachAgent(this)
            .AttachBlackBoard(AIManagerRef.BlackBoard)
            .Sequence()
                .Monitor(new CheckIfPlayerIsVisible())
                .Conditional(new CheckIfTargetIsAlive())
                .Selector()
                    .Sequence()
                        .Invert()
                            .Monitor(new CheckIfPlayerIsInRange(2f))
                        .End()
                        .Parallel()
                            .AttachDecorater(new RepeatBTNode())
                                .AttachTask(new SetPositionNearTarget())
                            .End()
                            .AttachTask(new GoToPosition())
                        .End()
                    .End()
                    .Parallel()
                        .Composite(new SelectorRandomBTNode())
                            .AttachTask(new PlayAnimation("Punch1"))
                            .AttachTask(new PlayAnimation("Punch2"))
                        .End()
                        .Sequence()
                            .AttachTask(new WaitXSec(0.4f))
                            .AttachTask(new AttackTarget())
                        .End()
                    .End()
                 .End();
        return btBuilder.BuildTree();
    }


    [Obsolete(message: "Use BTBuilder for cleaner code")]
     public BehaviorTree BuildBT()
    {
        var bt = new BehaviorTree() {
            Agent = this,
            Blackboard = AIManagerRef.BlackBoard
        };
        bt.RootNode.SetChild(
            new SelectorBTNode()
            {
                TagName = "Selector",
                BT = bt,
                ChildNodes = new List<IBTNode>()
                {
                    new SequenceBTNode()
                    {

                TagName = "Death seq",
                        BT = bt,
                        ChildNodes = new List<IBTNode>()
                        {
                            new CheckHealthBelowX(0) {BT = bt}, //todo check if health < 0
                            new PlayAnimation("Dead"){BT = bt},
                            new RepeatBTNode(){
                                  TagName = "DeathRepeat",
                                BT = bt,
                                ChildNode = new AlwasySucceedlBTNode(){BT = bt}
                            }
                        }
                    },
                    new SequenceBTNode
                    {
                        TagName = "player visible seq",
                        BT = bt,
                        ChildNodes = new List<IBTNode>()
                        {
                            new CheckIfPlayerIsVisible() {BT = bt},// check if player is visible
                            new SelectorBTNode()
                            {
                                TagName = "visible health select",
                                BT = bt,
                                ChildNodes = new List<IBTNode>()
                                {
                                    new SequenceBTNode()
                                    {
                                        TagName = "safe area seq",
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
                                           new SetPositionNearTarget(){ BT = bt },  
                                           new GoToPosition(){ BT = bt },
                                           new ParallelBTNode()
                                           {
                                                BT = bt,
                                                ChildNodes = new List<IBTNode>()
                                                {
                                                    new SelectorRandomBTNode()
                                                    {
                                                        BT = bt,
                                                        ChildNodes = new List<IBTNode>()
                                                        {
                                                            new PlayAnimation("Punch1"){BT = bt},
                                                            new PlayAnimation("Punch2"){BT = bt},
                                                        }
                                                    },
                                                    new SequenceBTNode() 
                                                    {
                                                        BT = bt,
                                                        ChildNodes = new List<IBTNode>()
                                                        {
                                                            new WaitXSec(0.4f){BT = bt},
                                                            new AttackTarget() {BT = bt }
                                                        }
                                                    }                                                    
                                                }
                                           }
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
        if (health <= 0)
            return;
        health = health - 45 < 0 ? 0 : health - 45;
        animator.SetTrigger("hit");
        AIManagerRef.BlackBoard.GetEntity(Id).health = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var BBEntity = AIManagerRef.BlackBoard.GetEntity(Id);
            BBEntity.condtition = 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var BBEntity = AIManagerRef.BlackBoard.GetEntity(Id);
            BBEntity.condtition = 2;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var BBEntity = AIManagerRef.BlackBoard.GetEntity(Id);
            BBEntity.condtition = 0;
        }

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
        float angle = 90;
        float fine = 20;
        for (int i = 0; i <= fine; i++)
        {
            var ray = new Ray(this.transform.position + new Vector3(0, 2, 0) + (transform.forward * 0.75f), Quaternion.AngleAxis(-angle / 2 + ((i / fine) * angle), Vector3.up) * (transform.forward.normalized * 20));
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, 20, layerMask) && hitinfo.collider.tag != "Untagged")
            {
                string otherId = hitinfo.collider.GetComponent<IBlackBoardEntity>()?.Id ?? null;
                if (!string.IsNullOrEmpty(otherId) && otherId == "player")
                {
                        AIManagerRef.BlackBoard.GetEntity(Id).canSeeTarget = true;
                        AIManagerRef.BlackBoard.GetEntity(Id).targetPos = hitinfo.collider.transform.position;
                        AIManagerRef.BlackBoard.GetEntity(Id).targetId = otherId;
                        AIManagerRef.BlackBoard.GetEntity(Id).targetDist = (this.transform.position - hitinfo.collider.transform.position).magnitude;
                        foundTarget = true;
                        Debug.DrawLine(this.transform.position + new Vector3(0, 2, 0), hitinfo.point, Color.red);
                        break;                   
                }
            }
            else
            {
                Debug.DrawRay(this.transform.position + new Vector3(0, 2, 0) + (transform.forward * 0.75f), hitinfo.point == null ? hitinfo.point : Quaternion.AngleAxis(-angle / 2 + ((i / fine) * angle), Vector3.up) * (transform.forward.normalized * 20), Color.green);
            }
        }
        if (!foundTarget)
        {
            AIManagerRef.BlackBoard.GetEntity(Id).canSeeTarget = false;
            AIManagerRef.BlackBoard.GetEntity(Id).targetId = "";
            AIManagerRef.BlackBoard.GetEntity(Id).targetDist = float.MaxValue;
        }
    }

    public void TryAttack()
    {
        float angle = 90;
        float fine = 15;
        for (int i = 0; i <= fine; i++)
        {
            var ray = new Ray(this.transform.position + new Vector3(0, 2, 0) + (transform.forward * 0.75f), Quaternion.AngleAxis(-angle / 2 + ((i / fine) * angle), Vector3.up) * (transform.forward.normalized * 20));
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, 20, layerMask) && hitinfo.collider.gameObject.GetComponent<PlayerController>() != null)
            {
                hitinfo.collider.gameObject.GetComponent<PlayerController>().GetAttacked();
                break;
            }
            else
            {
                Debug.DrawRay(this.transform.position + new Vector3(0, 2, 0) + (transform.forward * 0.75f), hitinfo.point == null ? hitinfo.point : Quaternion.AngleAxis(-angle / 2 + ((i / fine) * angle), Vector3.up) * (transform.forward.normalized * 20), Color.green);
            }
        }
    }
}
