using UnityEngine;
using UnityEngine.Serialization;

namespace com.gdcbd.bossbattle.player
{
    [CreateAssetMenu(fileName = "Health", menuName = "BossBattle/Player Health")]
    public class PlayerHealth : ScriptableObject
    {
        [Tooltip("Player Health"), Range(0f, 100f)]
        public float CurrentHealth = 100f;
        public int Life = 3;
        public bool IsDead => Life == 0;
        public float HealTime = 2f;
        
        private float _maxHealth = 100f;
        private float _lastDamageTime = 0f;

        public void TakeDamage(float damage)
        {
            if (IsDead)
                return;
            CurrentHealth -= damage;
            _lastDamageTime = TimeManager.Instance.TimeCount();
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Life--;
                if (Life > 0)
                {
                    Respawn();
                }
            }
        }

        public void Heal(float healAmount)
        {
            if (IsDead || !CanHeal())
                return;
            
            CurrentHealth = Mathf.Min(CurrentHealth + healAmount, _maxHealth);

        }

        private bool CanHeal()
        {
            return TimeManager.Instance.TimeCount() >= _lastDamageTime + HealTime;
        }

        private void Respawn()
        {
            if (IsDead)
                return;

            CurrentHealth = _maxHealth;
        }

        public void ResetHealth()
        {
            CurrentHealth = _maxHealth;
            Life = 3;
        }
    }

}