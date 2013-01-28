using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ComplexElementLevel
{

	public bool G = false;
	public bool GR= false;
	public bool GL= false;
	public bool GD= false;
	public bool GF= false;
	public bool spi= false;
	public bool mid= false;
	public bool low= false;
	public bool vc= false;
	public bool hc= false;
	public bool vid= false;
	
	public ComplexElementLevel(List<LevelElement> _LevelElements)
	{
		foreach(LevelElement lLevelElement in _LevelElements)
		{
			switch (lLevelElement.mLevelElementType)
			{
				case LevelElementType.G : G = true; break;
				case LevelElementType.GR: GR = true; break;
				case LevelElementType.GL : GL = true; break;
				case LevelElementType.GD : GD = true; break;
				case LevelElementType.GF : GF = true; break;
				case LevelElementType.spi: spi = true; break;
				case LevelElementType.mid : mid = true; break;
				case LevelElementType.low : low = true; break;
				case LevelElementType.vc : vc = true; break;
				case LevelElementType.hc : hc = true; break;
			}
		}
		

		
		if( !G && !GR && !GL && !GD && !GF && !spi && mid && !low && !vc && !hc)
		{
			vid = true;
		}
		
		
	}
}
