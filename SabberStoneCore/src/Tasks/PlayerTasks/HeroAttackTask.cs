﻿using SabberStoneCore.Enums;
using SabberStoneCore.Actions;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.PlayerTasks
{
    public class HeroAttackTask : PlayerTask
    {
        public static HeroAttackTask Any(Controller controller, IEntity target)
        {
            return new HeroAttackTask(controller, target);
        }
        private HeroAttackTask(Controller controller, IEntity target)
        {
            PlayerTaskType = EPlayerTaskType.HERO_ATTACK;
            Game = controller.Game;
            Controller = controller;
            Target = target;
        }
        public override ETaskState Process()
        {
            var success = Generic.AttackBlock.Invoke(Controller, Controller.Hero, Target as ICharacter);
            Controller.Game.NextStep = EStep.MAIN_CLEANUP;
            return ETaskState.COMPLETE;
        }

        public override string FullPrint()
        {
            return $"HeroAttackTask => [{Controller.Name}] {Controller.Hero} attacks {Target}";
        }
    }
}