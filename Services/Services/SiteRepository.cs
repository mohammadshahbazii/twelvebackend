using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using ViewModels;

namespace Services
{
    public class SiteRepository : ISiteRepository
    {
        TwelveDbContext db = new TwelveDbContext();

        public SeoTagsViewModel GetSeoTags()
        {
            var modelDB = db.SiteSettings.FirstOrDefault();
            modelDB.ApplyTranslation(db);
            var content = new SeoTagsViewModel()
            {
                Title = modelDB.Title,
                Description = modelDB.ShortDescription,
            };
            return content;
        }

        public string CheckMessage(FeedBack feedBack)
        {
            if (string.IsNullOrEmpty(feedBack.Fullname))
            {
                return "کاربر گرامی لطفا نام و نام خانوادگی خود را وارد کنید";
            }
            else if (string.IsNullOrEmpty(feedBack.Subject))
            {
                return "کاربر گرامی لطفا موضوع پیام خود را وارد کنید";
            }
            else if (string.IsNullOrEmpty(feedBack.Text))
            {
                return "کاربر گرامی لطفا متن پیام خود را وارد کنید";
            }
            else
            {
                return "موفق";
            }
        }

        public bool Create(DownloadLinkCrudViewModel link, IFormFile ImageName)
        {
            try
            {
                if (link.SelectedGroups.Any())
                {
                    if (ImageName != null)
                    {
                        link.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", link.ImageName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            ImageName.CopyTo(stream);
                        }

                    }
                    foreach (var group in link.SelectedGroups)
                    {
                        DownloadLink downloadLink = new DownloadLink()
                        {
                            DlgroupId = group,
                            ShortDescription = link.ShortDescription,
                            ImageName = link.ImageName,
                            Link = link.Link,
                            Title = link.Title,
                        };
                        db.DownloadLinks.Add(downloadLink);
                        db.SaveChanges();

                    }
                    return true;

                }
                return false;

            }
            catch
            {
                return false;
            }

        }

        public bool Delete(DownloadLink link)
        {
            try
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", link.ImageName);
                System.IO.File.Delete(imagePath);
                db.DownloadLinks.Remove(link);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public string GetDirectLink()
        {
            return db.SiteSettings.FirstOrDefault().DirectDownloadLink;
        }

        public IndexDownloadBoxViewModel GetDownloadBox()
        {
            var downloads = db.DownloadLinks.ToList();
            downloads.ApplyTranslations(db);
            IndexDownloadBoxViewModel model = new IndexDownloadBoxViewModel()
            {
                DirectLink = GetDirectLink(),
                SocialMedia = GetSocialMedia(),
                DownloadLinkGroups = GetDownloadLinkGoroups()
            };
            model.DownloadLinks = new List<DownloadLinkViewModel>();
            foreach (var download in downloads)
            {
                model.DownloadLinks.Add(new DownloadLinkViewModel()
                {
                    Title = download.Title,
                    ImageName = download.ImageName,
                    ShortDescription = download.ShortDescription,
                    Link = download.Link,
                    GroupID = download.DlgroupId
                });
            }
            return model;
        }

        public DownloadLinkGroup GetDownloadLinkGroup(int dlGroupID)
        {
            var item = db.DownloadLinkGroups.Find(dlGroupID);
            item.ApplyTranslation(db);
            return item;
        }

        public DownloadLink GetDownloadLink(int LinkID)
        {
            var item = db.DownloadLinks.Find(LinkID);
            item.ApplyTranslation(db);
            return item;
        }

        public List<DownloadLinkViewModel> GetDownloadLinks()
        {
            var model = new List<DownloadLinkViewModel>();
            var downloads = db.DownloadLinks.Include(d => d.Dlgroup).ToList();
            downloads.ApplyTranslations(db);
            foreach (var download in downloads)
            {
                download.Dlgroup.ApplyTranslation(db);
                model.Add(new DownloadLinkViewModel()
                {
                    DownloadID = download.DownloadLinkId,
                    Title = download.Title,
                    GroupID = download.DlgroupId,
                    GroupName = download.Dlgroup.GroupName,
                    ImageName = download.ImageName,
                    ShortDescription = download.ShortDescription,
                    Link = download.Link
                });
            }
            return model;
        }

        public List<FaqsItemViewModel> GetFaqsBySearch(string q = "")
        {
            List<FaqsItemViewModel> models = new List<FaqsItemViewModel>();
            var faqs = db.Faqs.Where(f => f.Answer.Contains(q) || f.Question.Contains(q)).ToList();
            faqs.ApplyTranslations(db);
            foreach (var faqsItem in faqs)
            {
                models.Add(new FaqsItemViewModel()
                {
                    Answer = faqsItem.Answer,
                    Question = faqsItem.Question,
                    FaqID = faqsItem.FaqId
                });
            }
            return models;
        }

        public SocialMedium GetMedia(int MediaID)
        {
            var item = db.SocialMedia.Find(MediaID);
            item.ApplyTranslation(db);
            return item;
        }

        public SearchPageDataViewModel GetSearchContent(string q = "")
        {
            SearchPageDataViewModel model = new SearchPageDataViewModel();
            model.Faqs = new List<FaqsItemViewModel>();
            var faqs = db.Faqs.Where(f => f.Question.Contains(q) || f.Answer.Contains(q)).ToList();
            faqs.ApplyTranslations(db);
            foreach (var item in faqs)
            {
                model.Faqs.Add(new FaqsItemViewModel()
                {
                    FaqID = item.FaqId,
                    Answer = item.Answer,
                    Question = item.Question
                });
            }

            model.Blogs = new List<BlogItemViewModel>();
            var blogs = db.Blogs.Where(b => b.Title.Contains(q)).ToList();
            blogs.ApplyTranslations(db);
            foreach (var blog in blogs)
            {
                model.Blogs.Add(new BlogItemViewModel()
                {
                    Title = blog.Title,
                    BlogID = blog.BlogId,
                    ImageName = blog.ImageName,
                    Source = blog.Source,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate),
                });
            }

            var news = db.NewsFeatures.Where(b => b.Title.Contains(q)).ToList();
            news.ApplyTranslations(db);
            foreach (var blog in news)
            {
                model.Blogs.Add(new BlogItemViewModel()
                {
                    Title = blog.Title,
                    BlogID = blog.NewsId,
                    ImageName = blog.ImageName,
                    Source = blog.Source,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate),
                });
            }

