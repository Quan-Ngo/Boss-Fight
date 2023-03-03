using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLockListener : MonoBehaviour
{
	public void endAnimationLock()
	{
		PlayerManager.Instance.endAnimationLock();
	}
}
