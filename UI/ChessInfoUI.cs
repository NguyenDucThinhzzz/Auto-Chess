using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessInfoUI : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI NameText { get; private set; }

	[field: SerializeField] public TextMeshProUGUI DescriptionText { get; private set; }

	[field: SerializeField] public TextMeshProUGUI ATKText { get; private set; }

	[field: SerializeField] public TextMeshProUGUI HPText { get; private set; }

	[field: SerializeField] public TextMeshProUGUI DEFText { get; private set; }

	[field: SerializeField] public TextMeshProUGUI SPDText { get; private set; }

	[field: SerializeField] public TextMeshProUGUI ATKSPDText { get; private set; }

	[field: SerializeField] public TextMeshProUGUI RangeText { get; private set; }

	[field: SerializeField] public UnityEngine.UI.Image Image { get; private set; }
	public void UpdateInfo(BaseChess chess)
	{
		Image.sprite = chess.Data.Image;
		NameText.text = chess.Data.Name;
		DescriptionText.text = chess.Data.Description;
		ATKText.text = chess.BaseStats.Attack.Value.ToString();
		HPText.text = chess.BaseStats.Health.Value.ToString();
		DEFText.text = chess.BaseStats.Defence.Value.ToString();
		SPDText.text = chess.BaseStats.Speed.Value.ToString();
		ATKSPDText.text = chess.BaseStats.AttackSpeed.Value.ToString();
		RangeText.text = chess.BaseStats.Range.Value.ToString();
	}
}
