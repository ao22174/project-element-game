public class UIManager : CoreComponent
{
    public HeartDisplay heartDisplay;
    public void SetHearts(float currentHealth, float maxHealth)
    {
        heartDisplay.SetHearts(currentHealth, maxHealth);
    }
}