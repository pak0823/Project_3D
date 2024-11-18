public enum eSCENE
{
    eSCENE_TITLE,   //0
    eSCENE_LOGIN,   //1
    eSCENE_LOBBY,   //2
    eSCENE_LOADING, //3
    eSCENE_BATTLE,  //4
    eSECNE_END      //5
}

public enum eAI
{
    eAI_NONE,   //생성 되자마자 움직인다면 안 만들어도 된다.
    eAI_CREATE,
    eAI_SEARCH,
    eAI_MOVE,
    eAI_RESET
}

public enum eCHARACTER
{
    eCHARACTER_PLAYER,
    eCHARACTER_MONSTER,
    eCHARACTER_END
}