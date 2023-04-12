using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Units.Components
{
    [RequireComponent(typeof(Rigidbody))]
    internal sealed class ChargeComponent : NetworkBehaviour
    {
        public bool IsDashing { get; private set; } = false;
        public UnityEvent OnChargeHitEvent; 

        [SerializeField] private float chargeDistance;
        [SerializeField] private float chargeTime;
        [SerializeField] private float chargeCooldown;

        private float dashTimer;
        private Rigidbody rb;
        private List<TakeDamageComponent> takeDamages = new List<TakeDamageComponent>();

        #region Mono
        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }
        private void OnEnable() {
            InputActionsSystem.Instance.OnChargePerformedEvent += OnChargePerformedHandler;
        }

        private void OnDisable() {
            InputActionsSystem.Instance.OnChargePerformedEvent -= OnChargePerformedHandler;
        }
        private void Update() {
            if (!isLocalPlayer) return;
            if (IsDashing) {
                dashTimer += Time.deltaTime;
                if (dashTimer >= chargeTime || (rb.position - transform.position).magnitude > chargeDistance) 
                    IsDashing = false;
                
            }
        }
        private void FixedUpdate() {
            if (!isLocalPlayer) return;
            if (IsDashing) {
                var dashSpeed = chargeDistance / chargeTime;
                rb.MovePosition(rb.position + rb.transform.forward * dashSpeed * Time.fixedDeltaTime);
            }
        }
        private void OnCollisionEnter(Collision collision) {
            if (IsDashing) {
                var damageble = collision.gameObject.GetComponent<TakeDamageComponent>();
                if (damageble) {
                    if (!takeDamages.Contains(damageble)) {
                        if (!damageble.IsImmortal) {
                            CmdOnChargeCollision(damageble);
                            takeDamages.Add(damageble);
                        }
                    }
                }
            }
        }
        #endregion

        [Command]
        private void CmdOnChargeCollision(TakeDamageComponent damageble) {
            damageble.TakeDamage();
            OnChargeHitEvent?.Invoke();
        }

        private void StartCharge() {
            if (!isLocalPlayer) return;
            if (IsDashing) return;
            StartCoroutine(ChargeRecoveryRoutine(chargeCooldown));
        }

        private IEnumerator ChargeRecoveryRoutine(float delay) {
            IsDashing = true;
            dashTimer = 0;

            yield return new WaitForSeconds(delay);
            IsDashing = false;
            takeDamages.Clear();
        }

        private void OnChargePerformedHandler() {
            StartCharge();
        }



    }
}
