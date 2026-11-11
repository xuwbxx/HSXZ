using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace System.Configuration {


    internal class Const {
        public const string CollectionElementName = "collection";
        public const string SingleElementName = "property";
        public const string ItemElementName = "item";
        public const string KeyAttributeName = "key";
        public const string ValueAttributeName = "value";
    }


    public class Settings : Dictionary<string, string> {
        public string Value {
            get { return this[Const.ValueAttributeName]; }
            set { this[Const.ValueAttributeName] = value; }
        }
    }

    public class SettingCollection : IEnumerable {
        private List<Settings> _settingList;

        public SettingCollection() {
            _settingList = new List<Settings>();
        }

        internal void Add(Settings keyValues) {
            _settingList.Add(keyValues);
        }

        public Settings this[int i] {
            get { return _settingList[i]; }
        }

        public string this[string name] {
            get { return _settingList[0][name]; }
        }

        public string Value {
            get { return _settingList[0][Const.ValueAttributeName]; }
        }

        public int Count {
            get { return _settingList.Count; }
        }

        #region IEnumerable 成员

        public IEnumerator GetEnumerator() {
            return _settingList.GetEnumerator();
        }

        #endregion
    }

}
