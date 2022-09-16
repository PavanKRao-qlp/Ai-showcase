using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateSet = GoapWorldState;
public class SampleGOAP : MonoBehaviour
{
    public UnityEngine.UI.Text  _Text;

    List<Goal> goals_;
    List<ActionGOAP> actions_;

    StateSet  our_state;

    void Start()
    {
        goals_ = new List<Goal>();
        actions_ = new List<ActionGOAP>();
        our_state = new StateSet();
        Init();
    }

    private void Init()
    {
        Vector2 X = UnityEngine.Random.insideUnitCircle * 5;
        goals_.Add(new Goal()
        {
            Name = $"MoveTo{X.ToString()}" ,
            GoalState = new StateSet {("At",X)}
        });

        actions_.Add(new ActionGOAP("MoveTo")
        {
            Cost = 5,
            Requires = new StateSet { },
            Satisfies = new StateSet { ("At", "%dynamic%") },
            DynamicKey = "At"
        });
        actions_.Add(new ActionGOAP("PickUp")
        {
            Cost = 3,
            Requires = new StateSet {("Carry","Nothing")},
            Satisfies = new StateSet { ("Carry", "%dynamic%") },
            DynamicKey = "Carry"
        });
        actions_.Add(new ActionGOAP("Drop")
        {
            Cost = 3,
            Requires = new StateSet { ("Carry", "Nothing") },
            Satisfies = new StateSet { ("Carry", "%dynamic%") },
            DynamicKey = "Carry"
        });
        actions_.Add(new ActionGOAP("DoNothing")
        {
            Cost = 2,
            Requires = new StateSet { },
            Satisfies = new StateSet { },
            DynamicKey = null
        });

#if Old
        goals_.Add(new Goal("rest", new HashSet<(string, object)> { ("has", "bed") }));

        actions_.Add(new ActionGOAP("make the bed")
        {
            _Cost = 3,
            _Requires = { ("has", "clean_bed") },
            _Satisfies = { ("has", "bed") }
        });
        actions_.Add(new ActionGOAP("clean the bed")
        {
            _Cost = 10,
            _Requires = { },
            _Satisfies = { ("has", "clean_bed") }
        });
        actions_.Add(new ActionGOAP("clean the bed shabily")
        {
            _Cost = 3,
            _Requires = { },
            _Satisfies = { ("has", "clean_bed") }
        });
        actions_.Add(new ActionGOAP("buy new bed")
        {
            _Cost = 5,
            _Requires = { ("has", "money") },
            _Satisfies =  {
                ("has", "bed"),
                ("has", "clean_bed")
            }
        });
        actions_.Add(new ActionGOAP("get money")
        {
            _Cost = 1,
            _Requires = { },
            _Satisfies = { ("has", "money") }
        });
        actions_.Add(new ActionGOAP("make the food")
        {
            _Cost = 4,
            _Requires = { },
            _Satisfies = { ("has", "food") }
        });
#endif
    }

    void UpdateState()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Goal goal = goals_[UnityEngine.Random.Range(0, goals_.Count)];
            List<ActionGOAP> plan =  PlannerGOAP.GetPlan(in goal ,in actions_ ,in our_state);
            _Text.text += $"\n The goal : {goal.Name} and plan is ";
            foreach (ActionGOAP action in plan)
            {
                _Text.text += $"\n {action._ActionName} : {action.Cost}";
            }
        }
    }
}
