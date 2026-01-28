using System;
using InfinityHeroes.News.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace InfinityHeroes.News.UI
{
    public class NewsController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Button _fetchButton;
        [SerializeField] private Transform _contentContainer;
        [SerializeField] private GameObject _articlePrefab;

        //private NewsClient _newsClient;

        private void Awake()
        {
            _fetchButton.onClick.AddListener(FetchNews);
        }

        private async void FetchNews()
        {
            NewsClient _newsClient = new NewsClient();

            if (!IsValid()) return;

            // prevent spaming
            _fetchButton.interactable = false;

            try
            {
                INewsResponse response = await _newsClient.GetArticlesAsync();

                if (response != null && response.Articles != null)
                {
                    // clear the old items
                    foreach (Transform child in _contentContainer)
                    {
                        Destroy(child.gameObject);
                    }

                    // place new items
                    foreach (var article in response.Articles)
                    {
                        CreateNewsItem(article);
                    }
                }
                else
                {
                    Debug.LogWarning("Fetch failed or returned no articles. Keeping old list.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to fetch news: {ex.Message}");
            }
            finally
            {
                _fetchButton.interactable = true;
            }
        }

        private void CreateNewsItem(INewsArticle article)
        {
            GameObject itemObj = Instantiate(_articlePrefab, _contentContainer);

            if (itemObj.TryGetComponent<NewsArticleItem>(out var itemScript))
            {
                itemScript.Initialize(article);
            }
            else
            {
                Debug.LogError("The Article Prefab is missing the 'NewsArticleItem' script!");
            }
        }

        private bool IsValid()
        {
            if (_fetchButton == null || _contentContainer == null || _articlePrefab == null)
            {
                Debug.LogError("NewsController: Please assign all UI references in the inspector.");
                return false;
            }
            return true;
        }
    }
}