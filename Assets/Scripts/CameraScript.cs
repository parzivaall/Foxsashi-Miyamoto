using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    #region Variables
    
        private Vector3 _offset;
        [SerializeField] private Transform target;
        [SerializeField] public float smoothTime;
        private Vector3 _currentVelocity = Vector3.zero;
        private Camera _camera;
        public float screenHeigtht = 10f;
        
    #endregion
    
    #region Unity callbacks
    
        private void Awake() {
            _offset = transform.position - target.position;
            _camera = GetComponent<Camera>();
            _camera.orthographicSize = screenHeigtht;
            
        }

        private void LateUpdate()
        {
            Vector3 targetPosition = target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
            StartCoroutine(WaitAndExecute(2f));

            
            
        }

        IEnumerator WaitAndExecute(float waitTime) {
            yield return new WaitForSeconds(waitTime);
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, screenHeigtht/2, Time.deltaTime * 1f);
        }
        
    #endregion
}
