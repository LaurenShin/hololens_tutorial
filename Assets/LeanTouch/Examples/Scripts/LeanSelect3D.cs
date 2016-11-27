using UnityEngine;

namespace Lean.Touch
{
	// This script allows you to select a GameObject using any finger, as long it has a collider
	public class LeanSelect3D : MonoBehaviour
	{

        public GameObject explosion;

		[Tooltip("This stores the layers we want the raycast to hit (make sure this GameObject's layer is included!)")]
		public LayerMask LayerMask = UnityEngine.Physics.DefaultRaycastLayers;
		
		[Tooltip("The currently selected GameObject")]
		public GameObject SelectedGameObject;

		[Tooltip("Change the color of the selected GameObject?")]
		public bool ColorSelected = true;

		[Tooltip("The color of the selected GameObject")]
		public Color SelectedColor = Color.green;
		
		protected virtual void OnEnable()
		{
			// Hook into the events we need
			LeanTouch.OnFingerTap += OnFingerTap;
		}
		
		protected virtual void OnDisable()
		{
			// Unhook the events
			LeanTouch.OnFingerTap -= OnFingerTap;
		}
		
		public void OnFingerTap(LeanFinger finger)
		{
			// Raycast information
			var ray = finger.GetRay();
			var hit = default(RaycastHit);
			
			// Was this finger pressed down on a collider?
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask) == true)
			{
				// Select the hit GameObject
				Select(hit.collider.gameObject);
                Instantiate(explosion, hit.collider.gameObject.transform.position, transform.rotation);
                Destroy(hit.collider.gameObject);
			}
            
			else
			{
				// Nothing was tapped, so deselect
				Deselect();
			}
		}

		private void Deselect()
		{
			// Is there a selected GameObject?
			if (SelectedGameObject != null)
			{
				// Remove color?
				if (ColorSelected == true)
				{
					ColorGameObject(SelectedGameObject, Color.white);
				}

				// Mark selected GameObject null
				SelectedGameObject = null;
			}
		}

		private void Select(GameObject newGameObject)
		{
			// Has the selected GameObject changed?
			if (newGameObject != SelectedGameObject)
			{
				// Deselect the old GameObject
				Deselect();

				// Change selection
				SelectedGameObject = newGameObject;

				// Apply color to newly selected GameObject?
				if (ColorSelected == true)
				{
					ColorGameObject(SelectedGameObject, SelectedColor);
				}
			}
		}
		
		private static void ColorGameObject(GameObject gameObject, Color color)
		{
			// Make sure the GameObject exists
			if (gameObject != null)
			{
				// Get renderer from this GameObject
				var renderer = gameObject.GetComponent<Renderer>();

				// Make sure the Renderer exists
				if (renderer != null)
				{
					// Get material copy from this renderer
					var material = renderer.material;

					// Make sure the material exists
					if (material != null)
					{
						// Set new color
						material.color = color;
					}
				}
			}
		}
	}
}