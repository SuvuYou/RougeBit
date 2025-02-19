[System.Serializable]
struct EntityMovementStats
{
    public float MaxMovementSpeed;
    public float AccelerationSpeed;
    public float Drag;
    public float DragThreshold;

    public EntityMovementStats(float maxMovementSpeed, float accelerationSpeed, float drag, float dragThreshold)
    {
        MaxMovementSpeed = maxMovementSpeed;
        AccelerationSpeed = accelerationSpeed;
        Drag = drag;
        DragThreshold = dragThreshold;
    }
}