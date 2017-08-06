﻿using SabberStoneCore.Collections;
using SabberStoneCore.Enums;
using SabberStoneCore.Model.Entities;
using System;
using System.Text;

namespace SabberStoneCore.Model
{
	/// <summary>
	/// Embodies a choice the specified controller must make.
	/// This class also holds the made choice.
	/// </summary>
	public sealed class EntityChoice
	{
		/// <summary>The player who made the choice.</summary>
		public readonly Controller Controller;

		/// <summary>Gets or sets the type of the choice.</summary>
		/// <value><see cref="EChoiceType"/></value>
		public EChoiceType ChoiceType { get; set; } = EChoiceType.INVALID;

		/// <summary>Gets or sets the action linked to the made choice.</summary>
		/// <value><see cref="EChoiceAction"/></value>
		public EChoiceAction ChoiceAction { get; set; } = EChoiceAction.INVALID;

		/// <summary>Gets or sets the set of possible choices, entity ID's.</summary>
		/// <value>The set of entity ID's, in a choice set it's not possible to have two options at the same time.</value>
		public IReadOnlyOrderedSet<int> Choices { get; set; }

		/// <summary>Gets or sets the entity ID that triggered the choice to be made.</summary>
		/// <value>The Entity ID.</value>
		public int SourceId { get; set; }

		/// <summary>Gets or sets the SET of chosen entities.</summary>
		/// <value>List of Entity ID's.</value>
		public IReadOnlyOrderedSet<int> TargetIds { get; set; }

		/// <summary>Initializes a new instance of the <see cref="EntityChoice"/> class.</summary>
		/// <param name="controller">The controller.</param>
		public EntityChoice(Controller controller)
		{
			Controller = controller;
		}

		/// <summary>Copies information from the provided object into this one.</summary>
		/// <param name="choice">The choice object to copy from.</param>
		public void Stamp(EntityChoice choice)
		{
			ChoiceType = choice.ChoiceType;
			ChoiceAction = choice.ChoiceAction;
			Choices = LightWeightOrderedSet<int>.Build(choice.Choices);
			SourceId = choice.SourceId;
			TargetIds = LightWeightOrderedSet<int>.Build(choice.TargetIds);
		}

		/// <summary>Builds a string with all information contained by this object.</summary>
		/// <returns>A string representing this object.</returns>
		public string FullPrint()
		{
			var str = new StringBuilder();
			str.Append($"{Controller.Name}[ChoiceType:{ChoiceType}][ChoiceAction:{ChoiceAction}][");
			str.Append(String.Join(",", Choices));
			return str.ToString();
		}
	}
}