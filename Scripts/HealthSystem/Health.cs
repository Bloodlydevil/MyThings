using System;

namespace MyThings.HealthSystem
{

    /// <summary>
    /// An Object Which Should Be Used For Proper Health Management
    /// </summary>
    [Serializable]
    public class Health
    {
        /// <summary>
        /// The Max Health
        /// </summary>
        private float _MaxHealth;


        /// <summary>
        /// The Max Health
        /// </summary>
        public float MaxHealth { get { return _MaxHealth; } }


        /// <summary>
        /// The Current Health
        /// </summary>
        private float _CurrentHealth;


        /// <summary>
        /// The Current Health
        /// </summary>
        public float CurrentHealth { get { return _CurrentHealth; } }


        /// <summary>
        /// The HealthUI To Show The Effects Of The Health Reduction Or Increase
        /// </summary>
        private IHealthUI HealthUI;


        #region Constructors


        /// <summary>
        /// Health Require MaxHealth And Current Health Is Set To MaxHealth
        /// </summary>
        /// <param name="maxHealth">The Max Health</param>
        /// <param name="uI">The Health Ui </param>
        public Health(float maxHealth, IHealthUI uI = null)
        {
            _MaxHealth = maxHealth;
            _CurrentHealth = maxHealth;
            HealthUI = uI;
        }


        /// <summary>
        /// Health Require MaxHealth And Current Health
        /// </summary>
        /// <param name="maxHealth">The Max Health</param>
        /// <param name="currentHealth">The Current Health</param>
        /// <param name="uI">The Health UI</param>
        public Health(float maxHealth, float currentHealth, IHealthUI uI = null)
        {
            _MaxHealth = maxHealth;
            _CurrentHealth = currentHealth;
            HealthUI = uI;
        }


        #endregion


        #region Functions


        /// <summary>
        /// Function Is Used To Set HealthUI
        /// </summary>
        /// <param name="uI"></param>
        public void SetHealthUI(IHealthUI uI) => HealthUI = uI;


        /// <summary>
        /// Function Is Used To Get The The Health int The Form Of Per(between 0 And 1)
        /// </summary>
        /// <returns></returns>
        public float GetHealthPer() => Math.Clamp(_CurrentHealth / _MaxHealth, 0, 1);


        /// <summary>
        /// This Function Is Used To Change The Max Health And Health To New Value
        /// </summary>
        /// <param name="health">The Current Health</param>
        /// <param name="maxHealth">The Max Health</param>
        public void ReSetHealth(float health, float maxHealth)
        {
            _CurrentHealth = health;
            _MaxHealth = maxHealth;
        }


        /// <summary>
        /// This Function Is Used TO Set Up Max Health
        /// </summary>
        /// <param name="maxHealth">The Max Health</param>
        public void SetMaxHealth(float maxHealth) => _MaxHealth = maxHealth;


        /// <summary>
        /// This Function Is Used To Set The Current Health
        /// </summary>
        /// <param name="currentHealth">The Current Health</param>
        /// <returns>If THe Current Health was Able To Change</returns>
        public bool SetCurrentHealth(float currentHealth)
        {
            if (currentHealth > _MaxHealth)
                return false;
            _CurrentHealth = currentHealth;
            return true;
        }


        /// <summary>
        /// Function Is Used To Reduce Health
        /// </summary>
        /// <param name="Damage">The Damage Dealt</param>
        /// <returns>If The Object Is Dead</returns>
        public bool DecreaseHealth(float Damage)
        {
            _CurrentHealth -= Damage;
            HealthUI.DecreaseHealth(GetHealthPer());
            if (_CurrentHealth <= 0)
            {
                _CurrentHealth = 0;
                HealthUI.EndAllAnimation();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Function Is Used To Increase Health
        /// </summary>
        /// <param name="Heal">The Increase In Health</param>
        /// <returns>If The Object Is At Max Health </returns>
        public bool IncreaseHealth(float Heal)
        {
            if (_CurrentHealth == _MaxHealth)
                return true;
            else
            {
                _CurrentHealth += Heal;
                HealthUI.IncreaseHealth(GetHealthPer());
                if (_CurrentHealth > _MaxHealth)
                {
                    _CurrentHealth = _MaxHealth;
                    return true;
                }
                return false;
            }
        }

        public void EndUIAnimation() => HealthUI.EndAllAnimation();
        #endregion
    }
}