using Godot;
using System;

public class Node2D : Godot.Node2D
{
    public override void _Ready()
    {
        OneShotTimer one1 = new OneShotTimer(this, 2f, nameof(TestFunc));
        OneShotTimer one2 = new OneShotTimer(this, 4f, nameof(TestFunc), false);
        OneShotTimer one3 = new OneShotTimer(this, 6f, nameof(TestFunc));
        OneShotTimer oneA1 = new OneShotTimer(this, 2f, nameof(TestFunc2), new object[] { "Hello world.."});
    }

    public void TestFunc()
    {
        GD.Print("TestFunc: OneShot Timer fired....");
    }

    public void TestFunc2(string msg)
    {
        GD.Print("TestFunc2: OneShot Timer  fired...." + msg);
    }

}
