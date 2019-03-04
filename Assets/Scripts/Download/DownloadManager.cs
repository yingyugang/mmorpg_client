using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CSV;
using BlueNoah.IO;

namespace MMO
{

    public class DownloadManager : SingleMonoBehaviour<DownloadManager>
    {

        public int maxDownloadCount = 5;
        public int totalDownloadSize = 0;
        public int totalDownloadedSize = 0;
        public bool isDownloading = false;

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            DownloadVersionCSV();
        }

        List<VersionCSVStructure> mVersions;
        int mDownloadingCount = 0;

        void DownloadVersionCSV()
        {
            StartCoroutine(_StartDownloadCSV());
        }

        void FiltVersionList()
        {
            List<VersionCSVStructure> filtedVersionList = new List<VersionCSVStructure>();
            for (int i = 0; i < mVersions.Count; i++)
            {
                VersionCSVStructure versionCSV = mVersions[i];
                string path = PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + versionCSV.FileName;
                if (FileManager.Exists(path))
                {
                    string hashCode = FileManager.GetFileHash(path);
                    if (hashCode.Trim() != versionCSV.HashCode.Trim())
                    {
                        filtedVersionList.Add(versionCSV);
                        totalDownloadSize += versionCSV.FileSize;
                    }
                }
                else
                {
                    filtedVersionList.Add(versionCSV);
                    totalDownloadSize += versionCSV.FileSize;
                }
            }
            mVersions = filtedVersionList;
        }

        private IEnumerator _StartDownloadCSV()
        {
            Debug.Log("_StartDownloadCSV".AliceblueColor());
            string URL = PathConstant.SERVER_RESOURCE_VERSION_CSV;
            Debug.Log(URL);
            var www = new WWW(URL);
            yield return www;
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                var stream = new MemoryStream(www.bytes);
                var reader = new StreamReader(stream);
                CsvContext mCsvContext = new CsvContext();
                IEnumerable<VersionCSVStructure> list = mCsvContext.Read<VersionCSVStructure>(reader);
                mVersions = new List<VersionCSVStructure>(list);
                FiltVersionList();
                StartCoroutine(_DownloadAssets());
            }
            else
            {
                Debug.Log(www.error);
                SceneManager.LoadCharacterSelect();
            }
            www.Dispose();
            www = null;
        }


        IEnumerator _DownloadAssets()
        {
            Debug.Log("_DownloadAssets".AliceblueColor());
            isDownloading = true;
            while (true)
            {
                if (mVersions.Count == 0 && mDownloadingCount == 0)
                {
                    Debug.Log("Download Done!".AliceblueColor());
                    //				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (2);
                    SceneManager.LoadCharacterSelect();
                    yield break;
                }
                if (mDownloadingCount < maxDownloadCount && mVersions.Count > 0)
                {
                    GameObject go = new GameObject("Downloader");
                    Downloader downloader = go.AddComponent<Downloader>();
                    mDownloadingCount++;
                    downloader.StartDownload(mVersions[0].FileName, () =>
                    {
                        mDownloadingCount--;
                    });
                    mVersions.RemoveAt(0);
                }
                yield return null;
            }
        }

    }
}
