using UnityEngine;

[CreateAssetMenu(fileName = "DamageInfo", menuName = "Scriptable Objects/DamageInfo")]
public class DamageInfo : ScriptableObject
{
    public enum DamageType {Melee, Bullet, Explosion, Tentacle }
    public DamageType damageType;
    public enum DamageDirection {Back, Front, Right, Left, Down}
    public DamageDirection damageDirection;

}
