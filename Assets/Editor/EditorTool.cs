using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class EditorTool
{
    const string needExportResPath = "Assets/NeedExportRes";
    const string suffixName = ".bytes";
    const string voicePath = "Assets/NeedExportRes/voiceNames.asset";
    const string atlas_mainPath = "Assets/NeedExportRes/UIMain.asset";
    const string atlas_qrCodePath = "Assets/NeedExportRes/UIQRCode.asset";

    //#region  设置animator 动画片段播放速度
    //static string path = "Assets/Animation/fishhook.controller";
    //static AnimatorController animator = AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController)) as AnimatorController;

    //[MenuItem("Tools/ChangeAnimSpeed/fishhook_down")]
    //static void fishhook_down()
    //{
    //    SetAniamtionSpeed(animator, AnimationName.down, 0.5f);
    //    SetAniamtionSpeed(animator, AnimationName.up, 0.5f);
    //}
    //[MenuItem("Tools/ChangeAnimSpeed/fishhook_get")]
    //static void fishhook_get()
    //{
    //    SetAniamtionSpeed(animator, AnimationName.catchs, 1f);
    //    SetAniamtionSpeed(animator, AnimationName.release, 2f);
    //}

    //static void SetAniamtionSpeed(AnimatorController ac, AnimationName name, float speed)
    //{
    //    //AnimatorController ac = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
    //    AnimatorControllerLayer[] layers = ac.layers;
    //    AnimatorStateMachine state = layers[0].stateMachine;
    //    ChildAnimatorState[] sts = state.states;
    //    for (int i = 0; i < sts.Length; i++)
    //    {
    //        if (name.ToString() == sts[i].state.name)
    //        {
    //            sts[i].state.speed = speed;
    //            Debug.Log(name + "------修改成功");
    //            break;
    //        }
    //    }
    //}

    //#endregion

    #region 制作语音数据资源 图集资源
    //打包语音数据包
    [MenuItem("Tools/ScriptableObject/BuildAsset_Audio")]
    static void BuildAssetBundlesExcell()
    {
        ExcelScriptObj es = ScriptableObject.CreateInstance<ExcelScriptObj>();
        es.voiceData = ExcelAccess.SelectTables(ExcelAccess.ExcelName);
        es.voiceType = ExcelAccess.SelectTables(ExcelAccess.ExcelType);
        if (File.Exists(voicePath))
            File.Delete(voicePath);
        AssetDatabase.CreateAsset(es, voicePath);
        AssetDatabase.SaveAssets();
        //EditorUtility.FocusProjectWindow();
        Selection.activeObject = es;
        Debug.Log("Build ScripteObj_Audio Success");
        AssetDatabase.Refresh();
    }
    ////打包图集
    [MenuItem("Tools/ScriptableObject/BuildAsset_UIAtlas")]
    static void BuildAssetGameStaus()
    {
        string atlasPath = "UIAtlas/";
        UIAtlasMain_Scriptable main = ScriptableObject.CreateInstance<UIAtlasMain_Scriptable>();
        main.sprites = Resources.LoadAll<Sprite>(atlasPath + UIAtlasName.UIMain);
        UIAtlasQRCode_Scriptable qrCode = ScriptableObject.CreateInstance<UIAtlasQRCode_Scriptable>();
        qrCode.sprites = Resources.LoadAll<Sprite>(atlasPath + UIAtlasName.UIQRCode);

        if (File.Exists(atlas_mainPath))
            File.Delete(atlas_mainPath);
        AssetDatabase.CreateAsset(main, atlas_mainPath);
        if (File.Exists(atlas_qrCodePath))
            File.Delete(atlas_qrCodePath);
        AssetDatabase.CreateAsset(qrCode, atlas_qrCodePath);
        AssetDatabase.SaveAssets();
        Debug.Log("Build ScripteObj_atlas Success");
        AssetDatabase.Refresh();
    }
    #endregion


    //打包android工程
    static void BuildProject()
    {
        string android_project_path = @"C:\Users\jhz\Desktop\UnityBao";
        BuildTarget target = BuildTarget.Android;
        string[] buildScences = new[] { "Assets/Scenes/loading.unity"};
        BuildPipeline.BuildPlayer(buildScences, android_project_path, target,
            BuildOptions.AcceptExternalModificationsToPlayer);
    }



    [MenuItem("Tools/ExportAssetBuild")]
    static void ExportAssetBuild()
    {
        ExportHelp.ClearConsole();
        AssetDatabase.Refresh();
        string currentPath = Directory.GetCurrentDirectory() + "\\";
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        DirectoryInfo dinfo = new DirectoryInfo(needExportResPath);
        List<FileInfo> fs = dinfo.GetFiles("*.*", SearchOption.AllDirectories).ToList().FindAll(m => !m.Name.EndsWith(".meta")&&m.Name!="md5.txt");
        List<string> needExportList= ExportHelp.MD5Comparison(needExportResPath, fs);
        if (needExportList.Count == 0)
        {
            Debug.Log("没有资源更新不需打包");
            return;
        }
        foreach (FileInfo file in fs)
        {
            AssetBundleBuild tbuild = new AssetBundleBuild();
            tbuild.assetBundleName = file.Name + suffixName;
            string fullName = file.FullName;
            fullName = fullName.Replace(currentPath, "");
            fullName = fullName.Replace("\\", "/");
            tbuild.assetNames = new string[] { fullName };
            builds.Add(tbuild);
        }
       ExportHelp.GoExport(builds.ToArray(), needExportList);
    }
}
