// Created by Kay
// Copyright 2013 by SCIO System-Consulting GmbH & Co. KG. All rights reserved.
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using AnimatorAccess;


namespace Scio.AnimatorWrapper
{
	public class MetaInfoRepository
	{
		public class MetaInfo
		{
			public string file;
			public string backupFile;
			public MetaInfo (string file, string backupFile) {
				this.file = file;
				this.backupFile = backupFile;
			}
		}
		Dictionary<string, MetaInfo> entries = new Dictionary<string, MetaInfo> (); 

		string backupDir;
		
		public void Prepare () {
			backupDir = Preferences.GetString (Preferences.Key.BackupDir);
			if (string.IsNullOrEmpty (backupDir) || !Directory.Exists (backupDir)) {
				backupDir = Application.temporaryCachePath + "/backup";
				try {
					Directory.CreateDirectory (backupDir);
					Preferences.SetString (Preferences.Key.BackupDir, backupDir);
				} catch (System.Exception ex) {
					Debug.LogWarning (ex.Message);
					backupDir = "";
				}
			}
		}
		
		public void MakeBackup (string className, string file) {
			string backupFile = MakeBackup (file);
			entries [className] =  new MetaInfo (file, backupFile);
		}

		string GetKey (BaseAnimatorAccess component) {
			if (component != null) {
				return component.GetType ().Name;
			}
			return "";
		}
		
		public string GetFile (BaseAnimatorAccess component) {
			return Get (component).file;
		}
		
		public bool HasBackup (BaseAnimatorAccess component) {
			return !string.IsNullOrEmpty (Get (component).backupFile);
		}
		
		public string RemoveBackup (BaseAnimatorAccess component) {
			string key = GetKey (component);
			if (entries.ContainsKey (key)) {
				string s = entries[key].backupFile;
				entries[key].backupFile = "";
				return s;
			}
			return "";
		}
		
		MetaInfo Get (BaseAnimatorAccess component) {
			var key = GetKey (component);
			if (!entries.ContainsKey (key)) {
				string fileName = key +  ".cs";
				string backupFileName = key +  ".txt";
				string file = CodeGenerationUtils.GetPathToFile (fileName);
				string backupFile = CodeGenerationUtils.GetPathToFile (backupFileName, backupDir);
				entries [key] = new MetaInfo (file, backupFile);
			}
			return entries [key];
		}
		
		string MakeBackup (string inputFile) {
			if (File.Exists (inputFile) && Directory.Exists (backupDir)) {
				try {
					string className = Path.GetFileNameWithoutExtension (inputFile);
					string backupFile = Path.Combine (backupDir, className) + ".txt";
					File.Copy (inputFile, backupFile, true);
					return backupFile;
				}
				catch (System.Exception ex) {
					string msg = " threw:\n" + ex.ToString ();
					Debug.LogError (msg);
				}
			}
			return "";
		}
		
	}
}
