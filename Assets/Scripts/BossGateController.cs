using UnityEngine;

public class BossGateController : MonoBehaviour
{    
    private GameObject _gateLeft;
    private GameObject _gateRight;
    private GateState _state = GateState.Closed;
    private float _gateOpeningTimeElapsed = 0.0f;

    private static float GATE_OPENING_TIME = 3.0f;

    void Start()
    {
        _gateLeft = GameObject.Find("GateLeft");
        _gateRight = GameObject.Find("GateRight");
    }

    void Update()
    {
        if(_state == GateState.Opening && _gateOpeningTimeElapsed < GATE_OPENING_TIME)
        {
            _gateOpeningTimeElapsed += Time.deltaTime;

            var degrees = (90.0f / GATE_OPENING_TIME) * Time.deltaTime;
            _gateLeft.transform.Rotate(0, -degrees, 0);
            _gateRight.transform.Rotate(0, degrees, 0);
        }
        else
        {
            _state = GateState.Opened;
        }
    }

    public void OpenGate() => _state = GateState.Opening;
}

enum GateState
{
    Closed = 1,
    Opening = 2,
    Opened = 3
}