using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public static class ComboUtility
	{

		public static void AdjustAttackQueue()
		{

		}

		public static void AddQueueRange(List<float> AttackQueue,float[] queue,float attackDelay,float moveSpeed)
		{
			int index = 0;
			for(int i = 0 ; i < queue.Length ; i ++)
			{
				for(int j = index; j < AttackQueue.Count;j++)
				{
					if(queue[i] <= AttackQueue[j])
					{
						index = j;
						AttackQueue.Insert(j,queue[0]);
						break;
					}
				}
			}
		}

		public static void RemoveQueue()
		{

		}

	}
	
	[System.Serializable]
	public class AttackQueue
	{
		public float TimeDelay;
		public int ATK;
		public AudioClip ATKSound;
	}
}

