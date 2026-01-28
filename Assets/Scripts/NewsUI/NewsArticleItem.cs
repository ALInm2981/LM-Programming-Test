using InfinityHeroes.News.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InfinityHeroes.News.UI
{
    public class NewsArticleItem : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private RawImage _articleImage;
        [SerializeField] private Button _clickButton;

        private INewsArticle _article;

        public async void Initialize(INewsArticle article)
        {
            _article = article;
            if (_article == null) return;
            if (_titleText != null) _titleText.text = _article.Title;
            if (_contentText != null) _contentText.text = _article.Contents;

            if (_clickButton != null)
            {
                _clickButton.onClick.RemoveAllListeners();
                _clickButton.onClick.AddListener(OnClicked);
            }

            if (_articleImage != null)
            {
                var texture = await _article.GetTextureAsync();
                if (this != null && texture != null)
                {
                    _articleImage.texture = texture;
                }
            }
        }

        private void OnClicked()
        {
            if (_article != null && _article.Source != null)
            {
                _article.Source.Open();
            }
            else
            {
                Debug.LogWarning("Cannot open article source: Source is null.");
            }
        }
    }
}