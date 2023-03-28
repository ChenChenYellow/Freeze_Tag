using System.Collections.Generic;
using UnityEngine;
public class Character : MonoBehaviour
{
    public Node Target;
    public Node MyNode;
    public float
        PathUpdateInterval = 2,
        FrozenTime = 30,
        ChaserSpeed = 2,
        EvaderSpped = 1,
        CrowdTime = 10,
        Radius = 1.1f;
    private float
        lastPathUpdate,
        frozenTime,
        crowdTime,
        alertTimeRemain = 0;
    public bool Alerted = false;
    private int chaserAllowance = 1;

    public enum CharacterState
    {
        Chaser,
        PatrolingChaser,
        ActiveEvader,
        PassiveEvader,
        FrozenEvader,
        UnfrozenEvader,
        RescuerEvader,
        GreedyEvader,
        Player
    }
    [SerializeField]
    private CharacterState state;
    public CharacterState State
    {
        get { return state; }
        set
        {
            gameObject.layer = StateLayerMap[value];
            if (agent != null)
            {
                agent.SteeringMovements = StateSteeringMap[value];
                agent.MaxSpeed = StateSpeedMap[value];
            }
            if (material != null)
            {
                material.color = StateColorMap[value];
            }
            if (state == CharacterState.Player && value == CharacterState.FrozenEvader)
            {
                GameManager.Lost();
            }
            else if (state == CharacterState.UnfrozenEvader && value != CharacterState.FrozenEvader)
            {
                GameManager.Save();
            }
            state = value;
            UpdateNow();
        }
    }

    public static Dictionary<CharacterState, Color> StateColorMap = new Dictionary<CharacterState, Color>
    {
        [CharacterState.Player] = Color.black,
        [CharacterState.RescuerEvader] = Color.green,
        [CharacterState.PassiveEvader] = Color.white,
        [CharacterState.ActiveEvader] = Color.yellow,
        [CharacterState.GreedyEvader] = Color.black,
        [CharacterState.FrozenEvader] = Color.cyan,
        [CharacterState.UnfrozenEvader] = Color.grey,
        [CharacterState.Chaser] = Color.red,
        [CharacterState.PatrolingChaser] = Color.red,
    };

