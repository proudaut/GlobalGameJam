using UnityEngine;
using System.Collections;

public enum LevelElementType
{
	G = 1,
	GR,
	GL,
	GD,
	GF,
	spi,
	mid,
	low,
	vc,
	hc,
}

public class LevelElement
{
	public LevelElementType mLevelElementType;
	public int mX;
	public int mY;
	public LevelElement(IDictionary _Dic)
	{
		mLevelElementType = (LevelElementType)int.Parse(_Dic["ElementType"].ToString());
		mX = int.Parse(_Dic["x"].ToString());
		mY = int.Parse(_Dic["y"].ToString());
	}
}