            model.Features = new List<FeaturesItemViewModel>();
            var features = db.Features.Where(f => f.Title.Contains(q)).ToList();
            features.ApplyTranslations(db);
            for (int i = 0; i < features.Count; i++)
            {
                model.Features.Add(new FeaturesItemViewModel()
                {
                    FeatureID = features[i].FeatureId,
                    ImageName = features[i].ImageName,
                    AnimateImage = features[i].AnimateFilename,
                    ShortDescription = features[i].ShortDescription,
                    Title = features[i].Title,
                    Demo = features[i].Title.Replace(" ", "-"),
                    IsBig = BigFeatureChecker.Check(i + 1)

                });
            }
            return model;
        }

        public SiteSetting GetSiteSetting()
        {
            var item = db.SiteSettings.FirstOrDefault();
            item.ApplyTranslation(db);
            return item;
        }
        public DownloadLinkCrudViewModel GetDownloadLinkModelForCreate()
        {
            DownloadLinkCrudViewModel model = new DownloadLinkCrudViewModel();
            model.Groups = new List<DownloadLinkGroupViewModel>();
            var downloadGroups = db.DownloadLinkGroups.ToList();
            downloadGroups.ApplyTranslations(db);
            foreach (var group in downloadGroups)
            {
                model.Groups.Add(new DownloadLinkGroupViewModel()
                {
                    GroupID = group.DlgroupId,
                    GroupName = group.GroupName
                });
            }
            return model;
        }

        public DownloadLinkCrudViewModel GetDownloadLinkModelForUpdate(int linkID)
        {
            var link = GetDownloadLink(linkID);
            DownloadLinkCrudViewModel model = new DownloadLinkCrudViewModel()
            {
                ImageName = link.ImageName,
                Link = link.Link,
                ShortDescription = link.ShortDescription,
                Title = link.Title,
                DownloadID = link.DownloadLinkId,
            };
            model.SelectedGroups = new List<int>();
            model.SelectedGroups.Add(link.DlgroupId);
            model.Groups = new List<DownloadLinkGroupViewModel>();
            var downloadGroups = db.DownloadLinkGroups.ToList();
            downloadGroups.ApplyTranslations(db);
            foreach (var group in downloadGroups)
            {
                model.Groups.Add(new DownloadLinkGroupViewModel()
                {
                    GroupID = group.DlgroupId,
                    GroupName = group.GroupName
                });
            }
            return model;
        }

        public List<SocialMediaItemViewModel> GetSocialMedia()
        {
            var content = db.SocialMedia.ToList();
            content.ApplyTranslations(db);
            List<SocialMediaItemViewModel> model = new List<SocialMediaItemViewModel>();
            foreach (var item in content)
            {
                model.Add(new SocialMediaItemViewModel()
                {
                    MediaID = item.SocialMediaId,
                    Title = item.Title,
                    ClassName = item.ClassName,
                    ImageName = item.ImageName,
                    Link = item.Link,
                    ColorName = item.ColorCode
                });
            }
            return model;
        }

        public void PlusVisit()
        {
            SiteVisit siteVisit = new SiteVisit() { CreateDate = DateTime.Now };
            db.SiteVisits.Add(siteVisit);
            db.SaveChanges();
        }

        public string SubmitMessage(string fullname, string Email, string Phonenumber, string Subject, string Text)
        {
            try
            {
                FeedBack feedBack = new FeedBack()
                {
                    Email = Email,
                    Fullname = fullname,
                    Subject = Subject,
                    Text = Text,
                    IsShow = false,
                    CreateDate = DateTime.Now
                };
                if (CheckMessage(feedBack) == "موفق")
                {
                    db.FeedBacks.Add(feedBack);
                    db.SaveChanges();
                    return $"{fullname} عزیز ! پیام شما با موفقیت ارسال شد";
                }
                else
                {
                    return CheckMessage(feedBack);
                }
            }
            catch
            {
                return $"{fullname} عزیز ! هنگام ارسال پیام خطایی رخ داد لطفا مجددا تلاش فرمایید";

            }
        }

        public bool Update(DownloadLinkCrudViewModel link, IFormFile ImageName)
        {
            try
            {
                if (link.SelectedGroups.Any())
                {
                    if (ImageName != null)
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", link.ImageName);

                        System.IO.File.Delete(imagePath);

                        link.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", link.ImageName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            ImageName.CopyTo(stream);
                        }

                    }
                    var downloadLink = GetDownloadLink(link.DownloadID);
                    downloadLink.Title = link.Title;
                    downloadLink.ShortDescription = link.ShortDescription;
                    downloadLink.ImageName = link.ImageName;
                    downloadLink.Link = link.Link;

                    if (link.SelectedGroups.Count == 1)
                    {
                        downloadLink.DlgroupId = link.SelectedGroups.FirstOrDefault();
                        db.DownloadLinks.Update(downloadLink);
                        db.SaveChanges();
                    }
                    else
                    {
                        foreach (var group in link.SelectedGroups)
                        {
                            if (group == downloadLink.DlgroupId)
                            {
                                db.DownloadLinks.Update(downloadLink);
                                db.SaveChanges();
                            }
                            else
                            {
                                var newDownloadLink = new DownloadLink()
                                {
                                    Title = link.Title,
                                    ShortDescription = link.ShortDescription,
                                    ImageName = link.ImageName,
                                    Link = link.Link,
                                    DlgroupId = group
                                };
                                db.DownloadLinks.Add(newDownloadLink);
                                db.SaveChanges();
                            }
                            
                        }
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateMedia(SocialMedium social)
        {
            try
            {

                db.SocialMedia.Update(social);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateMedia(SocialMedium social)
        {
            try
            {

                db.SocialMedia.Add(social);
                db.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMedia(SocialMedium social)
        {
            try
            {
                db.SocialMedia.Remove(social);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateSite(SiteSetting siteSetting, IFormFile fileName, IFormFile imageProduct)
        {
            try
            {
                if (fileName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", siteSetting.DirectDownloadLink);

                    System.IO.File.Delete(imagePath);

                    siteSetting.DirectDownloadLink = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(fileName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", siteSetting.DirectDownloadLink);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        fileName.CopyTo(stream);
                    }
                }
                if (imageProduct != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", siteSetting.BlogBannerImage);

                    System.IO.File.Delete(imagePath);

                    siteSetting.BlogBannerImage = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imageProduct.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", siteSetting.BlogBannerImage);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        imageProduct.CopyTo(stream);
                    }

                }
                db.SiteSettings.Update(siteSetting);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ContentItemViewModel GetIndexAbout()
        {
            var items = db.IndexContents.FirstOrDefault();

            ContentItemViewModel model = new ContentItemViewModel()
            {
                Title = items.AboutTitle,
                Description = items.AboutDescription,
                SubTitle = items.AboutSubTitle
            };
            return model;
        }

        public ContentItemViewModel GetIndexFeature()
        {
            var items = db.IndexContents.FirstOrDefault();

            ContentItemViewModel model = new ContentItemViewModel()
            {
                Title = items.FeatureTitle,
                Description = items.FeatureDescription,
                SubTitle = items.FeatureSubTitle
            };
            return model;
        }

        public ContentItemViewModel GetIndexFaq()
        {
            var items = db.IndexContents.FirstOrDefault();

            ContentItemViewModel model = new ContentItemViewModel()
            {
                Title = items.FaqTitle,
                Description = items.FaqDescription,
                SubTitle = items.FaqSubTitle
            };
            return model;
        }

        public void AddDownloadCount()
        {
            var setting = db.SiteSettings.FirstOrDefault();
            setting.DownloadCount += 1;
            db.SiteSettings.Update(setting);
            db.SaveChanges();
        }

        public AboutUsContent GetAboutUsContent()
        {
            var item = db.AboutUsContents.FirstOrDefault();
            item.ApplyTranslation(db);
            return item;
        }

        public AboutUsContentCrudViewModel GetAboutUsContentForEdit()
        {
            var item = db.AboutUsContents.FirstOrDefault();
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(AboutUsContent) && t.EntityId == item.AboutUsContentId).ToList();
            return new AboutUsContentCrudViewModel
            {
                AboutUsContentId = item.AboutUsContentId,
                SubTitle = item.SubTitle,
                Title = item.Title,
                Description = item.Description,
                ImageName = item.ImageName,
                SecondSubTitle = item.SecondSubTitle,
                SecondTitle = item.SecondTitle,
                SecondDescription = item.SecondDescription,
                ThirdSubTitle = item.ThirdSubTitle,
                ThirdTitle = item.ThirdTitle,
                DownloadImageName = item.DownloadImageName,
                ForthSubTitle = item.ForthSubTitle,
                ForthTitle = item.ForthTitle,
                ForthDescription = item.ForthDescription,
                LogoTitle = item.LogoTitle,
                LogoSubTitle = item.LogoSubTitle,
                LogoDescription = item.LogoDescription,
                SubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SubTitle) && t.Culture == "en")?.Value,
                SubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SubTitle) && t.Culture == "ar")?.Value,
                SubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SubTitle) && t.Culture == "ur")?.Value,
                TitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.Title) && t.Culture == "en")?.Value,
                TitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.Title) && t.Culture == "ar")?.Value,
                TitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.Title) && t.Culture == "ur")?.Value,
                DescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.Description) && t.Culture == "en")?.Value,
                DescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.Description) && t.Culture == "ar")?.Value,
                DescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.Description) && t.Culture == "ur")?.Value,
                SecondSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondSubTitle) && t.Culture == "en")?.Value,
                SecondSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondSubTitle) && t.Culture == "ar")?.Value,
                SecondSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondSubTitle) && t.Culture == "ur")?.Value,
                SecondTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondTitle) && t.Culture == "en")?.Value,
                SecondTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondTitle) && t.Culture == "ar")?.Value,
                SecondTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondTitle) && t.Culture == "ur")?.Value,
                SecondDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondDescription) && t.Culture == "en")?.Value,
                SecondDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondDescription) && t.Culture == "ar")?.Value,
                SecondDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.SecondDescription) && t.Culture == "ur")?.Value,
                ThirdSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ThirdSubTitle) && t.Culture == "en")?.Value,
                ThirdSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ThirdSubTitle) && t.Culture == "ar")?.Value,
                ThirdSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ThirdSubTitle) && t.Culture == "ur")?.Value,
                ThirdTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ThirdTitle) && t.Culture == "en")?.Value,
                ThirdTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ThirdTitle) && t.Culture == "ar")?.Value,
                ThirdTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ThirdTitle) && t.Culture == "ur")?.Value,
                ForthSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthSubTitle) && t.Culture == "en")?.Value,
                ForthSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthSubTitle) && t.Culture == "ar")?.Value,
                ForthSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthSubTitle) && t.Culture == "ur")?.Value,
                ForthTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthTitle) && t.Culture == "en")?.Value,
                ForthTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthTitle) && t.Culture == "ar")?.Value,
                ForthTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthTitle) && t.Culture == "ur")?.Value,
                ForthDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthDescription) && t.Culture == "en")?.Value,
                ForthDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthDescription) && t.Culture == "ar")?.Value,
                ForthDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.ForthDescription) && t.Culture == "ur")?.Value,
                LogoTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoTitle) && t.Culture == "en")?.Value,
                LogoTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoTitle) && t.Culture == "ar")?.Value,
                LogoTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoTitle) && t.Culture == "ur")?.Value,
                LogoSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoSubTitle) && t.Culture == "en")?.Value,
                LogoSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoSubTitle) && t.Culture == "ar")?.Value,
                LogoSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoSubTitle) && t.Culture == "ur")?.Value,
                LogoDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoDescription) && t.Culture == "en")?.Value,
                LogoDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoDescription) && t.Culture == "ar")?.Value,
                LogoDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(AboutUsContent.LogoDescription) && t.Culture == "ur")?.Value
            };
        }

        public List<AboutUsSight> GetAboutUsSights()
        {
            var items = db.AboutUsSights.ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public List<AboutUsArticle> GetAboutUsArticles()
        {
            var items = db.AboutUsArticles.ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public List<AboutUsItem> GetAboutUsItems()
        {
            var items = db.AboutUsItems.ToList();
            items.ApplyTranslations(db);
            return items;
        }

        public AboutUsLogoImagesViewModel GetAboutUsLogoImages()
        {
            var content = db.AboutUsLogoes.ToList();
            content.ApplyTranslations(db);
            AboutUsLogoImagesViewModel model = new AboutUsLogoImagesViewModel();
            model.NormalPics = new List<AboutUsLogoItemViewModel>();
            model.MainPics = new List<AboutUsLogoItemViewModel>();
            foreach (var item in content)
            {
                if (item.IsMain)
                {
                    model.MainPics.Add(new AboutUsLogoItemViewModel() { ImageName = item.ImageName });
                }
                else
                {
                    model.NormalPics.Add(new AboutUsLogoItemViewModel() { ImageName = item.ImageName });
                }
            }
            return model;
        }

        public ContentItemViewModel GetContactUsSecondDescription()
        {
            var content = db.ContactUsContents.FirstOrDefault();
            content.ApplyTranslation(db);
            ContentItemViewModel model = new ContentItemViewModel()
            {
                Title = content.SecondTitle,
                SubTitle = content.SecondSubTitle,
                Description = content.SecondDescription
            };
            return model;
        }

        public ContentItemViewModel GetContactUsFirstDescription()
        {
            var content = db.ContactUsContents.FirstOrDefault();
            content.ApplyTranslation(db);
            ContentItemViewModel model = new ContentItemViewModel()
            {
                Title = content.FirstTitle,
                SubTitle = content.FirstSubTitle,
                Description = content.FristDescription
            };
            return model;
        }

        #region Contents
        public IndexContentCrudViewModel GetIndexContent()
        {
            var item = db.IndexContents.FirstOrDefault();
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(IndexContent) && t.EntityId == item.IndexContentId).ToList();
            return new IndexContentCrudViewModel
            {
                IndexContentId = item.IndexContentId,
                AboutTitle = item.AboutTitle,
                AboutSubTitle = item.AboutSubTitle,
                AboutDescription = item.AboutDescription,
                FeatureTitle = item.FeatureTitle,
                FeatureSubTitle = item.FeatureSubTitle,
                FeatureDescription = item.FeatureDescription,
                FaqTitle = item.FaqTitle,
                FaqSubTitle = item.FaqSubTitle,
                FaqDescription = item.FaqDescription,
                AboutTitleEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutTitle) && t.Culture == "en")?.Value,
                AboutTitleAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutTitle) && t.Culture == "ar")?.Value,
                AboutTitleUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutTitle) && t.Culture == "ur")?.Value,
                AboutSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutSubTitle) && t.Culture == "en")?.Value,
                AboutSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutSubTitle) && t.Culture == "ar")?.Value,
                AboutSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutSubTitle) && t.Culture == "ur")?.Value,
                AboutDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutDescription) && t.Culture == "en")?.Value,
                AboutDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutDescription) && t.Culture == "ar")?.Value,
                AboutDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.AboutDescription) && t.Culture == "ur")?.Value,
                FeatureTitleEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureTitle) && t.Culture == "en")?.Value,
                FeatureTitleAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureTitle) && t.Culture == "ar")?.Value,
                FeatureTitleUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureTitle) && t.Culture == "ur")?.Value,
                FeatureSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureSubTitle) && t.Culture == "en")?.Value,
                FeatureSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureSubTitle) && t.Culture == "ar")?.Value,
                FeatureSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureSubTitle) && t.Culture == "ur")?.Value,
                FeatureDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureDescription) && t.Culture == "en")?.Value,
                FeatureDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureDescription) && t.Culture == "ar")?.Value,
                FeatureDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FeatureDescription) && t.Culture == "ur")?.Value,
                FaqTitleEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqTitle) && t.Culture == "en")?.Value,
                FaqTitleAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqTitle) && t.Culture == "ar")?.Value,
                FaqTitleUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqTitle) && t.Culture == "ur")?.Value,
                FaqSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqSubTitle) && t.Culture == "en")?.Value,
                FaqSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqSubTitle) && t.Culture == "ar")?.Value,
                FaqSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqSubTitle) && t.Culture == "ur")?.Value,
                FaqDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqDescription) && t.Culture == "en")?.Value,
                FaqDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqDescription) && t.Culture == "ar")?.Value,
                FaqDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(IndexContent.FaqDescription) && t.Culture == "ur")?.Value
            };
        }

        public bool UpdateIndexContent(IndexContentCrudViewModel content)
        {
            try
            {
                var entity = db.IndexContents.Find(content.IndexContentId);
                entity.AboutTitle = content.AboutTitle;
                entity.AboutSubTitle = content.AboutSubTitle;
                entity.AboutDescription = content.AboutDescription;
                entity.FeatureTitle = content.FeatureTitle;
                entity.FeatureSubTitle = content.FeatureSubTitle;
                entity.FeatureDescription = content.FeatureDescription;
                entity.FaqTitle = content.FaqTitle;
                entity.FaqSubTitle = content.FaqSubTitle;
                entity.FaqDescription = content.FaqDescription;
                db.IndexContents.Update(entity);
                db.SaveChanges();
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutTitle), "en", content.AboutTitleEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutTitle), "ar", content.AboutTitleAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutTitle), "ur", content.AboutTitleUr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutSubTitle), "en", content.AboutSubTitleEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutSubTitle), "ar", content.AboutSubTitleAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutSubTitle), "ur", content.AboutSubTitleUr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutDescription), "en", content.AboutDescriptionEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutDescription), "ar", content.AboutDescriptionAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.AboutDescription), "ur", content.AboutDescriptionUr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureTitle), "en", content.FeatureTitleEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureTitle), "ar", content.FeatureTitleAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureTitle), "ur", content.FeatureTitleUr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureSubTitle), "en", content.FeatureSubTitleEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureSubTitle), "ar", content.FeatureSubTitleAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureSubTitle), "ur", content.FeatureSubTitleUr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureDescription), "en", content.FeatureDescriptionEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureDescription), "ar", content.FeatureDescriptionAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FeatureDescription), "ur", content.FeatureDescriptionUr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqTitle), "en", content.FaqTitleEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqTitle), "ar", content.FaqTitleAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqTitle), "ur", content.FaqTitleUr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqSubTitle), "en", content.FaqSubTitleEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqSubTitle), "ar", content.FaqSubTitleAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqSubTitle), "ur", content.FaqSubTitleUr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqDescription), "en", content.FaqDescriptionEn);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqDescription), "ar", content.FaqDescriptionAr);
                SaveTranslation(nameof(IndexContent), entity.IndexContentId, nameof(IndexContent.FaqDescription), "ur", content.FaqDescriptionUr);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool UpdateAboutUsContent(AboutUsContentCrudViewModel content, IFormFile imageProduct, IFormFile downloadProduct)
        {
            try
            {
                var entity = db.AboutUsContents.Find(content.AboutUsContentId);
                entity.SubTitle = content.SubTitle;
                entity.Title = content.Title;
                entity.Description = content.Description;
                entity.SecondSubTitle = content.SecondSubTitle;
                entity.SecondTitle = content.SecondTitle;
                entity.SecondDescription = content.SecondDescription;
                entity.ThirdSubTitle = content.ThirdSubTitle;
                entity.ThirdTitle = content.ThirdTitle;
                entity.ForthSubTitle = content.ForthSubTitle;
                entity.ForthTitle = content.ForthTitle;
                entity.ForthDescription = content.ForthDescription;
                entity.LogoTitle = content.LogoTitle;
                entity.LogoSubTitle = content.LogoSubTitle;
                entity.LogoDescription = content.LogoDescription;
                if (imageProduct != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.ImageName);
                    System.IO.File.Delete(imagePath);
                    entity.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imageProduct.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        imageProduct.CopyTo(stream);
                    }
                }
                if (downloadProduct != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.DownloadImageName);
                    System.IO.File.Delete(imagePath);
                    entity.DownloadImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(downloadProduct.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", entity.DownloadImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        downloadProduct.CopyTo(stream);
                    }
                }
                db.AboutUsContents.Update(entity);
                db.SaveChanges();
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SubTitle), "en", content.SubTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SubTitle), "ar", content.SubTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SubTitle), "ur", content.SubTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.Title), "en", content.TitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.Title), "ar", content.TitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.Title), "ur", content.TitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.Description), "en", content.DescriptionEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.Description), "ar", content.DescriptionAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.Description), "ur", content.DescriptionUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondSubTitle), "en", content.SecondSubTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondSubTitle), "ar", content.SecondSubTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondSubTitle), "ur", content.SecondSubTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondTitle), "en", content.SecondTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondTitle), "ar", content.SecondTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondTitle), "ur", content.SecondTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondDescription), "en", content.SecondDescriptionEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondDescription), "ar", content.SecondDescriptionAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.SecondDescription), "ur", content.SecondDescriptionUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ThirdSubTitle), "en", content.ThirdSubTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ThirdSubTitle), "ar", content.ThirdSubTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ThirdSubTitle), "ur", content.ThirdSubTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ThirdTitle), "en", content.ThirdTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ThirdTitle), "ar", content.ThirdTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ThirdTitle), "ur", content.ThirdTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthSubTitle), "en", content.ForthSubTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthSubTitle), "ar", content.ForthSubTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthSubTitle), "ur", content.ForthSubTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthTitle), "en", content.ForthTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthTitle), "ar", content.ForthTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthTitle), "ur", content.ForthTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthDescription), "en", content.ForthDescriptionEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthDescription), "ar", content.ForthDescriptionAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.ForthDescription), "ur", content.ForthDescriptionUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoTitle), "en", content.LogoTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoTitle), "ar", content.LogoTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoTitle), "ur", content.LogoTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoSubTitle), "en", content.LogoSubTitleEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoSubTitle), "ar", content.LogoSubTitleAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoSubTitle), "ur", content.LogoSubTitleUr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoDescription), "en", content.LogoDescriptionEn);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoDescription), "ar", content.LogoDescriptionAr);
                SaveTranslation(nameof(AboutUsContent), entity.AboutUsContentId, nameof(AboutUsContent.LogoDescription), "ur", content.LogoDescriptionUr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateContactUsContent(ContactUsContentCrudViewModel content)
        {
            try
            {
                var entity = db.ContactUsContents.Find(content.ContactUsContentId);
                entity.FirstSubTitle = content.FirstSubTitle;
                entity.FirstTitle = content.FirstTitle;
                entity.FristDescription = content.FristDescription;
                entity.SecondSubTitle = content.SecondSubTitle;
                entity.SecondTitle = content.SecondTitle;
                entity.SecondDescription = content.SecondDescription;
                db.ContactUsContents.Update(entity);
                db.SaveChanges();
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FirstSubTitle), "en", content.FirstSubTitleEn);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FirstSubTitle), "ar", content.FirstSubTitleAr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FirstSubTitle), "ur", content.FirstSubTitleUr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FirstTitle), "en", content.FirstTitleEn);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FirstTitle), "ar", content.FirstTitleAr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FirstTitle), "ur", content.FirstTitleUr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FristDescription), "en", content.FristDescriptionEn);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FristDescription), "ar", content.FristDescriptionAr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.FristDescription), "ur", content.FristDescriptionUr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondSubTitle), "en", content.SecondSubTitleEn);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondSubTitle), "ar", content.SecondSubTitleAr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondSubTitle), "ur", content.SecondSubTitleUr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondTitle), "en", content.SecondTitleEn);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondTitle), "ar", content.SecondTitleAr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondTitle), "ur", content.SecondTitleUr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondDescription), "en", content.SecondDescriptionEn);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondDescription), "ar", content.SecondDescriptionAr);
                SaveTranslation(nameof(ContactUsContent), entity.ContactUsContentId, nameof(ContactUsContent.SecondDescription), "ur", content.SecondDescriptionUr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ContactUsContentCrudViewModel GetContactUsContent()
        {
            var item = db.ContactUsContents.FirstOrDefault();
            var translations = db.EntityTranslations.Where(t => t.EntityName == nameof(ContactUsContent) && t.EntityId == item.ContactUsContentId).ToList();
            return new ContactUsContentCrudViewModel
            {
                ContactUsContentId = item.ContactUsContentId,
                FirstSubTitle = item.FirstSubTitle,
                FirstTitle = item.FirstTitle,
                FristDescription = item.FristDescription,
                SecondSubTitle = item.SecondSubTitle,
                SecondTitle = item.SecondTitle,
                SecondDescription = item.SecondDescription,
                FirstSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FirstSubTitle) && t.Culture == "en")?.Value,
                FirstSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FirstSubTitle) && t.Culture == "ar")?.Value,
                FirstSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FirstSubTitle) && t.Culture == "ur")?.Value,
                FirstTitleEn = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FirstTitle) && t.Culture == "en")?.Value,
                FirstTitleAr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FirstTitle) && t.Culture == "ar")?.Value,
                FirstTitleUr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FirstTitle) && t.Culture == "ur")?.Value,
                FristDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FristDescription) && t.Culture == "en")?.Value,
                FristDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FristDescription) && t.Culture == "ar")?.Value,
                FristDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.FristDescription) && t.Culture == "ur")?.Value,
                SecondSubTitleEn = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondSubTitle) && t.Culture == "en")?.Value,
                SecondSubTitleAr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondSubTitle) && t.Culture == "ar")?.Value,
                SecondSubTitleUr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondSubTitle) && t.Culture == "ur")?.Value,
                SecondTitleEn = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondTitle) && t.Culture == "en")?.Value,
                SecondTitleAr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondTitle) && t.Culture == "ar")?.Value,
                SecondTitleUr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondTitle) && t.Culture == "ur")?.Value,
                SecondDescriptionEn = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondDescription) && t.Culture == "en")?.Value,
                SecondDescriptionAr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondDescription) && t.Culture == "ar")?.Value,
                SecondDescriptionUr = translations.FirstOrDefault(t => t.Property == nameof(ContactUsContent.SecondDescription) && t.Culture == "ur")?.Value
            };
        }

        public List<DownloadLinkGroupViewModel> GetDownloadLinkGoroups()
        {
            var modelDB = db.DownloadLinkGroups.ToList();
            modelDB.ApplyTranslations(db);
            List<DownloadLinkGroupViewModel> content = new List<DownloadLinkGroupViewModel>();
            foreach (var item in modelDB)
            {
                content.Add(new DownloadLinkGroupViewModel()
                {
                    GroupID = item.DlgroupId,
                    GroupName = item.GroupName
                });
            }
            return content;
        }




        #endregion Contents

        private void SaveTranslation(string entityName, int entityId, string property, string culture, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            var tr = db.EntityTranslations.FirstOrDefault(t => t.EntityName == entityName && t.EntityId == entityId && t.Property == property && t.Culture == culture);
            if (tr == null)
            {
                tr = new EntityTranslation
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    Property = property,
                    Culture = culture
                };
                db.EntityTranslations.Add(tr);
            }
            tr.Value = value;
            db.SaveChanges();
        }
    }
}
