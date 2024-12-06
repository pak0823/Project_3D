public enum eSCENE
{
    eSCENE_TITLE,   //0
    eSCENE_LOGIN,   //1
    eSCENE_LOBBY,   //2
    eSCENE_LOADING, //3
    eSCENE_BATTLE,  //4
    eSECNE_END      //5
}

public enum eMONSTERSTATE
{
    CREATE,
    IDLE,
    MOVE,
    CHASE,
    ATTACK,
    HIT,
    DIE
}

public enum eMONSTERTYPE
{
    Slime,
    Turtle,
    Cactus,
    Mushroom,
    Golem
}

public enum ePLAYERSTATE
{
    IDLE,
    MOVE,
    ATTACK,
    HIT
}