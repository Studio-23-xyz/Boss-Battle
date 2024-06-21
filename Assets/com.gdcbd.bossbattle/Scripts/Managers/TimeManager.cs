using com.gdcbd.bossbattle.utility;
using UnityEngine;

namespace com.gdcbd.bossbattle
{
    public class TimeManager : PersistentMonoSingleton<TimeManager>
    {
        private float _time;

        protected override void Initialize()
        {
            _time = 0;
        }

        private void Update()
        {
            _time += Time.deltaTime;
        }

        public float TimeCount()
        {
            return _time;
        }
    }
}
