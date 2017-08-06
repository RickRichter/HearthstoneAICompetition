﻿using SabberStoneCore.Enums;
using SabberStoneCore.Actions;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.PlayerTasks
{
    public class HeroPowerTask : PlayerTask
    {
        public static HeroPowerTask Any(Controller controller, IEntity target = null)
        {
            return new HeroPowerTask(controller, target);
        }

        private HeroPowerTask(Controller controller, IEntity target)
        {
            PlayerTaskType = EPlayerTaskType.HERO_POWER;
            Game = controller.Game;
            Controller = controller;
            Target = target;
        }

        public override ETaskState Process()
        {
            var success = Generic.HeroPower(Controller, Target as ICharacter);
            Controller.Game.NextStep = EStep.MAIN_CLEANUP;
            return ETaskState.COMPLETE;
        }

        public override string FullPrint()
        {
            return $"HeroPowerTask => [{Controller.Name}] using {Controller.Hero.Power}" +
                   $"{(Target != null ? $" attack {Target}" : "")}";
        }
    }
}