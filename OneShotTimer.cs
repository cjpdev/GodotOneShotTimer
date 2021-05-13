/**
*
	Copyright (c) 2021 Chay Palton
	Permission is hereby granted, free of charge, to any person obtaining
	a copy of this software and associated documentation files (the "Software"),
	to deal in the Software without restriction, including without limitation
	the rights to use, copy, modify, merge, publish, distribute, sublicense,
	and/or sell copies of the Software, and to permit persons to whom the Software
	is furnished to do so, subject to the following conditions:
	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.
	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
	OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
	IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
	CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
	TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
	OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Godot;
using System;

public class OneShotTimer : Godot.Object
{
    private Timer t = null;
    private Godot.Object target = null;
    private string method = null;
    private Node node = null;

    private bool callDeferred = false;

    private Godot.Collections.Array binds = null;

    private OneShotTimer()
    {  
    }

    /// <summary>
    /// Create instance of a one shot timer, once the time has fired it is removed
    /// from the Node tree.
	/// </summary>
	/// <param name="node">Godot.Node</param>
    /// <param name="sec">Wait time in Seconds.</param>
    /// <param name="method">Target to call on the node object.</param>
    /// <param name="callDeferred">Use deferred call, Default = true</param>
    public OneShotTimer(Godot.Node node, float sec, string method, bool callDeferred = true)
    {
        CreateOneShotTimer( node,  sec, method,callDeferred);
    }

	/// <summary>
    /// Create instance of a one shot timer, once the time has fired it is removed
    /// from the Node tree.
    /// <example>
    /// See example code
    /// <code>
    ///  OneShotTimer oneA1 = new OneShotTimer(this, 2f, nameof(ExFunc),
    ///         new Godot.Collections.Array() { "Hello world.."});
    ///
    ///  public void ExFunc(string msg)
    ///  {
    ///    GD.Print("ExFunc: OneShot Timer fired...." + msg);
    ///  }
    /// </code>
    /// </example>
	/// </summary>
	/// <param name="node">Godot.Node</param>
    /// <param name="sec">Wait time in Seconds.</param>
    /// <param name="method">Target to call on the node object.</param>
    /// <param name="binds">Params to pass when calling the targer method.</param> 
    /// <param name="callDeferred">Use deferred call, Default = true</param>
    public OneShotTimer(Godot.Node node, float sec, string method, Godot.Collections.Array binds, bool callDeferred = true)
    {
        this.binds = binds;
  
        CreateOneShotTimer(node, sec, method,callDeferred);
    }

    private void CreateOneShotTimer(Godot.Node node, float sec, string method, bool callDeferred = true)
    {
        this.callDeferred = callDeferred;

        if(method == "")
        {
            GD.Print("OneShotTimer: Error no method name ");
            return;
        }

        if(node != null)
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
            
            } else {

                GD.Print("OneShotTimer: Error target has no method named " + method);
                return;
            }
        }

        GD.Print("OneShotTimer: was not created.");
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
                // If we have params use them in the call.
                if(binds != null)
                {
                    // Useful to be able choose the call type.
                    if(callDeferred)
                    {
                        target.CallDeferred(method, binds);
                    } else {
                        target.Call(method, binds);
                    }

                } else {

                    // Useful to be able choose the call type.
                    if(callDeferred)
                    {
                        target.CallDeferred(method);
                    } else {
                        target.Call(method);
                    }
                }
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