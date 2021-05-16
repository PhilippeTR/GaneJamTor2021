using UnityEngine;

public interface IHealthSubscriber
{
    void NotifyHealthChange(Health healthScript, int health);
    void NotifyHealthDepleted(Health healthScript);
}

public class Health : MonoSubscribable<IHealthSubscriber>
{
    [Header("Health")]
    [SerializeField]
    private int _maxHealth = 1;
    public int MaxHealth
    {
        get { return _maxHealth; }
        private set { _maxHealth = value; }
    }

    public int HealthPoint { get; protected set; }

    private void Awake()
    {
        HealthPoint = MaxHealth;
    }

    public void ModifyHealth(int damage)
    {
        int previousHealthPoint = HealthPoint;
        Mathf.Clamp(HealthPoint += damage, 0, MaxHealth);

        if (previousHealthPoint != HealthPoint)
        {
            // Warn subscribers that health changed
            foreach (IHealthSubscriber subscriber in Subscribers)
            {
                subscriber.NotifyHealthChange(this, HealthPoint);
            }

            if (HealthPoint <= 0)
            {
                // Warn subscribers that health is depleted
                foreach (IHealthSubscriber subscriber in Subscribers)
                {
                    subscriber.NotifyHealthDepleted(this);
                }
            }
        }
    }
}
