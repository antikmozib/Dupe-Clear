﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DupeClear.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ActivationEmail {
            get {
                return ((string)(this["ActivationEmail"]));
            }
            set {
                this["ActivationEmail"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public long ActivationCode {
            get {
                return ((long)(this["ActivationCode"]));
            }
            set {
                this["ActivationCode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1753-01-01")]
        public global::System.DateTime DayZero {
            get {
                return ((global::System.DateTime)(this["DayZero"]));
            }
            set {
                this["DayZero"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("All files (*.*)")]
        public string SearchExtensions {
            get {
                return ((string)(this["SearchExtensions"]));
            }
            set {
                this["SearchExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ExcludedExtensions {
            get {
                return ((string)(this["ExcludedExtensions"]));
            }
            set {
                this["ExcludedExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1980-01-01")]
        public global::System.DateTime DateCreatedFrom {
            get {
                return ((global::System.DateTime)(this["DateCreatedFrom"]));
            }
            set {
                this["DateCreatedFrom"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2020-12-31")]
        public global::System.DateTime DateCreatedTo {
            get {
                return ((global::System.DateTime)(this["DateCreatedTo"]));
            }
            set {
                this["DateCreatedTo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1980-01-01")]
        public global::System.DateTime DateModifiedFrom {
            get {
                return ((global::System.DateTime)(this["DateModifiedFrom"]));
            }
            set {
                this["DateModifiedFrom"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2020-12-31")]
        public global::System.DateTime DateModifiedTo {
            get {
                return ((global::System.DateTime)(this["DateModifiedTo"]));
            }
            set {
                this["DateModifiedTo"] = value;
            }
        }
    }
}
