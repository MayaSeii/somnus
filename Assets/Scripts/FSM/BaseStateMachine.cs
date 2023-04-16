using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    [field: SerializeField] public BaseState InitialState { get; set; }
    public BaseState CurrentState { get; set; }

    private Dictionary<Type, Component> _components;
    
    private void Awake()
    {
        CurrentState = InitialState;
        _components = new Dictionary<Type, Component>();
    }

    private void Start()
    {
        CurrentState.Enter(this);
    }

    private void Update()
    {
        CurrentState.Execute(this);
    }

    public new T GetComponent<T>() where T : Component
    {
        if(_components.ContainsKey(typeof(T)))
            return _components[typeof(T)] as T;

        var component = base.GetComponent<T>();
        if (component) _components.Add(typeof(T), component);
        
        return component;
    }
}
