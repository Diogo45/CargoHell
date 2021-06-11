using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyInfoController : MonoBehaviour
{

    [SerializeField] private TMP_InputField PositionX;
    [SerializeField] private TMP_InputField PositionY;

    [SerializeField] private TMP_InputField Rotation;
    //[SerializeField] private TMP_InputField DirectionY;

    [SerializeField] private TMP_InputField Speed;

    [SerializeField] private TMP_InputField Delay;

    [SerializeField] private Toggle ShouldMove;


    [SerializeField] private EnemyInfoUIController _uiController;

    private void Awake()
    {
        _uiController = GetComponent<EnemyInfoUIController>();

        PositionX.onValueChanged.AddListener(PosXInput);
        PositionY.onValueChanged.AddListener(PosYInput);

        Rotation.onValueChanged.AddListener(RotationInput);

        Speed.onValueChanged.AddListener(SpeedInput);
        Delay.onValueChanged.AddListener(DelayInput);

        ShouldMove.onValueChanged.AddListener(ShouldMoveInput);



    }

    private void OnEnable()
    {
        var selected = LevelCreatorUI.instance._selectedObject;

        var pos = Camera.main.WorldToViewportPoint(selected.transform.position);

        string posx = pos.x.ToString().Substring(0, (4 > pos.x.ToString().Length ? pos.x.ToString().Length : 4));
        string posy = pos.y.ToString().Substring(0, (4 > pos.y.ToString().Length ? pos.y.ToString().Length : 4));

        PositionX.SetTextWithoutNotify(posx);
        PositionY.SetTextWithoutNotify(posy);

        Rotation.SetTextWithoutNotify(selected.transform.rotation.eulerAngles.z.ToString());

        Speed.SetTextWithoutNotify(selected.Speed.ToString());
        Delay.SetTextWithoutNotify(selected.Delay.ToString());
        ShouldMove.SetIsOnWithoutNotify(selected.ShouldMove);



    }

    void PosXInput(string pos)
    {
        bool parse = float.TryParse(pos, out float posX);

        if (!parse) return;

        var selected = LevelCreatorUI.instance._selectedObject;

        var viewport = Camera.main.WorldToViewportPoint(selected.transform.position);

        selected.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(posX, viewport.y, viewport.z));

    }

    void PosYInput(string pos)
    {
        bool parse = float.TryParse(pos, out float posY);

        if (!parse) return;

        var selected = LevelCreatorUI.instance._selectedObject;

        var viewport = Camera.main.WorldToViewportPoint(selected.transform.position);

        selected.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(viewport.x, posY , viewport.z));

    }

    void RotationInput(string dir)
    {
        bool parse = float.TryParse(dir, out float rotation);

        if (!parse) return;

        var selected = LevelCreatorUI.instance._selectedObject;

        selected.transform.rotation = Quaternion.Euler(selected.transform.rotation.x, selected.transform.rotation.y, rotation);

    }

    void DirYInput(string dir)
    {
        bool parse = float.TryParse(dir, out float dirY);

        if (!parse) return;

        var selected = LevelCreatorUI.instance._selectedObject;

        selected.transform.up = new Vector3(selected.transform.up.x, dirY, selected.transform.up.z);

    }

    void SpeedInput(string speed)
    {
        bool parse = float.TryParse(speed, out float newSpeed);

        if (!parse) return;

        var selected = LevelCreatorUI.instance._selectedObject;

        selected.Speed = newSpeed;
    }

    void DelayInput(string delay)
    {
        bool parse = float.TryParse(delay, out float newDelay);

        if (!parse) return;

        var selected = LevelCreatorUI.instance._selectedObject;

        selected.Speed = newDelay;
    }

    void ShouldMoveInput(bool move)
    {
        var selected = LevelCreatorUI.instance._selectedObject;

        selected.ShouldMove = move;
    }


}
