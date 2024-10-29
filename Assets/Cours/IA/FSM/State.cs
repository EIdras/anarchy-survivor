
using UnityEngine;

public class State<T>
{
    // The name for the state.
    public string Name { get; set; }

    // The ID of the state.
    public T ID { get; private set; }

    public State(T id)
    {
        ID = id;
    }
    public State(T id, string name) : this(id)
    {
        Name = name;
    }
   
    virtual public void Enter()
    {
        Debug.Log("Entering " + ID.ToString());
    }

    virtual public void Exit()
    {
       
    }
    virtual public void Update()
    {
      
    }

    virtual public void FixedUpdate()
    {
        
    }
}
