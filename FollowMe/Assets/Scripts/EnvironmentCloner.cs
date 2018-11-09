using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentCloner : MonoBehaviour
{

	public Transform environmentPrototype;
	public float offset = 10f;
	public int count = 1;

	[SerializeField, HideInInspector]
	Transform[] environments = new Transform[0];

	void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}

		if (count != environments.Length)
		{
			for (int i = 0; i < environments.Length; i++)
			{
				if (environments[i] != null)
				{
					DestroyImmediate(environments[i].gameObject);
				}
			}

			var gridSize = Mathf.CeilToInt(Mathf.Sqrt(count));

			environments = new Transform[count];
			for (int i = 0; i < count; i++)
			{

				var pos = new Vector3(i % gridSize, 0, i / gridSize) * offset;
				environments[i] = Instantiate(environmentPrototype);
				environments[i].transform.SetParent(this.transform);
				environments[i].transform.localPosition = pos;
				environments[i].name = "Environment " + i;
			}
		}
	}
}