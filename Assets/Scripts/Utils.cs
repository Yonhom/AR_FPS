
using UnityEngine;

public class Utils {

	public static void AssignLayerMaskToObjectAndChildren(GameObject _objParent,  int _maskValue) {
		if (_objParent == null)
			Debug.LogError ("the object to assign layer mask to can not be null!");

		_objParent.layer = _maskValue;

		// in game objects, the transform component is hierarchial. we can use it to iterate child object indirectly
		foreach (Transform _child in _objParent.transform) {
			AssignLayerMaskToObjectAndChildren (_child.gameObject, _maskValue);
		}
	}
}
