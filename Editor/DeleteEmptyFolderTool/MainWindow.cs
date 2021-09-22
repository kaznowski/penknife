using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace DoubleDash.DeleteEmptyDirTools
{
    public class MainWindow : EditorWindow
    {
        List<DirectoryInfo> emptyDirs;
        Vector2 scrollPosition;
        bool lastCleanOnSave;
        string delayedNotiMsg;
        
        bool hasNoEmptyDir { get { return emptyDirs == null || emptyDirs.Count == 0; } }

        const float DIR_LABEL_HEIGHT = 21;

        [MenuItem("DoubleDash/Delete Empty Folders")]
        public static void ShowWindow()
        {
            var w = GetWindow<MainWindow>();
            w.titleContent.text = "Clean";
        }

        void OnEnable()
        {
            lastCleanOnSave = DeleteEmptyFolders.CleanOnSave;
            DeleteEmptyFolders.OnAutoClean += Core_OnAutoClean;
            delayedNotiMsg = "Click 'Find Empty Dirs' Button.";
        }
        
        void OnDisable()
        {
            DeleteEmptyFolders.CleanOnSave = lastCleanOnSave;
            DeleteEmptyFolders.OnAutoClean -= Core_OnAutoClean;
        }
        void Core_OnAutoClean()
        {
            delayedNotiMsg = "Cleaned on Save";
        }

        void OnGUI()
        {
            if ( delayedNotiMsg != null )
            {
                ShowNotification( new GUIContent( delayedNotiMsg ) );
                delayedNotiMsg = null;
            }

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Find Empty Dirs"))
                    {
                        DeleteEmptyFolders.FillEmptyDirList(out emptyDirs);
                        if (hasNoEmptyDir)
                        {
                            ShowNotification( new GUIContent( "No Empty Directory" ) );
                        }
                        else
                        {
                            RemoveNotification();
                        }
                    }
                    if ( ColorButton( "Delete All", ! hasNoEmptyDir, Color.red ) )
                    {
                        DeleteEmptyFolders.DeleteAllEmptyDirAndMeta(ref emptyDirs);
                        ShowNotification( new GUIContent( "Deleted All" ) );
                    }
                }
                
                EditorGUILayout.EndHorizontal();
                bool cleanOnSave = GUILayout.Toggle(lastCleanOnSave, " Clean Empty Dirs Automatically On Save");
                if (cleanOnSave != lastCleanOnSave)
                {
                    lastCleanOnSave = cleanOnSave;
                    DeleteEmptyFolders.CleanOnSave = cleanOnSave;
                }

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                if ( ! hasNoEmptyDir )
                {
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true));
                    {
                        EditorGUILayout.BeginVertical();
                        {
#if UNITY_4_6   // and higher
                            GUIContent folderContent = EditorGUIUtility.IconContent("Folder Icon");
#else
                            GUIContent folderContent = new GUIContent();
#endif
                            foreach (var dirInfo in emptyDirs)
                            {
                                UnityEngine.Object assetObj = AssetDatabase.LoadAssetAtPath( "Assets", typeof(UnityEngine.Object) );
                                if ( null != assetObj )
                                {
                                    folderContent.text = DeleteEmptyFolders.GetRelativePath(dirInfo.FullName, Application.dataPath);
                                    GUILayout.Label( folderContent, GUILayout.Height( DIR_LABEL_HEIGHT ) );
                                }
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndScrollView();
                }
            }
            EditorGUILayout.EndVertical();
        }

        void ColorLabel(string title, Color color)
        {
            Color oldColor = GUI.color;
            //GUI.color = color;
            GUI.enabled = false;
            GUILayout.Label(title);
            GUI.enabled = true;;
            GUI.color = oldColor;
        }
        bool ColorButton(string title, bool enabled, Color color)
        {
            bool oldEnabled = GUI.enabled;
            Color oldColor = GUI.color;

            GUI.enabled = enabled;
            GUI.color = color;

            bool ret = GUILayout.Button(title);

            GUI.enabled = oldEnabled;
            GUI.color = oldColor;
            
            return ret;
        }
    }
}
