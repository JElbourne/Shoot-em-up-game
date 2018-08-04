using UnityEngine.EventSystems;
using UnityEngine;
using Rts2DEngine.Units;

public class InputController : MonoBehaviour {
    public GameObject target;
    FireTrait m_FireTrait;
    FaceTargetTrait m_FaceTargetTrait;
    InteractTrait m_InteractTrait;

    private void Start()
    {
        SetupInput(target);
    }

    public void SetupInput (GameObject _target) {
        m_FaceTargetTrait = _target.GetComponentInChildren<FaceTargetTrait>();
        m_FireTrait = _target.GetComponentInChildren<FireTrait>();
        m_InteractTrait = _target.GetComponentInChildren<InteractTrait>();
    }
	
	// Update is called once per frame
	void Update () {

        if(EventSystem.current.IsPointerOverGameObject())
        {
            // Pointing at UI
        } else
        {
            if (m_FaceTargetTrait)
            {
                Vector3 m_MousePos = Input.mousePosition;
                Vector3 inputWorldPosition = Camera.main.ScreenToWorldPoint(m_MousePos);
                m_FaceTargetTrait.SetDirection(inputWorldPosition);
            }

            if (m_FireTrait && Input.GetButton("Fire"))
                m_FireTrait.Shoot(true);

            if (m_InteractTrait && Input.GetButton("Interact"))
            {
                m_InteractTrait.Interact();
            }
        }
	}
}
