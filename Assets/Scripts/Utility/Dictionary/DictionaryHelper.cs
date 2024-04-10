using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Utility.Dictionary
{
    public class DictionaryHelper : IDisposable
    {
        private string _fileContent;
        private HashSet<string> _validWords;

        public async Task ReadFile()
        {
            var assetManager = InstanceManager.GetInstanceAsSingle<AssetManager>();
            var textAsset = await assetManager.LoadAssetAsync<TextAsset>("words");
            _fileContent = textAsset.text;
            
            _validWords = new HashSet<string>();
            var splitFileContent = _fileContent.Split('\n');
            foreach (var word in splitFileContent)
            {
                _validWords.Add(word);
            }
        }

        public bool IsWordValid(string word)
        {
            word  = word.ToLower();
            return word.Length>2 && _validWords.Contains(word);
        }


        public void Dispose()
        {
            _validWords.Clear();
        }
    }
    
        
    }
