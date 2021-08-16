using System;


namespace Test
{
    public interface ITimeRemaining
    {
        Action Method { get; }
        bool IsRepeating { get; }
        float Time { get; set; }
        float CurrentTime { get; set; }
    }
}
