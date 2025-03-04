public class Enemy : Target
{
    public Target Target { get; private set; }

    public void SetTarget(Target target) => Target = target;
}