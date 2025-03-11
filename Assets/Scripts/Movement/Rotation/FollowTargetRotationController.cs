public class FollowTargetRotationController : RotationController
{
    private Target _target;
    
    public void SetTarget(Target target) => _target = target;

    protected override void Update()
    {
        SetNewTarget(_target.transform.position);

        base.Update();
    } 
}
