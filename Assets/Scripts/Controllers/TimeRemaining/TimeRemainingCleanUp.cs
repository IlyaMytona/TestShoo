using System.Collections.Generic;
using Test.Interface;


namespace Test.Controllers.TimeRemaining
{
    public sealed class TimeRemainingCleanUp : ICleanUp
    {      
        #region Fields
                   
        private readonly List<ITimeRemaining> _timeRemainings;
                   
        #endregion
           
                   
        #region ClassLifeCycles
           
        public TimeRemainingCleanUp()
        {
            _timeRemainings = TimeRemainingExtensions.TimeRemainings;
        }
                   
        #endregion
        
        
        #region ICleanupController
        
        public void Clean()
        {
            _timeRemainings.Clear();
        }

        #endregion
    }
}