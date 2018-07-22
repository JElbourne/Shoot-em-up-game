using UnityEngine.EventSystems;
using UnityEngine;

public class InputController : MonoBehaviour {

    MoveTrait m_MoveTrait;
    FireTrait m_FireTrait;
    FaceTargetTrait m_FaceTargetTrait;
    InteractTrait m_InteractTrait;
    //PathfindingTrait m_PathfindingTrait;

    public void SetupInput (GameObject _target) {
        m_MoveTrait = _target.GetComponent<MoveTrait>();
        m_FaceTargetTrait = _target.GetComponentInChildren<FaceTargetTrait>();
        m_FireTrait = _target.GetComponentInChildren<FireTrait>();
        m_InteractTrait = _target.GetComponentInChildren<InteractTrait>();
        //m_PathfindingTrait = _target.GetComponent<PathfindingTrait>();
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

            if (m_MoveTrait)
            {
                Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                m_MoveTrait.Move(input);
            }

            //if (m_PathfindingTrait)
            //{
            //    if(Input.GetMouseButtonDown(1))
            //    {
            //        Vector3 m_MousePos = Input.mousePosition;
            //        Vector3 inputWorldPosition = Camera.main.ScreenToWorldPoint(m_MousePos);
            //        m_PathfindingTrait.Move(inputWorldPosition);
            //    }
            //}

            if (m_FireTrait && Input.GetButton("Fire"))
                m_FireTrait.Shoot(true);

            if (m_InteractTrait && Input.GetButton("Interact"))
            {
                m_InteractTrait.Interact();
            }
        }
	}
}
