using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.MNUinteractive.SceneSelector
{
	public class SceneSelectorToolbar : EditorWindow
	{
		private List<Tuple<string,string>> sceneNames;
		private int selectedIndex = 0;

		[MenuItem("MNUinteractive/Scene Selector")]
		public static void ShowWindow()
		{
			var toolbar = GetWindow<SceneSelectorToolbar>();
			toolbar.titleContent = new GUIContent("Scene Selector");
			toolbar.minSize = new Vector2(300, 20);
			toolbar.maxSize = new Vector2(2000, 20);
			toolbar.Show();
		}

		private void OnEnable()
		{
			RefreshSceneList();
			SelectOpenedScene();
		}

		private void SelectOpenedScene()
		{
			var currentScene = EditorSceneManager.GetActiveScene();
			var currentSceneName = currentScene.name;
			var currentSceneIndex = sceneNames.FindIndex(t => t.Item2 == currentSceneName);
			if (currentSceneIndex != -1)
			{
				selectedIndex = currentSceneIndex;
			}
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.FlexibleSpace();

			var oldSelectedIndex = selectedIndex;

			selectedIndex = EditorGUILayout.Popup(selectedIndex, sceneNames.Select( t=> t.Item2).ToArray());

			if (oldSelectedIndex != selectedIndex)
			{
				OpenSelectedScene();
			}

			if (GUILayout.Button("Refresh"))
			{
				RefreshSceneList();
			}

			if (GUILayout.Button("Load"))
			{
				OpenSelectedScene();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void OpenSelectedScene()
		{
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.OpenScene(sceneNames[selectedIndex].Item1);
			}
		}

		private void RefreshSceneList()
		{
			sceneNames = new List<Tuple<string, string>>();

			string[] sceneFiles = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
			foreach (string sceneFile in sceneFiles)
			{
				sceneNames.Add(new Tuple<string, string>("Assets" + sceneFile.Substring(Application.dataPath.Length),
					Path.GetFileNameWithoutExtension(sceneFile)));
			}
		}
	}
}