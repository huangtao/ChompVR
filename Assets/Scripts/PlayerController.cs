public class PlayerController : Singleton<PlayerController>
{
    public bool HasPowerBall
    {
        get; set;
    }

    public bool IsDisplayingTutorialMessage {
        get; set;
    }
}
