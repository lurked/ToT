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
    debug02,
    menuItem01,
    menuItem02,
    menuItem03
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
    Wood,
    Production,
    Energy
}



public enum UpgradeType
{
    Gold,
    Food,
    Wood,
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
    ToggleLog
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
    tooltip
}
