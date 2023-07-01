using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp List", menuName = "Object/PowerUp List")]
public class PowerUpsListSO : ScriptableObject
{
    public List<PowerUpSO> powerUpSOList;
}
