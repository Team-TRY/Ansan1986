using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneSystems.RoadNavigator
{
	[CreateAssetMenu(fileName = "LevelMapSettings", menuName = "Insane Systems/Road GPS Navigator/Level Map Settings")]
	public class LevelMapSettings : ScriptableObject
	{
		public Vector2 baseOffset;
		public Vector2 realMapScale;
		public Sprite mapSprite;
	}
}