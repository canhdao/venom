using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Config {
	public readonly static int[]   NUMBER_ENEMIES	= new int[]		{10, 12, 14, 16, 18, 20};

	public readonly static float[] RATE_SOLDIER_1	= new float[]	{25, 20, 15, 15, 15, 15};
	public readonly static float[] RATE_SOLDIER_2	= new float[]	{25, 20, 15, 15, 15, 15};
	public readonly static float[] RATE_SOLDIER_3	= new float[]	{25, 20, 15, 15, 15, 15};
	public readonly static float[] RATE_SOLDIER_4	= new float[]	{25, 20, 15, 15, 15, 15};
	public readonly static float[] RATE_DOG_SOLDIER	= new float[]	{ 0,  0, 20, 10, 10, 10};
	public readonly static float[] RATE_TRUCK		= new float[]	{ 0,  0,  0, 10, 10, 10};
	public readonly static float[] RATE_GIRL		= new float[]	{ 0, 20, 20, 20, 20, 20};

	public const float RATE_PATTERN_2 = 20;
	public const float RATE_PATTERN_3 = 20;
	
	public const float WAVE_GAP_TIME = 3;
}
