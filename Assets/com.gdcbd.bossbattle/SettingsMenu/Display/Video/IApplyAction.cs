using com.gdcbd.bossbattle;


namespace Studio23.SS2
{
    public interface IApplyAction
    {
        public void SubscribeEvent()
        {
            InputManager.Instance.OnHoldSubmitActionCompleted += Save;
            InputManager.Instance.OnHoldResetActionCompleted += Reset;
        }

        public void UnSubscribeEvent()
        {
            InputManager.Instance.OnHoldSubmitActionCompleted -= Save;
            InputManager.Instance.OnHoldResetActionCompleted -= Reset;
        }

        public void Save();
        public void Reset();
    }
}
