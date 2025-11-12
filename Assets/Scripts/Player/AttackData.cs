using UnityEngine;

public struct AttackData
{
    public int damage;
    public GameObject effect;
    public string animation;

    public AudioClip sfx;

    public AttackData(int damage, GameObject effect, string animation, AudioClip sfx)
    {
        this.damage = damage;
        this.effect = effect;
        this.animation = animation;
        this.sfx = sfx;
    }
}
