namespace FeatureToggle;

public interface IFeatureToggleService
{
    Task<bool> IsFeatureEnabled(string flagName);
}