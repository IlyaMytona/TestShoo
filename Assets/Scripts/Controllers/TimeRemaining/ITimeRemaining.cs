using System;


namespace Test.Controllers.TimeRemainings
{
    public interface ITimeRemaining
    {
        Action Method { get; }
        bool IsRepeating { get; }
        float Time { get; set; }
        float CurrentTime { get; set; }
    }
}
