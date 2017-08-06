﻿using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Tasks.PlayerTasks;
using System;
using Xunit;
using Generic = SabberStoneCore.Actions.Generic;

namespace SabberStoneCoreTest.Cloning
{

	public class CloneStressTest
	{
		[Fact]
		public void CloneSameSame()
		{
			var rnd = new Random();
			var classes = new[]
			{
				ECardClass.DRUID, ECardClass.HUNTER, ECardClass.MAGE, ECardClass.PALADIN, ECardClass.PRIEST,
				ECardClass.ROGUE, ECardClass.SHAMAN, ECardClass.WARLOCK, ECardClass.WARRIOR
			};
			var flag = true;
			for (var i = 0; i < 10 && flag; i++)
			{
				var game = new Game(new GameConfig
				{
					StartPlayerIdx = 1,
					FormatType = EFormatType.FT_STANDARD,
					Player1CardClass = classes[rnd.Next(classes.Length)],
					Player2CardClass = classes[rnd.Next(classes.Length)],
					FillDecks = true
				});
				game.StartGame();

				while (game.State != EState.COMPLETE)
				{
					var options = game.CurrentPlayer.Options();
					var option = options[rnd.Next(options.Count)];
					game.Process(option);
					var cloneGame = game.Clone();
					var str1 = game.Hash();
					var str2 = cloneGame.Hash();
					flag &= str1.Equals(str2);
					if (!flag)
					{
						break;
					}
				}
			}
			Assert.Equal(true, flag);
		}

		[Fact]
		public void CloneSameState()
		{
			var game = new Game(new GameConfig
			{
				StartPlayerIdx = 1,
				FormatType = EFormatType.FT_STANDARD,
				Player1CardClass = ECardClass.SHAMAN,
				Player2CardClass = ECardClass.SHAMAN,
				FillDecks = true
			});
			game.Player1.BaseMana = 10;
			game.Player2.BaseMana = 10;
			game.StartGame();
			var clone = game.Clone();
			Assert.Equal(game.Hash(), clone.Hash());

			var spell1 = Generic.DrawCard(game.CurrentPlayer, Cards.FromName("Lightning Bolt"));
			var minion1 = Generic.DrawCard(game.CurrentPlayer, Cards.FromName("Stonetusk Boar"));
			var spell2 = Generic.DrawCard(game.CurrentPlayer, Cards.FromName("Lightning Bolt"));

			var cSpell1 = Generic.DrawCard(clone.CurrentPlayer, Cards.FromName("Lightning Bolt"));
			var cSpell2 = Generic.DrawCard(clone.CurrentPlayer, Cards.FromName("Lightning Bolt"));
			var cMinion1 = Generic.DrawCard(clone.CurrentPlayer, Cards.FromName("Stonetusk Boar"));

			game.Process(PlayCardTask.SpellTarget(game.CurrentPlayer, spell1, game.CurrentOpponent.Hero));
			game.Process(PlayCardTask.SpellTarget(game.CurrentPlayer, spell2, game.CurrentOpponent.Hero));
			game.Process(PlayCardTask.Any(game.CurrentPlayer, minion1));

			clone.Process(PlayCardTask.Any(clone.CurrentPlayer, cMinion1));
			clone.Process(PlayCardTask.SpellTarget(clone.CurrentPlayer, cSpell2, clone.CurrentOpponent.Hero));
			clone.Process(PlayCardTask.SpellTarget(clone.CurrentPlayer, cSpell1, clone.CurrentOpponent.Hero));

			var ignored = new EGameTag[] { EGameTag.LAST_CARD_PLAYED, EGameTag.ENTITY_ID };

			Assert.Equal(game.Hash(ignored), clone.Hash(ignored));
		}

		[Fact]
		public void CloneEndTurnTask()
		{
			var game = new Game(new GameConfig
			{
				StartPlayerIdx = 1,
				FormatType = EFormatType.FT_STANDARD,
				Player1CardClass = ECardClass.SHAMAN,
				Player2CardClass = ECardClass.SHAMAN,
				FillDecks = true
			});
			game.Player1.BaseMana = 10;
			game.Player2.BaseMana = 10;
			game.StartGame();
			var clone = game.Clone();
			Assert.Equal(game.Hash(), clone.Hash());

			clone.Process(EndTurnTask.Any(clone.CurrentPlayer));

			Assert.NotEqual(game.CurrentPlayer.Name, clone.CurrentPlayer.Name);

			clone.Process(EndTurnTask.Any(clone.CurrentPlayer));

			Assert.Equal(game.CurrentPlayer.Name, clone.CurrentPlayer.Name);
		}
	}
}