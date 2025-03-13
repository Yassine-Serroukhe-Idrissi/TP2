using UnityEngine;
public struct UnoptimisedStruct1
{ 
    //12 byte
    public Vector3 position;
    //4 byte
    public float velocity;
    public float size;
    public int baseHP;
    public int currentHp;
    public int nbAllies;
    //1 byte
    public bool isVisible;
    public bool canJump;
    public bool isStanding;
    public byte colorAlpha;
    //8 byte
    public double range;
    public UnoptimizedStruct2[] otherFriends;
    public float[] distancesFromObjectives;
    public UnoptimizedStruct2 mainFriend;

    public UnoptimisedStruct1(float velocity, bool canJump, int baseHP, int nbAllies, Vector3 position, int currentHp, float[] distancesFromObjectives, byte colorAlpha, double range, UnoptimizedStruct2 mainFriend, bool isVisible, UnoptimizedStruct2[] otherFriends, bool isStanding, float size)
    {
        this.velocity = velocity;
        this.canJump = canJump;
        this.baseHP = baseHP;
        this.nbAllies = nbAllies;
        this.position = position;
        this.currentHp = currentHp;
        this.distancesFromObjectives = distancesFromObjectives;
        this.colorAlpha = colorAlpha;
        this.range = range;
        this.mainFriend = mainFriend;
        this.isVisible = isVisible;
        this.otherFriends = otherFriends;
        this.isStanding = isStanding;
        this.size = size;
    }
}

public enum FriendState
{
    isFolowing,
    isSearching,
    isPatrolling,
    isGuarding,
    isJumping,
}

public struct UnoptimizedStruct2 
{
    //12 byte
    public Vector3 position;
    //8 byte
    public double maxSight;
    //4 byte
    public FriendState currentState;
    public float height;
    public float velocity;
    public float acceleration;
    public float maxVelocity;
    public int currentObjective;
    //1 byte
    public bool isAlive;
    public bool canJump;

    public UnoptimizedStruct2(bool isAlive, float height, FriendState currentState, float velocity, int currentObjective, double maxSight, bool canJump, float acceleration, Vector3 position, float maxVelocity)
    {
        this.isAlive = isAlive;
        this.height = height;
        this.currentState = currentState;
        this.velocity = velocity;
        this.currentObjective = currentObjective;
        this.maxSight = maxSight;
        this.canJump = canJump;
        this.acceleration = acceleration;
        this.position = position;
        this.maxVelocity = maxVelocity;
    }
}
