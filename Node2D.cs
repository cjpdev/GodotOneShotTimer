using Godot;
using System;

public class Node2D : Godot.Node2D
{
    public override void _Ready()
    {
        OneShotTimer oneShotTimer1 = new OneShotTimer(this, 2f, nameof(TestFunc));
        OneShotTimer oneShotTimer2 = new OneShotTimer(this, 4f, nameof(TestFunc), false);
        OneShotTimer oneShotTimer3 = new OneShotTimer(this, 6f, nameof(TestFunc3), true, "Yes" , 100);
        OneShotTimer oneShotTimer4 = new OneShotTimer(this, 2f, nameof(TestFunc2), true, "Hello world");

        try {
            OneShotTimer oneShotTimerError = new OneShotTimer(this, 2f, "TestFunc5", true, "Hello world");
        } catch(Exception ex) {
            GD.Print(ex);
        }
    }

    public void TestFunc()
    {
        GD.Print("TestFunc: oneShotTimer fired.");
    }

    public void TestFunc2(string msg)
    {
        GD.Print("TestFunc2: oneShotTimer fired, msg=" + msg);
    }

    public void TestFunc3(string msg, int value)
    {
        GD.Print("TestFunc2: oneShotTimer1 fired, msg=" + msg + ", value=" + value.ToString());
    }
}