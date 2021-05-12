using Godot;
using System;

public class Node2D : Godot.Node2D
{
    public override void _Ready()
    {
        OneShotTimer one1 = new OneShotTimer(this, 2, nameof(TestFunc));
        OneShotTimer one2 = new OneShotTimer(this, 4, nameof(TestFunc));
        OneShotTimer one3 = new OneShotTimer(this, 6, nameof(TestFunc), false);
        OneShotTimer oneA1 = new OneShotTimer(this, 2, nameof(TestFunc2));
    }

    public void TestFunc()
    {
        GD.Print("OneShot Timer fired....");
    }

    public void TestFunc2()
    {
        GD.Print("OneShot Timer fired....");
    }

}
