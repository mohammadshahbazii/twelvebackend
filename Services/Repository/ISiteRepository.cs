using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface ISiteRepository : IDisposable
    {
        #region Contents
        public IndexContentCrudViewModel GetIndexContent();
        public ContactUsContentCrudViewModel GetContactUsContent();
        public AboutUsContentCrudViewModel GetAboutUsContentForEdit();
        public bool UpdateIndexContent(IndexContentCrudViewModel content);
        public bool UpdateAboutUsContent(AboutUsContentCrudViewModel content, IFormFile imageProduct, IFormFile downloadProduct);
        public bool UpdateContactUsContent(ContactUsContentCrudViewModel content);

        #endregion Contents
        public ContentItemViewModel GetContactUsSecondDescription();
        public ContentItemViewModel GetContactUsFirstDescription();
        public AboutUsLogoImagesViewModel GetAboutUsLogoImages();
        public List<AboutUsItem> GetAboutUsItems();
        public List<AboutUsArticle> GetAboutUsArticles();
        public List<AboutUsSight> GetAboutUsSights();
        public AboutUsContent GetAboutUsContent();

        public void AddDownloadCount();
        public ContentItemViewModel GetIndexAbout();
        public ContentItemViewModel GetIndexFeature();
        public ContentItemViewModel GetIndexFaq();
        public List<FaqsItemViewModel> GetFaqsBySearch(string q="");
        public SearchPageDataViewModel GetSearchContent(string q = "");
        public bool UpdateSite(SiteSetting siteSetting, IFormFile Filename , IFormFile imageProduct);
        public SiteSetting GetSiteSetting();
        public DownloadLinkCrudViewModel GetDownloadLinkModelForCreate();
        public DownloadLinkCrudViewModel GetDownloadLinkModelForUpdate(int LinkID);
        public bool Create(DownloadLinkCrudViewModel link, IFormFile ImageName);
        public bool Update(DownloadLinkCrudViewModel link, IFormFile ImageName);
        public bool Delete(DownloadLink link);
        public DownloadLinkGroup GetDownloadLinkGroup(int dlGroupID);
        public DownloadLink GetDownloadLink(int LinkID);

        public bool UpdateMedia(SocialMedium social);
        public bool CreateMedia(SocialMedium social);
        public bool DeleteMedia(SocialMedium social);
        public SocialMedium GetMedia(int MediaID);
        public List<DownloadLinkViewModel> GetDownloadLinks();
        public List<DownloadLinkGroupViewModel> GetDownloadLinkGoroups();

        public void PlusVisit();
        public string CheckMessage(FeedBack feedBack);
        public string SubmitMessage(string fullname, string Email, string Phonenumber, string Subject, string Text);
        public IndexDownloadBoxViewModel GetDownloadBox();

        public List<SocialMediaItemViewModel> GetSocialMedia();

        public string GetDirectLink();

        public SeoTagsViewModel GetSeoTags();
    }
}
