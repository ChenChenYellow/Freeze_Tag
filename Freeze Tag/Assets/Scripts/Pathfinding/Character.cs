using UnityEngine;
using System.Collections.Generic;
public class Character : MonoBehaviour
{
    public Character Target;
    private Room currentRoom;
    public Room CurrentRoom
    {
        get { return currentRoom; }
        set
        {
            if (value == CurrentRoom) { return; }

            Node.Neighbors.Clear();
            if (currentRoom != null)
            {
                currentRoom.CharacterLeave(this);
            }

            currentRoom = value;
            Node.Neighbors.AddRange(currentRoom.Nodes);
            currentRoom.CharacterEnter(this);
        }
    }
    public Node Node;
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
        crowdTime;

    public enum CharacterState
    {
        Chaser,
        PatrolingChaser,
        ActiveEvader,
        PassiveEvader,
        FrozenEvader,
        UnfrozenEvader,
        RescuerEvader,
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
        Node = GetComponent<Node>();
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
        HandleCollision();
        switch (State)
        {
            case CharacterState.Chaser:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    // Assign chaser a target
                    if (StateChange()) { break; }
                    // Assign chaser a path
                    agent.CurrentPath = ChaserPathAssign.GetPath(this, Target);
                }
                break;
            case CharacterState.PatrolingChaser:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    // Assign patroling chaser a target
                    if (StateChange()) { break; }
                    // Assign patroling chaser a path
                    agent.CurrentPath = PatrolingChaserPathAssign.GetPath(this);
                }
                break;
            case CharacterState.ActiveEvader:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    //Debug.Log("Active Evader Update");
                    // Assign active evader a target
                    if (StateChange()) { break; }
                    // Assign active evader a path
                    agent.CurrentPath = ActiveFleePathAssign.GetPath(Target, this);
                    if (agent.CurrentPath.Count > 0)
                    {
                        Debug.DrawLine(transform.position, agent.CurrentPath[0].transform.position, Color.red, 1);
                    }
                }
                break;
            case CharacterState.PassiveEvader:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    lastPathUpdate = 0;
                    //Debug.Log("Passive Evader Update");
                    // Assign passive evader a target
                    if (StateChange()) { break; }
                    // Assign passive evader a path
                    agent.CurrentPath = PassiveEvaderPathAssign.GetPath(this);
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
                        //Debug.Log("Frozen Evader Update");
                        lastPathUpdate = 0;

                        // Assign frozen evader a target
                        if (StateChange()) { break; }
                        // Assign frozen evader a path
                        agent.CurrentPath = FrozenEvaderPathAssign.GetPath(this);
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
                        //SDebug.Log("Unfrozen Evader Update");
                        lastPathUpdate = 0;

                        // Assign unfrozen evader a target
                        if (StateChange()) { break; }
                        // Assign unfrozen evader a path
                        agent.CurrentPath = UnfrozenEvaderPathAssign.GetPath(this, Target);
                    }
                }
                break;
            case CharacterState.RescuerEvader:
                if (lastPathUpdate >= PathUpdateInterval)
                {
                    //Debug.Log("Rescuer Evader Update");
                    lastPathUpdate = 0;

                    // Assign rescuer evader a target
                    if (StateChange()) { break; }
                    // Assign rescuer evader a path
                    agent.CurrentPath = RescuerEvaderPathAssign.GetPath(this, Target);
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
    private bool StateChange()
    {
        switch (State)
        {
            case CharacterState.Chaser:
                Target = ChaserTargetAssign.GetTarget(this);
                if (Target == null)
                {
                    //Debug.Log("Chaser Target not Found");
                    State = CharacterState.PatrolingChaser;
                    return true;
                }
                return false;
            case CharacterState.ActiveEvader:
            case CharacterState.PassiveEvader:
            case CharacterState.RescuerEvader:
                Target = ActiveEvaderTargetAssign.GetTarget(this);
                if (Target != null)
                {
                    if (State == CharacterState.ActiveEvader) { return false; }
                    State = CharacterState.ActiveEvader;
                    return true;
                }
                //Debug.Log("Active Evader Target not Found");
                Target = RescuerEvaderTargetAssign.GetTarget(this);
                if (Target != null)
                {
                    if (State == CharacterState.RescuerEvader) { return false; }
                    State = CharacterState.RescuerEvader;
                    return true;
                }
                //Debug.Log("Rescuer Evader Target not Found");
                Target = PassiveEvaderTargetAssign.GetTarget(this);
                if (State == CharacterState.PassiveEvader) { return false; }
                State = CharacterState.PassiveEvader;
                return true;
            case CharacterState.FrozenEvader:
                Target = FrozenEvaderTargetAssign.GetTarget(this);
                return false;
            case CharacterState.UnfrozenEvader:
                Target = UnfrozenEvaderTargetAssign.GetTarget(this);
                return false;
            case CharacterState.Player:
                return false;
            case CharacterState.PatrolingChaser:
                Target = ChaserTargetAssign.GetTarget(this);
                if (Target == null) { return false; }
                State = CharacterState.Chaser;
                return false;
            default:
                return false;
        }
    }
    public void UpdateNow()
    {
        lastPathUpdate = PathUpdateInterval;
    }
}
