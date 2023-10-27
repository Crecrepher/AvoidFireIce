using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlay : MonoBehaviour
{
	public void Start()
	{
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		Social.localUser.Authenticate((bool success) => {
		});
	}

	public void OpenLeaderBoard()
	{
		if (PlayerPrefs.GetFloat("1-6" + "Score") > 0f)
		{
			float CompactTime = 0f;
			for (int i = 0; i < 7; i++)
			{
				CompactTime += PlayerPrefs.GetFloat($"1-{i}" + "Score");
			}
			PlayGamesPlatform.Instance.ReportScore((long)(CompactTime * 1000f), "CgkIipnPp7EPEAIQBQ", (bool success) =>
			{
				// Handle success or failure
			});
		}
		if (PlayerPrefs.GetFloat("2-7" + "Score") > 0f)
		{
			float CompactTime = 0f;
			for (int i = 0; i < 8; i++)
			{
				CompactTime += PlayerPrefs.GetFloat($"2-{i}" + "Score");
			}
			PlayGamesPlatform.Instance.ReportScore((long)(CompactTime * 1000f), "CgkIipnPp7EPEAIQAw", (bool success) =>
			{
				// Handle success or failure
			});
		}
		if (PlayerPrefs.GetFloat("3-6" + "Score") > 0f)
		{
			float CompactTime = 0f;
			for (int i = 0; i < 7; i++)
			{
				CompactTime += PlayerPrefs.GetFloat($"3-{i}" + "Score");
			}
			PlayGamesPlatform.Instance.ReportScore((long)(CompactTime * 1000f), "CgkIipnPp7EPEAIQBA", (bool success) =>
			{
				// Handle success or failure
			});
		}
		PlayGamesPlatform.Instance.ShowLeaderboardUI();
	}
}
