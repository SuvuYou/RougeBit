using UnityEngine;

public class PalyerInput : MonoBehaviour
{
    [SerializeField] private PalyerInputSO _palyerInputSO;

    private Vector2 _input;

    void Update()
    {
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");

        _palyerInputSO.SetMovementInput(newInput: _input);
    }
}
