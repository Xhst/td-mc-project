﻿using Godot;


namespace TowerDefenseMC.Singletons
{
    public class Game : Node
    {
        public int CurrentLevel = -1;

        public static bool EnemyIsDead { set; get; }
    }
}