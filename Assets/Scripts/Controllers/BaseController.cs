using Test.GameServices;
using Test.Interface;
using Test.UI;


namespace Test.Controllers
{
    public abstract class BaseController
    {
        #region Private Data

        protected UiInterface UiInterface;

        #endregion


        #region Class LifeCycle

        protected BaseController()
        {
            UiInterface = new UiInterface();
            Services.Instance.LevelService.UiInterface = UiInterface;
        }

        #endregion


        #region Properties

        public bool IsActive { get; private set; }

        #endregion


        #region Methods

        public virtual void On()
        {
            On(null);
        }

        public virtual void On(params IModel[] obj)
        {
            IsActive = true;
        }

        public virtual void Off()
        {
            IsActive = false;
        }

        public void Switch()
        {
            if (IsActive)
            {
                Off();
            }
            else
            {
                On();
            }
        }

        #endregion
    }
}
