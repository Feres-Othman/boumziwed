using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadQR : MonoBehaviour
{

	private IScanner BarcodeScanner;
	public RawImage Image;
	private float RestartTime;
	public GameObject resultText;

	// Disable Screen Rotation on that screen
	void Awake()
	{

		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
	}

	IEnumerator Start()
	{
#if UNITY_IOS
		Application.RequestUserAuthorization(UserAuthorization.WebCam);
		yield return new WaitForSeconds(.1f);
		while (!Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			Application.RequestUserAuthorization(UserAuthorization.WebCam);
			yield return new WaitForSeconds(.1f);
		}
#endif

#if UNITY_ANDROID
		Permission.RequestUserPermission(Permission.Camera);
		yield return new WaitForSeconds(.1f);
		while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
		{
			Permission.RequestUserPermission(Permission.Camera);
			yield return new WaitForSeconds(.1f);
		}
#endif

		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			// Set Orientation & Texture
			//Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();

			Vector3 camScale = BarcodeScanner.Camera.GetScale();

			//TextHeader.text += new Vector2(BarcodeScanner.Camera.Width,BarcodeScanner.Camera.Height).ToString();


			Image.transform.localScale = new Vector3(camScale.x, camScale.y / 2, camScale.z);

			Image.texture = BarcodeScanner.Camera.Texture;

			// Keep Image Aspect Ratio
			var rect = Image.GetComponent<RectTransform>();

			var newHeight = ((rect.sizeDelta.x) * BarcodeScanner.Camera.Width / BarcodeScanner.Camera.Height);

			rect.sizeDelta = new Vector2(rect.sizeDelta.x * 1.5f, newHeight * 1.5f);

			rect.rotation = Quaternion.Euler(0, 0, -90);

			//TextHeader.text += "    " + rect.sizeDelta.ToString();

			//var newHeight = (rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width);
			//rect.sizeDelta = new Vector2( newHeight, rect.sizeDelta.x);

			//rect.sizeDelta = new Vector2(Screen.width, Screen.height);


			RestartTime = Time.realtimeSinceStartup;
		};
	}

	/// <summary>
	/// Start a scan and wait for the callback (wait 1s after a scan success to avoid scanning multiple time the same element)
	/// </summary>
	private void StartScanner()
	{
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			BarcodeScanner.Stop();
			
			RestartTime += Time.realtimeSinceStartup + 1f;

			print(barCodeValue);

			Image.gameObject.SetActive(false);

			resultText.SetActive(true);

			resultText.GetComponentInChildren<TextMeshProUGUI>().text = barCodeValue;

			FindObjectOfType<SceneController>().load(3, 2);

		});
	}

	/// <summary>
	/// The Update method from unity need to be propagated
	/// </summary>
	void Update()
	{
		if (BarcodeScanner != null)
		{
			BarcodeScanner.Update();
		}

		// Check if the Scanner need to be started or restarted
		if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
		{
			StartScanner();
			RestartTime = 0;
		}
	}

	//public void test()
	//{
	//	SceneParser.token = "4e8bd0e126ee49edbb325630798af8a2";
	//	SceneManager.LoadScene(1);
	//}

	public void nextScene()
	{
		SceneManager.LoadScene(1);
	}

#region UI Buttons

	public void ClickBack()
	{
		// Try to stop the camera before loading another scene
		StartCoroutine(StopCamera(() => {
			SceneManager.LoadScene(1);
		}));
	}

	/// <summary>
	/// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
	/// Trying to stop the camera in OnDestroy provoke random crash on Android
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator StopCamera(Action callback)
	{
		// Stop Scanning
		Image = null;
		BarcodeScanner.Destroy();
		BarcodeScanner = null;

		// Wait a bit
		yield return new WaitForSeconds(0.1f);

		callback.Invoke();

	}

#endregion
}