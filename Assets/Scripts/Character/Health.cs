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

    public Animator _animator;

    private void Awake()
    {
        HealthPoint = MaxHealth;
    }

    public void ModifyHealth(int HealthChange)
    {
        int previousHealthPoint = HealthPoint;
        HealthPoint = Mathf.Clamp(HealthPoint + HealthChange, 0, MaxHealth);
        Debug.Log(HealthPoint);
        if (previousHealthPoint != HealthPoint)
        {
            // Warn subscribers that health changed
            foreach (IHealthSubscriber subscriber in Subscribers)
            {
                subscriber.NotifyHealthChange(this, HealthPoint);
            }

            if (HealthPoint <= 0)
            {
                _animator.SetTrigger("Dead");
                // Warn subscribers that health is depleted
                foreach (IHealthSubscriber subscriber in Subscribers)
                {
                    subscriber.NotifyHealthDepleted(this);
                }
            }
        }
    }
}
