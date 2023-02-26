using System;
using LSB.Classes.Enemies;
using LSB.Components.Audio;
using LSB.Components.Player;
using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Enemies {
    public class Goblin : MonoBehaviour {
        [Header("Stats")] 
        [SerializeField] private Stats BaseStats;
        [SerializeField] private Stats CurrentStats;
        [Header("Drop")] 
        [SerializeField] private GameObject Potion;
        [SerializeField] private GameObject Coin;
        
        private Enemy _enemy;
        private MeleeAttack _attack;
        private Chase _movement;
        private float _damageTimer;

        private void OnEnable() {
            _movement = new Chase(transform, GetComponent<Rigidbody2D>(), CurrentStats.Speed);
            _enemy = new Enemy(_movement, _attack, transform, GetComponentInChildren<Animator>(), GetComponentInChildren<SpriteRenderer>(), 
                gameObject, CurrentStats.MaxHp, CurrentStats.AttackCooldown, Potion, Coin);

            _attack = new MeleeAttack(transform, 0.2f, _enemy.GetAnimation());
            _enemy.OnEnemyDie += (o => { if (_enemy.MakeDeadSound()) SoundManager.Instance.PlayOneShot("GoblinDie"); });
        }

        private void Update() {
            _enemy.Animate();
        }

        private void FixedUpdate() {
            _enemy.Move();
        }

        public void SubscribeEvent(Action<GameObject> function) {
            _enemy.OnEnemyDie += function;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (_enemy.OnTrigger(col)) StartCoroutine(_enemy.ChangeColor(Color.red));
        }

        private void OnCollisionEnter2D(Collision2D col) {
            if (_enemy.OnCollide(col)) StartCoroutine(_enemy.ChangeColor(Color.red));
        }

        private void OnCollisionStay2D(Collision2D collision) {
            _damageTimer -= Time.deltaTime;

            if (!collision.collider.CompareTag("Player") || _damageTimer > 0) return;

            _damageTimer = CurrentStats.AttackCooldown;
            SoundManager.Instance.PlayOneShot("GoblinAttack");
            collision.collider.GetComponentInParent<PlayerManager>().TakeDamage(CurrentStats.Damage);
            _enemy.GetAnimation().AttackAnimation();
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (!other.collider.CompareTag("Player")) return;

            _damageTimer = 0f;
        }

        public void ResetStats() {
            CurrentStats.Damage = BaseStats.Damage;
            CurrentStats.Speed = BaseStats.Speed;
            CurrentStats.AttackCooldown = BaseStats.AttackCooldown;
            CurrentStats.MaxHp = BaseStats.MaxHp;
        }
    }
}
