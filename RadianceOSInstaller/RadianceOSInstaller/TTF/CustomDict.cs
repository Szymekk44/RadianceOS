using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * <summary>
 *  Only supports string as key to allow for == operator, as Equals doesn't seem to work with cosmos
 * </summary>
 */
namespace CosmosTTF {
    public class CustomDictString<TV> : IEnumerable {
        private List<CustomDictEntry<TV>> entries;
        public int Count => entries.Count;

        public CustomDictString() {
            this.entries = new();
        }

        public void Add(string key, TV value) {
            this.entries.Add(new(key, value));
        }

        public TV Get(string key) {
            foreach (var entry in this.entries) {
                if (entry.key == key) return entry.value;
            }

            throw new NullReferenceException("CustomDict does not include key");
        }

        public void Set(string key, TV value) {
            foreach (var entry in this.entries) {
                if (entry.key == key) {
                    entry.value = value;
                    return;
                }
            }

            this.entries.Add(new(key, value));
        }

        public bool Remove(string key) {
            int i = 0;
            foreach (var entry in this.entries) {
                if (entry.key == key) {
                    entries.RemoveAt(0);
                    return true;
                }

                i++;
            }

            return false;
        }

        public bool TryGet(string key, out TV val) {
            foreach (var entry in this.entries) {
                //TTFManager.DebugUIPrint("CustomDict TryGet: Checking entry");
                if (entry.key == key) {
                    //TTFManager.DebugUIPrint("CustomDict TryGet: Entry matched, setting out");
                    val = entry.value;
                    //TTFManager.DebugUIPrint("CustomDict TryGet: Entry matched, setted out and returning true");
                    return true;
                }
            }

            //TTFManager.DebugUIPrint("CustomDict TryGet: Not found, setting value to default(TV)");
            val = default(TV);
            //TTFManager.DebugUIPrint("CustomDict TryGet: Return false");
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.entries.GetEnumerator();
        }

        public TV this[string key] {
            get => Get(key);
            set => Set(key, value);
        }
    }

    internal class CustomDictEntry<TV> {
        public string key;
        public TV value;

        public CustomDictEntry(string key, TV value) {
            this.key = key;
            this.value = value;
        }
    }
}
