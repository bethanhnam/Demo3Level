//using UnityEditor.Rendering;
//using UnityEngine;
//using UnityEngine.UI;

//public class PlayButton : MonoBehaviour
//{
//	public static PlayButton instance;
//	public Button button;
//	public int level;
//	public bool haschange=false;
//	// Start is called before the first frame update
//	private void Awake()
//	{
//		if (instance == null)
//		{
//			instance = this;
//		}
//		button = GetComponent<Button>();
//	}
//	void Start()
//	{
//		level = 0;	
//	}
//	// Update is called once per frame
//	void Update()
//	{
//		if(haschange)
//		{
//			PlayButton.instance.button.onClick.AddListener(() => GameManager.instance.SetCurrentLevel(level));
//			PlayButton.instance.button.onClick.AddListener(UIManager.instance.menuPanel.PlayGame);
//			haschange = false;
//		}
//	}
//	private void OnEnable()
//	{
		
//	}
//}
