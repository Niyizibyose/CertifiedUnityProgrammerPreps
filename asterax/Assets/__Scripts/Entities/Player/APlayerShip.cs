using System;
using __Scripts.Asteroids;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Stats), typeof(SphereCollider))]
public class APlayerShip : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private Turret turret;
    [SerializeField] private float damageOnHit;

    public Stats Stats { get; private set; }

    private Mover _mover;
    private bool _enabled = true;
    private SphereCollider _sphereCollider;
    private MeshRenderer[] _children;

    private void Awake()
    {
        Stats = GetComponent<Stats>();
        _sphereCollider = GetComponent<SphereCollider>();
        _children = GetComponentsInChildren<MeshRenderer>();
        _mover = GetComponent<Mover>();
        inputController.OnMove += _mover.Move;
        inputController.OnRotate += turret.Rotate;
        inputController.OnFire += () =>
        {
            if (_enabled) turret.Fire();
        };
    }

    /// <summary>
    /// <para>Disables the ship by disabling the sphere collider and his children</para>
    /// </summary>
    public void Disable()
    {
        _sphereCollider.enabled = false;
        _enabled = false;
        SetChildrenActivation(false);
    }

    /// <summary>
    /// <para>Enables the ship by enabling the sphere collider, his children and resetting his health.</para>
    /// </summary>
    public void Enable()
    {
        _sphereCollider.enabled = true;
        Stats.ResetHealth();
        SetChildrenActivation(true);
        _enabled = true;
    }

    /// <summary>
    /// <para>If the Collision has the component Asteroid then it deals damage to it</para>
    /// <para>The ship health is reduced by zero</para>
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        var asteroid = other.gameObject.GetComponentInParent<Asteroid>();
        if (asteroid == null) return;
        asteroid.ReceiveDamage(damageOnHit, false);
        Stats.Health = 0;
    }
    
    /// <summary>
    /// <para>Activates or deactivates all the children with the component MeshRenderer</para>
    /// </summary>
    /// <param name="active"></param>
    private void SetChildrenActivation(bool active)
    {
        for (var i = 0; i < _children.Length; i++)
        {
            _children[i].gameObject.SetActive(active);
        }
    }
}