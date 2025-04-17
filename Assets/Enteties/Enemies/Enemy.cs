using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Action<Enemy> OnDeath;
}
