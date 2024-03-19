using Godot;
using System;

// Author: Ironee

namespace com.IronicEntertainment.Scripts.Flow
{
    public class GameConfig : Node
    {
        public override void _Ready()
        {
            ResLocation = ProjectSettings.GlobalizePath("res://");
        }

        public static string ResLocation;
    }
}