using UnityEngine;

public struct AttackData
{
    public int damage;
    public GameObject effect;
    public string animation;

    public AttackData(int damage, GameObject effect, string animation)
    {
        this.damage = damage;
        this.effect = effect;
        this.animation = animation;
    }
}
