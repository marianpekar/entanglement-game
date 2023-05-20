using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	[SerializeField]
	private float damping = 5f;

	[SerializeField]
	Vector3 offset;

	void Start()
	{
		offset = transform.position - target.position;
	}

	void LateUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, target.position + offset, damping * Time.deltaTime);
	}
}