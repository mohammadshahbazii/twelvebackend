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
        public IndexContent GetIndexContent()
        {
            var item = db.IndexContents.FirstOrDefault();
            item.ApplyTranslation(db);
            return item;
        }

        public bool UpdateIndexContent(IndexContent content)
        {
            try
            {
                db.IndexContents.Update(content);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool UpdateAboutUsContent(AboutUsContent content, IFormFile imageProduct, IFormFile downloadProduct)
        {
            try
            {
                if (imageProduct != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", content.ImageName);

                    System.IO.File.Delete(imagePath);

                    content.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imageProduct.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", content.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        imageProduct.CopyTo(stream);
                    }

                }
                if (downloadProduct != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", content.DownloadImageName);

                    System.IO.File.Delete(imagePath);

                    content.DownloadImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(downloadProduct.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Site", content.DownloadImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        downloadProduct.CopyTo(stream);
                    }

                }
                db.AboutUsContents.Update(content);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateContactUsContent(ContactUsContent content)
        {
            try
            {
                db.ContactUsContents.Update(content);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ContactUsContent GetContactUsContent()
        {
            var item = db.ContactUsContents.FirstOrDefault();
            item.ApplyTranslation(db);
            return item;
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
    }
}
