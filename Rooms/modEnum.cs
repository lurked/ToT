public enum ClientState
{
    Idle,
    Splashscreen,
    Loading,
    MainMenu,
    Game,
    GameMenu
}

public enum MenuType
{
    Text,
    Image,
    TextAndImage
}

public enum Font
{
    logo01,
    debug01,
    menuItem01,
    menuItem02
}

public enum ThingType
{
    Container,
    Creature,
    Decor,
    NPC,
    Player,
    Resource,
    Sign
}

public enum LevelType
{
    Static,
    Dynamic,
    LoadStatic,
    LoadDynamic
}