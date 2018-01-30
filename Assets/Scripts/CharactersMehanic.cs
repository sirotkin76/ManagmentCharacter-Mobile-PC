using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersMehanic : MonoBehaviour {
	// Основные параметры
	public float speedMove; // Скорость персонажа
	public float jumpPower; // Сила прыжка

	// Параметры геймлея для персонажа
	private float gravityForce; // Гравитация персонажа
	private Vector3 moveVector; // Направление движения персонажа

	// Ссылки на компоненты
	private CharacterController ch_controller;
	private Animator ch_animator;
	private MobileController mContr;

	void Start () {
		ch_controller = GetComponent<CharacterController>();
		ch_animator = GetComponent<Animator>();
		mContr = GameObject.FindGameObjectWithTag("Joystick").GetComponent<MobileController>();
	}
	
	void Update () {
		CharacterMove();
		GamingGravity();
	}

	// Метод перемещения персонажа
	private void CharacterMove() {
		// Отключаем перемещение в прыжке
		if (ch_controller.isGrounded) {

			ch_animator.ResetTrigger("isJump");
			ch_animator.SetBool("isFalling", false);

			// Перемещение по поверхности
			moveVector = Vector3.zero;
			moveVector.x = mContr.Horizontal() * speedMove;
			moveVector.z = mContr.Vertical() * speedMove;

			// Анимация передвижения персонажа
			if (moveVector.x != 0 || moveVector.z != 0) ch_animator.SetBool("isWalk", true);
			else ch_animator.SetBool("isWalk", false);

			if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0) {
				Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, speedMove, 0.0f);
				transform.rotation = Quaternion.LookRotation(direct);
			}
		} else {
			if (gravityForce <- 3f) ch_animator.SetBool ("isFalling", true);
		}

		moveVector.y = gravityForce;
		ch_controller.Move(moveVector * Time.deltaTime); // Метод передвижения по направлению
	}

	// метод гравитации
	private void GamingGravity() {
		if (!ch_controller.isGrounded) gravityForce -= 20 * Time.deltaTime;
		else gravityForce = -1f;

		if (Input.GetKeyDown(KeyCode.Space) && ch_controller.isGrounded) {
			gravityForce = jumpPower;
			ch_animator.SetTrigger("isJump");
		}


	}
}
