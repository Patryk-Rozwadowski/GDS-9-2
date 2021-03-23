using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraftPickController : MonoBehaviour {
    [SerializeField] private GameObject _leftTeamPanel, _rightTeamPanel;
    [SerializeField] private Button _button;
    public List<GameObject> _leftTeam, _rightTeam;

    private bool _pickedPosition;
    private GameObject _pickedUnit;
    void Start() {
        // _rightTeamPanel.SetActive(false);
        _leftTeam = new List<GameObject>();
        _rightTeam = new List<GameObject>();
    }

    public void PickUnit(GameObject element) {
        _leftTeam.Add(element);
        Debug.Log(_leftTeam);

        _pickedPosition = element;
        if (Input.GetMouseButtonDown(0) && _pickedUnit != null) {
            _pickedPosition = true;
            var el = Instantiate(element, CursorUtils.GetMouseWorldPosition(), Quaternion.identity);
            el.transform.localScale = new Vector3(5,5,1);
        }
       
    }
}