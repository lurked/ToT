public enum ClientState
{
    Idle,
    Splashscreen,
    Loading,
    MainMenu,
    Game,
    GameMenu,
    GameOver
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
    debug02,
    menuItem01,
    menuItem02,
    menuItem03
}

public enum GameType
{
    Survival,
    Exploration,
    Defense,
    Load
}
public enum ThingType
{
    Container,
    Creature,
    Decor,
    NPC,
    Player,
    Resource,
    Sign,
    Tower,
    Guard,
    Special
}

public enum StatType
{
    HP,
    MoveSpeed,
    UsedMove,
}

public enum LevelType
{
    Static,
    Dynamic,
    LoadStatic,
    LoadDynamic
}

public enum ElementType
{
    Air,
    Fire,
    Earth,
    Water,
    Neutral
}

public enum ResourceType
{
    Gold,
    Food,
    Production,
    Energy,
    Empty
}

public enum UpgradeType
{
    Gold,
    Food,
    Production,
    Energy,
    Minions
}

public enum Cardinals
{
    North,
    East,
    South,
    West
}

public enum UIType
{
    Basic,
    BasicInvis,
    BasicOpaque
}

public enum UIItemType
{
    TextFix,
    TextFloating,
    ImageFix,
    ImageFloating,
    TextImage,
    ImageText
}

public enum UIAction
{
    None,
    Info,
    BuyTile,
    UpgradeTile,
    EndTurn,
    TileSheet,
    ToggleLog,
    NewGame,
    LoadGame,
    Options,
    Exit,
    BuildTile,
    NewGameMenu,
    NewGameSurvival,
    NewGameExploration,
    NewGameDefense
}

public enum UIItemsFlow
{
    Vertical,
    Horizontal,
    Flow
}

public enum UITemplate
{
    toolbar01,
    playerSheet,
    realmSheet,
    turn01,
    log,
    income,
    tileSheet,
    tileExpendNorth,
    tileExpendEast,
    tileExpendWest,
    tileExpendSouth,
    tooltip,
    mainNew,
    mainLoad,
    mainOptions,
    mainExit,
    mainLoadSaves,
    improveUI,
    buildUI,
    selectionUI,
    mainNewMenu
}

public enum BuildingType
{
    Tower_Normal,
    Tower_Red,
    Tower_Blue,
    Tower_Yellow,
    Spawn_Enemy_Basic,
    Spawn_Enemy_Special
}

public enum TowerAttack
{
    Normal,
    Burn,
    AoE,
    Bounce
}
