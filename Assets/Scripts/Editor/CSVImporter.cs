using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NinjaChompek
{
	public class CSVImporter : Editor
	{
		[MenuItem("CatKings/Import Data from CSV")]
		static void ImportData()
		{
			ImportCards();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void ImportCards()
		{
			string relativePath = "/_CSV/Cards.csv";
			string filePath = Application.dataPath + relativePath;

			if (!File.Exists(filePath))
			{
				Debug.LogError("Card CSV not found.");
				return;
			}

			List<Card> cards = new List<Card>();

			string[] lines = File.ReadAllLines(filePath);

			foreach (string line in lines.Skip(2)) // Skip header rows
			{
				// Parse the line
				string[] fields = SplitCsvLine(line);

				// If the card row has no name, skip it
				if (string.IsNullOrEmpty(fields[0]))
				{
					continue;
				}

				string cardName = fields[0];
				string cardDescription = fields[3];
				string cardCharacterName = fields[4];

				string sequenceName = "";
				int sequenceOrder = 0;
				int authorityLeft = 0;
				int moneyLeft = 0;
				int armyLeft = 0;
				int peopleLeft = 0;
				int sequenceNextLeft = 0;
				int authorityRight = 0;
				int moneyRight = 0;
				int armyRight = 0;
				int peopleRight = 0;
				int sequenceNextRight = 0;

				if (!string.IsNullOrEmpty(fields[1]) && !string.IsNullOrEmpty(fields[2]))
				{
					sequenceName = fields[1];
					sequenceOrder = int.Parse(fields[2]);
				}

				string buttonLeftText = fields[5];

				if (!string.IsNullOrEmpty(fields[6]))
				{
					authorityLeft = int.Parse(fields[6]);
				}

				if (!string.IsNullOrEmpty(fields[7]))
				{
					moneyLeft = int.Parse(fields[7]);
				}

				if (!string.IsNullOrEmpty(fields[8]))
				{
					armyLeft = int.Parse(fields[8]);
				}

				if (!string.IsNullOrEmpty(fields[9]))
				{
					peopleLeft = int.Parse(fields[9]);
				}

				if (!string.IsNullOrEmpty(fields[10]))
				{
					sequenceNextLeft = int.Parse(fields[10]);
				}

				string buttonRightText = fields[11];

				if (!string.IsNullOrEmpty(fields[12]))
				{
					authorityRight = int.Parse(fields[12]);
				}

				if (!string.IsNullOrEmpty(fields[13]))
				{
					moneyRight = int.Parse(fields[13]);
				}

				if (!string.IsNullOrEmpty(fields[14]))
				{
					armyRight = int.Parse(fields[14]);
				}

				if (!string.IsNullOrEmpty(fields[15]))
				{
					peopleRight = int.Parse(fields[15]);
				}

				if (!string.IsNullOrEmpty(fields[16]))
				{
					sequenceNextRight = int.Parse(fields[16]);
				}

				Card newCard = CreateInstance<Card>();

				newCard.Name = cardName;
				newCard.Description = cardDescription;
				newCard.SequenceName = sequenceName;
				newCard.SequenceOrder = sequenceOrder;
				newCard.ButtonLeftText = buttonLeftText;
				newCard.AuthorityLeft = authorityLeft;
				newCard.MoneyLeft = moneyLeft;
				newCard.ArmyLeft = armyLeft;
				newCard.PeopleLeft = peopleLeft;
				newCard.SequenceNextLeft = sequenceNextLeft;
				newCard.ButtonRightText = buttonRightText;
				newCard.AuthorityRight = authorityRight;
				newCard.MoneyRight = moneyRight;
				newCard.ArmyRight = armyRight;
				newCard.PeopleRight = peopleRight;
				newCard.SequenceNextRight = sequenceNextRight;

				if (!string.IsNullOrEmpty(cardCharacterName))
				{
					Character cardCharacter = AssetDatabase.LoadAssetAtPath<Character>($"Assets/ScriptableObjects/Characters/{cardCharacterName}.asset");
					newCard.Character = cardCharacter;
				}

				SaveOrUpdateAsset<Card>(newCard, newCard.Name);
			}
		}

		private static string[] SplitCsvLine(string line)
		{
			var result = new List<string>();
			bool inQuotes = false;
			string currentField = "";

			foreach (char c in line)
			{
				if (c == '"') // If we find a quote, toggle inQuotes
				{
					inQuotes = !inQuotes;
				}
				else if (c == ',' && !inQuotes) // If we find a comma and we're not inside quotes, we've reached the end of the field
				{
					result.Add(currentField);
					currentField = "";
				}
				else
				{
					currentField += c;
				}
			}
			result.Add(currentField);

			return result.ToArray();
		}

		private static void SaveOrUpdateAsset<T>(T newAsset, string assetName) where T : Object
		{
			string assetType = typeof(T).Name;
			string path = $"Assets/ScriptableObjects/Cards/{assetName}.asset";

			T existingAsset = AssetDatabase.LoadAssetAtPath<T>(path);

			if (existingAsset == null)
			{
				// If the asset doesn't exist, create it
				AssetDatabase.CreateAsset(newAsset, path);
			}
			else
			{
				// If the asset exists, update it
				UpdateAsset(newAsset, existingAsset);
				EditorUtility.SetDirty(existingAsset);
			}
		}

		private static void UpdateAsset<T>(T source, T target)
		{
			foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
			{
				var value = field.GetValue(source);
				field.SetValue(target, value);
			}
		}
	}
}