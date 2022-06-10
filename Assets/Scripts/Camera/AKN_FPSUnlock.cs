using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKN_FPSUnlock : MonoBehaviour
{	
	[SerializeField] private int androidFPS = 60;

	private void Start()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
			Application.targetFrameRate = androidFPS;
		#endif
	}
}
