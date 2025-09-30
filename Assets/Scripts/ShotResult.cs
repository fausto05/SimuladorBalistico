using UnityEngine;

public class ShotResult
{
    public float angle;
    public float force;
    public float mass;
    public bool hitTarget;
    public float distance;
    public int pieces;

    public ShotResult(float angle, float force, float mass, bool hitTarget, float distance, int pieces)
    {
        this.angle = angle;
        this.force = force;
        this.mass = mass;
        this.hitTarget = hitTarget;
        this.distance = distance;
        this.pieces = pieces;
    }
}
