using UnityEngine;

namespace HzrController {
	public class ProjectileInfo : MonoBehaviour
	{
		public float damage;

		private float _rayDistance = 2.0f;
		private RaycastHit _hit;

        private void Start()
        {
			Destroy(this.gameObject, 12.0f);
        }

        private void Update()
        {
			GiveDamage();
        }

        private void GiveDamage()
        {
			bool isHit = Physics.Raycast(transform.position, -transform.up, out _hit, _rayDistance);
			if (!isHit) return;

			Debug.Log(_hit.transform.name);

			Transform hitTransform = _hit.transform;
			Character character = null;
			if(hitTransform.TryGetComponent(out character))
            {
				character.ApplyDamage(damage);
            }

			Destroy(this.gameObject, 0.25f);
		}
	}
}
