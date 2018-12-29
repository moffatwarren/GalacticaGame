using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] float moveSpeed = 12f;
	[SerializeField] float padding = 0.5f;
	[SerializeField] GameObject laserPrefab;
	[SerializeField] float laserVelocity = 15f;
	[SerializeField] float laserShotDelay = 0.5f;

	float xMin;
	float xMax;
	float yMin;
	float yMax;

	Coroutine firingCoroutine;

	// Use this for initialization
	void Start()
	{
		SetUpMoveBoundaries();
	}

	// Update is called once per frame
	void Update()
	{
		MovePlayer();
		Fire();
	}

	private void Fire()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			firingCoroutine = StartCoroutine(FireContinously());
		}
		if (Input.GetButtonUp("Fire1"))
		{
			StopCoroutine(firingCoroutine);
		}
	}

	IEnumerator FireContinously()
	{
		while (true)
		{
			GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
			laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserVelocity);
			yield return new WaitForSeconds(laserShotDelay);
		}
	}

	private void SetUpMoveBoundaries()
	{
		Camera gameCamera = Camera.main;
		xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
		xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
		yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
		yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
	}

	private void MovePlayer()
	{
		var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime;
		var deltaY = Input.GetAxis("Vertical") * Time.deltaTime;
		var newXpos = Mathf.Clamp(transform.position.x + deltaX  * moveSpeed, xMin, xMax);
		var newYpos = Mathf.Clamp(transform.position.y + deltaY * moveSpeed, yMin, yMax);
		transform.position = new Vector2(newXpos, newYpos);
	}
}
