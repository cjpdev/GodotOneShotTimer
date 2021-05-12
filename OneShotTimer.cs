using Godot;
using System;

public class OneShotTimer : Godot.Object
{
    private Timer t = null;
    private Godot.Object target = null;
    private string method = null;
    private Node node = null;

    private OneShotTimer()
    {  
    }

    public OneShotTimer(Godot.Node node, float sec, string method)
    {
        if(node != null && method != "")
        {
            this.target = (Godot.Object)node;

            if(target.HasMethod(method))
            {
                this.node = node;
                this.method = method;

                t = new Timer();
                t.WaitTime = sec;
                t.OneShot = true;
                
                // Handle the timeout internally.
                t.Connect("timeout", this, nameof(Done));

                if(node != null)
                {
                    node.AddChild(t);
                    t.Start();
                }
    
                return;
            }
        }

        string err = (method=="")?"no method name":method;
        GD.Print("target, does not have method " + err);
    }

    public void Done()
    {
        if(t != null)
        {
            // Will not fire again, but disconnet anyway
            // for extra safety.
            if(t.IsConnected("timeout", this, nameof(Done)))
            {
                t.Disconnect("timeout", this, nameof(Done));
            }
        }

        // Free up the child
        if(node != null)
        {
            node.RemoveChild(t);
        
            // Call the real function
            if(target.HasMethod(method))
            {
                target.CallDeferred(method);
            }
        }

        // Free timer now, help GC.
        if(t != null)
        {
            t.Dispose();
            t = null;
        }
                    
        // Help GC free these, as they are no longer needed.
        this.node = null;
        this.target = null;
        this.method = null;
    }
}
