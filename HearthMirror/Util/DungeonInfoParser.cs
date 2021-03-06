﻿using System;
using System.Collections.Generic;
using HearthMirror.Enums;
using HearthMirror.Objects;
using static HearthMirror.Enums.GameSaveKeySubkeyId;

namespace HearthMirror.Util
{
	internal class DungeonInfoParser : DungeonInfo
	{
		private readonly dynamic _map;

		public DungeonInfoParser(int cardSet, dynamic map)
		{
			CardSet = cardSet;
			_map = map;
			GetValue(DUNGEON_CRAWL_BOSSES_DEFEATED, out _bossesDefeated);
			GetValue(DUNGEON_CRAWL_DECK_CARD_LIST, out _dbfIds);
			GetValue(DUNGEON_CRAWL_DECK_CLASS, out _heroCardClass);
			GetValue(DUNGEON_CRAWL_PLAYER_PASSIVE_BUFF_LIST, out _passiveBuffs);
			GetValue(DUNGEON_CRAWL_NEXT_BOSS_FIGHT, out _nextBossDbfId);
			GetValue(DUNGEON_CRAWL_LOOT_OPTION_A, out _lootA);
			GetValue(DUNGEON_CRAWL_LOOT_OPTION_B, out _lootB);
			GetValue(DUNGEON_CRAWL_LOOT_OPTION_C, out _lootC);
			GetValue(DUNGEON_CRAWL_TREASURE_OPTION, out _treasure);
			GetValue(DUNGEON_CRAWL_LOOT_HISTORY, out _lootHistory);
			GetValue(DUNGEON_CRAWL_IS_RUN_ACTIVE, out int active);
			_runActive = active > 0;
			GetValue(DUNGEON_CRAWL_BOSS_LOST_TO, out _bossesLostTo);
			GetValue(DUNGEON_CRAWL_PLAYER_CHOSEN_LOOT, out _playerChosenLoot);
			GetValue(DUNGEON_CRAWL_PLAYER_CHOSEN_TREASURE, out _playerChosenTreasure);
			GetValue(DUNGEON_CRAWL_NEXT_BOSS_HEALTH, out _nextBossHealth);
			GetValue(DUNGEON_CRAWL_HERO_HEALTH, out _heroHealth);
			GetValue(DUNGEON_CRAWL_CARDS_ADDED_TO_DECK_MAP, out _cardsAddedToDeck);
			GetValue(DUNGEON_CRAWL_SELECTED_HERO_CLASS, out _heroClass);
			GetValue(DUNGEON_CRAWL_PLAYER_SELECTED_HERO_POWER, out _selectedHeroPower);
			GetValue(DUNGEON_CRAWL_PLAYER_CHOSEN_SHRINE, out _playerChosenShrine);
			GetValue(DUNGEON_CRAWL_SHRINE_OPTIONS, out _shrines);
			GetValue(DUNGEON_CRAWL_PLAYER_SELECTED_LOADOUT_TREASURE_ID, out _selectedLoadoutTreasureDbId);

			// Dalaran
			GetValue(DUNGEON_CRAWL_PLAYER_SELECTED_DECK, out _selectedDeckId);
		}

		private bool GetValue(GameSaveKeySubkeyId key, out int value)
		{
			value = 0;
			List<int> values;
			if(GetValue(key, out values))
			{
				value = values[0];
				return true;
			}
			return false;
		}

		private bool GetValue<T>(GameSaveKeySubkeyId key, out List<T> values)
		{
			values = null;
			try
			{
				var subIndex = Reflection.GetKeyIndex(_map, (int)key);
				if(subIndex == -1)
					return false;
				var list = _map["valueSlots"][subIndex]?[GetTypeKey(typeof(T))];
				if(list == null)
					return false;
				var size = (int)list["_size"];
				if(size <= 0)
					return false;
				values = new List<T>();
				var items = list["_items"];
				for(var i = 0; i < size; i++)
					values.Add((T)items[i]);
				return true;
			}
			catch(Exception e)
			{
				return false;
			}
		}

		private string GetTypeKey(Type t)
		{
			if(t == typeof(int))
				return "_IntValue";
			if(t == typeof(double))
				return "_FloatValue";
			if(t == typeof(string))
				return "_StringValue";
			return null;
		}

	}
}
