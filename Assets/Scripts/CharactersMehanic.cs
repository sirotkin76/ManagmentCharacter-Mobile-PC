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
	private bool jump = false;
	private bool crouchWalk = false;
	private bool isRun = false;
	private bool isRoll = false;
	private float timeRoll = 0.3f;

	// Ссылки на компоненты
	private CharacterController ch_controller;
	private Animator ch_animator;
	private MobileController mContr;
	private MobileController mContrJoy2;

	float timer = 0.0f;

	void Start () {
		ch_controller = GetComponent<CharacterController>();
		ch_animator = GetComponent<Animator>();
		mContr = GameObject.FindGameObjectWithTag("Joystick").GetComponent<MobileController>();
		mContrJoy2 = GameObject.FindGameObjectWithTag("Joystick2").GetComponent<MobileController>();
	}
	
	void Update () {
		CharacterMove();
		CharacterMoveJoy2();
		GamingGravity();
	}

	// Метод перемещения персонажа - джойстик 1
	private void CharacterMove() {

		// Отслеживаем кувырок
		if (isRoll) {

			timer += Time.deltaTime;
			if (timer >= timeRoll) {
				
			} else {
				ch_controller.Move(transform.forward * 5f * Time.deltaTime);
				return;
			}
		}

		// Отключаем перемещение в прыжке
		if (ch_controller.isGrounded) {

			ch_animator.ResetTrigger("isJump");
			ch_animator.SetBool("isFalling", false);

			float crouchSpeed = 1f;
			float runSpeed = 1f;

			// Скорость в приседе or // Скорость в беге
			crouchSpeed = (crouchWalk) ? 3 : 1;
			runSpeed = (isRun && !crouchWalk) ? 2.5f : 1;

			// Перемещение по поверхности
			moveVector = Vector3.zero;
			moveVector.x = mContr.Horizontal() * (speedMove / crouchSpeed) * runSpeed;
			moveVector.z = mContr.Vertical() * (speedMove / crouchSpeed) * runSpeed;

			if (!crouchWalk) {
				ch_animator.SetBool("isCrouchWalk", false);

				// Анимация передвижения персонажа стоя
				if (moveVector.x != 0 || moveVector.z != 0) 
					ch_animator.SetBool("isWalk", true);
				else 
					ch_animator.SetBool("isWalk", false);

			} else {
				// Уменьшаем размеры персонажа когда он сидит
				ch_controller.center = new Vector3 ( 0f, 0.57f, 0f);
				ch_controller.height = 1.2f;

				ch_animator.SetBool("isWalk", false);

				// Анимация передвижения персонажа в присиде
				if (moveVector.x != 0 || moveVector.z != 0)
					ch_animator.SetBool("isCrouchWalk", true);
				else 
					ch_animator.SetBool("isCrouchWalk", false);
			}

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

	// Метод перемещения character - джойстик 2
	private void CharacterMoveJoy2() {
		// События по вертикали
		if (mContrJoy2.VerticalJoy2() >= 0.7f && ch_controller.isGrounded ) {
			// Прыжок
			jump = true;
		} else if (((Input.GetKey(KeyCode.LeftControl)) || (mContrJoy2.VerticalJoy2() <= -0.7f)) && ch_controller.isGrounded) {
			// Сесть
			ch_animator.SetBool("isCrouch", true);
			crouchWalk = true;
		} else  {
			// Дефолтные значения
			ch_animator.SetBool("isCrouch", false);
			ch_animator.SetBool("isCrouchWalk", false);
			crouchWalk = false;
		}
		
		// События по горизонтали Run
		if ( (moveVector.x != 0 || moveVector.z != 0) &&  ((Input.GetKey(KeyCode.LeftShift)) || (mContrJoy2.HorizontalJoy2() >= 0.5f)) && ch_controller.isGrounded) {
			ch_animator.SetBool("isRun", true);
			isRun = true;
		} else if ( ((Input.GetKey(KeyCode.LeftAlt)) || (mContrJoy2.HorizontalJoy2() <= -0.4f)) && ch_controller.isGrounded) {
			ch_animator.SetTrigger("isRoll");
			isRoll = true;
			timer = 0;
		} else {
			ch_animator.ResetTrigger("isRoll");
			ch_animator.SetBool("isRun", false);
			ch_animator.SetBool("isRun", false);
			isRun = false;
		}
	}

	// метод гравитации
	private void GamingGravity() {
		if (!ch_controller.isGrounded) 
			gravityForce -= 20 * Time.deltaTime;
		else 
			gravityForce = -1f;

		if ( jump || (Input.GetKeyDown(KeyCode.Space)) && ch_controller.isGrounded) {
			jump = false;
			gravityForce = jumpPower;
			ch_animator.SetTrigger("isJump");
		} 
		
		if (ch_controller.isGrounded  && !crouchWalk) {
			ch_controller.height = 1.8f;
			ch_controller.center = new Vector3(0f, 0.93f, 0f);
		} else if (!ch_controller.isGrounded && !crouchWalk){
			ch_controller.height = 1.57f;
			ch_controller.center = new Vector3(0f, 1.08f, 0f);
		}
	}
}