    private Dictionary<CharacterState, List<SteeringMovement>> StateSteeringMap;
    private Dictionary<CharacterState, int> StateLayerMap;
    private Dictionary<CharacterState, float> StateSpeedMap;
    private SteeringAgent agent;
    private Material material;
    public void Awake()
    {
        MyNode = GetComponent<Node>();
        lastPathUpdate = PathUpdateInterval;
        frozenTime = 0;
        crowdTime = 0;
        if (State != CharacterState.Player)
        {
            agent = GetComponent<SteeringAgent>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            material = meshRenderer.materials[0];
        }
        StateSteeringMap = new Dictionary<CharacterState, List<SteeringMovement>>
        {
            [CharacterState.Player] = new List<SteeringMovement>() { },
            [CharacterState.RescuerEvader] = new List<SteeringMovement>()
            {
                GetComponent<Seek>(),
                GetComponent<ObstacleAvoid>(),
                GetComponent<ObstacleCollision>(),
                GetComponent<FaceTarget>(),
                GetComponent<CharacterAvoid>(),
            },
            [CharacterState.PassiveEvader] = new List<SteeringMovement>()
            {
                GetComponent<ObstacleAvoid>(),
                GetComponent<CharacterAvoid>(),
                GetComponent<ObstacleCollision>(),
            },
            [CharacterState.ActiveEvader] = new List<SteeringMovement>()
            {
                GetComponent<Seek>(),
                GetComponent<ObstacleAvoid>(),
                GetComponent<FaceTarget>(),
                GetComponent<CharacterAvoid>(),
                GetComponent<ObstacleCollision>(),
            },
            [CharacterState.GreedyEvader] = new List<SteeringMovement>()
            {
                GetComponent<Seek>(),
                GetComponent<ObstacleAvoid>(),
                GetComponent<FaceTarget>(),
                GetComponent<CharacterAvoid>(),
                GetComponent<ObstacleCollision>(),
            },
            [CharacterState.FrozenEvader] = new List<SteeringMovement>()
            {
                GetComponent<ObstacleAvoid>(),
                GetComponent<FrozenCharacterAvoid>(),
                GetComponent<ObstacleCollision>(),
            },
            [CharacterState.UnfrozenEvader] = new List<SteeringMovement>()
            {
                GetComponent<Seek>(),
                GetComponent<ObstacleAvoid>(),
                GetComponent<FaceTarget>(),
                GetComponent<CharacterAvoid>(),
                GetComponent<ObstacleCollision>(),
            },
            [CharacterState.Chaser] = new List<SteeringMovement>()
            {
                GetComponent<Seek>(),
                GetComponent<ObstacleAvoid>(),
                GetComponent<FaceTarget>(),
                GetComponent<ChaserCharacterAvoid>(),
                GetComponent<ObstacleCollision>(),
            },
            [CharacterState.PatrolingChaser] = new List<SteeringMovement>()
            {
                GetComponent<Seek>(),
                GetComponent<ObstacleAvoid>(),
                GetComponent<FaceTarget>(),
                GetComponent<ChaserCharacterAvoid>(),
                GetComponent<ObstacleCollision>(),
            },
        };
        StateLayerMap = new Dictionary<CharacterState, int>
        {
            [CharacterState.Player] = LayerMask.NameToLayer("Player"),
            [CharacterState.RescuerEvader] = LayerMask.NameToLayer("RescuerEvader"),
            [CharacterState.PassiveEvader] = LayerMask.NameToLayer("PassiveEvader"),
            [CharacterState.ActiveEvader] = LayerMask.NameToLayer("ActiveEvader"),
            [CharacterState.GreedyEvader] = LayerMask.NameToLayer("GreedyEvader"),
            [CharacterState.FrozenEvader] = LayerMask.NameToLayer("FrozenEvader"),
            [CharacterState.UnfrozenEvader] = LayerMask.NameToLayer("UnfrozenEvader"),
            [CharacterState.Chaser] = LayerMask.NameToLayer("Chaser"),
            [CharacterState.PatrolingChaser] = LayerMask.NameToLayer("PatrolingChaser")
        };
        StateSpeedMap = new Dictionary<CharacterState, float>
        {
            [CharacterState.Player] = EvaderSpped,
            [CharacterState.RescuerEvader] = EvaderSpped,
            [CharacterState.PassiveEvader] = EvaderSpped,
            [CharacterState.ActiveEvader] = EvaderSpped,
            [CharacterState.GreedyEvader] = EvaderSpped,
            [CharacterState.FrozenEvader] = EvaderSpped,
            [CharacterState.UnfrozenEvader] = EvaderSpped,
            [CharacterState.Chaser] = ChaserSpeed,
            [CharacterState.PatrolingChaser] = ChaserSpeed
        };
    }
    private void Start()
    {
        State = state;
    }
    private void FixedUpdate()
    {
        lastPathUpdate += Time.fixedDeltaTime;
        if (alertTimeRemain > 0) { alertTimeRemain -= Time.fixedDeltaTime; }
        else { Alerted = false; }
        HandleCollision();
        switch (State)
        {
            case CharacterState.Chaser:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    if (StateChange()) { break; }
                    agent.CurrentPath = ChaserPathAssign.GetPath(MyNode, Target);
                }
                break;
            case CharacterState.PatrolingChaser:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    if (StateChange()) { break; }
                    agent.CurrentPath = PatrolingChaserPathAssign.GetPath(MyNode, Target);
                }
                break;
            case CharacterState.ActiveEvader:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    if (StateChange()) { break; }
                    agent.CurrentPath = ActiveFleePathAssign.GetPath(Target, MyNode);
                    if (agent.CurrentPath.Count > 0)
                    {
                        Debug.DrawLine(transform.position, agent.CurrentPath[0].transform.position, Color.red, 1);
                    }
                }
                break;
            case CharacterState.GreedyEvader:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    if (StateChange()) { break; }
                    agent.CurrentPath = GreedyEvaderPathAssign.GetPath(MyNode, Target);
                }
                break;
            case CharacterState.PassiveEvader:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    if (StateChange()) { break; }
                    agent.CurrentPath = PassiveEvaderPathAssign.GetPath(MyNode);
                }
                break;
            case CharacterState.FrozenEvader:
                frozenTime += Time.fixedDeltaTime;
                if (frozenTime > FrozenTime)
                {
                    frozenTime = 0;
                    State = CharacterState.Chaser;
                }
                else
                {
                    if (lastPathUpdate >= PathUpdateInterval)
                    {
                        lastPathUpdate = 0;
                        if (StateChange()) { break; }
                        agent.CurrentPath = FrozenEvaderPathAssign.GetPath(MyNode);
                    }
                }
                break;
            case CharacterState.UnfrozenEvader:
                crowdTime += Time.fixedDeltaTime;
                if (crowdTime > CrowdTime)
                {
                    crowdTime = 0;
                    State = CharacterState.PassiveEvader;
                }
                else
                {
                    if (lastPathUpdate >= PathUpdateInterval)
                    {
                        lastPathUpdate = 0;
                        if (StateChange()) { break; }
                        agent.CurrentPath = UnfrozenEvaderPathAssign.GetPath(MyNode, Target);
                    }
                }
                break;
            case CharacterState.RescuerEvader:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    if (StateChange()) { break; }
                    agent.CurrentPath = RescuerEvaderPathAssign.GetPath(MyNode, Target);
                }
                break;
            case CharacterState.Player:
                break;
        }
    }
    private void HandleCollision()
    {
        int layermask = 0;
        //Debug.Log("Handle Collision");
        switch (State)
        {
            case CharacterState.Chaser:
            case CharacterState.PatrolingChaser:
                layermask += 1 << StateLayerMap[CharacterState.ActiveEvader];
                layermask += 1 << StateLayerMap[CharacterState.GreedyEvader];
                layermask += 1 << StateLayerMap[CharacterState.PassiveEvader];
                layermask += 1 << StateLayerMap[CharacterState.RescuerEvader];
                layermask += 1 << StateLayerMap[CharacterState.UnfrozenEvader];
                layermask += 1 << StateLayerMap[CharacterState.Player];

                foreach (Collider collider in Physics.OverlapSphere(transform.position, Radius, layermask))
                {
                    Character opponent = collider.GetComponent<Character>();
                    if (opponent == null) { continue; }
                    opponent.State = CharacterState.FrozenEvader;
                }
                break;
            case CharacterState.ActiveEvader:
            case CharacterState.GreedyEvader:
            case CharacterState.PassiveEvader:
            case CharacterState.RescuerEvader:
            case CharacterState.Player:
            case CharacterState.UnfrozenEvader:
                layermask = 1 << StateLayerMap[CharacterState.FrozenEvader];
                foreach (Collider collider in Physics.OverlapSphere(transform.position, Radius, layermask))
                {
                    Character opponent = collider.GetComponent<Character>();
                    if (opponent == null) { continue; }
                    opponent.State = CharacterState.UnfrozenEvader;
                }
                break;
            case CharacterState.FrozenEvader:
                break;
        }
    }
    private void Alert()
    {
        int layermask = 0;
        layermask += 1 << StateLayerMap[CharacterState.PatrolingChaser];
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 20, layermask))
        {
            if (collider.gameObject == gameObject) { continue; }
            collider.GetComponent<Character>().BeingAlert(Target);
        }
    }
    private void BeingAlert(Node target)
    {
        Target = target;
        Alerted = true;
        State = CharacterState.Chaser;
        alertTimeRemain = 5;
    }
    private bool StateChange()
    {
        switch (State)
        {
            case CharacterState.Chaser:
                if (Alerted) { return false; }
                Node temp = ChaserTargetAssign.GetTarget(MyNode);
                if (temp == null)
                {
                    chaserAllowance--;
                    if (chaserAllowance < 0)
                    {
                        Target = temp;
                        State = CharacterState.PatrolingChaser;
                        GetComponent<WayPoint>().Reset();
                        return true;
                    }
                }
                else { Target = temp; chaserAllowance = 1; Alert(); }
                return false;
            case CharacterState.ActiveEvader:
            case CharacterState.GreedyEvader:
            case CharacterState.PassiveEvader:
            case CharacterState.RescuerEvader:
                Target = ActiveEvaderTargetAssign.GetTarget(MyNode);
                if (Target != null)
                {
                    if (State == CharacterState.ActiveEvader) { return false; }
                    State = CharacterState.ActiveEvader;
                    return true;
                }
                //Debug.Log("Active Evader Target not Found");
                Target = RescuerEvaderTargetAssign.GetTarget(MyNode);
                if (Target != null)
                {
                    if (State == CharacterState.RescuerEvader) { return false; }
                    State = CharacterState.RescuerEvader;
                    return true;
                }
                Target = GreedyEvaderTargetAssign.GetTarget(MyNode);
                if (Target != null)
                {
                    if (State == CharacterState.GreedyEvader) { return false; }
                    State = CharacterState.GreedyEvader;
                    return true;
                }
                //Debug.Log("Rescuer Evader Target not Found");
                Target = PassiveEvaderTargetAssign.GetTarget(MyNode);
                if (State == CharacterState.PassiveEvader) { return false; }
                State = CharacterState.PassiveEvader;
                return true;
            case CharacterState.FrozenEvader:
                Target = FrozenEvaderTargetAssign.GetTarget(MyNode);
                return false;
            case CharacterState.UnfrozenEvader:
                Target = UnfrozenEvaderTargetAssign.GetTarget(MyNode);
                return false;
            case CharacterState.Player:
                return false;
            case CharacterState.PatrolingChaser:
                Target = ChaserTargetAssign.GetTarget(MyNode);
                if (Target == null)
                {
                    Target = PatrolingChaserTargetAssign.GetTarget(MyNode);
                    return false;
                }
                State = CharacterState.Chaser;
                chaserAllowance = 1;
                Alert();
                return true;
            default:
                return false;
        }
    }
    public void UpdateNow()
    {
        lastPathUpdate = PathUpdateInterval;
    }
}
