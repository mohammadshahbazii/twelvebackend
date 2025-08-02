using Azure;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;
using ViewModels;
using System.Globalization;

namespace Services
{
    public class BlogRepository : IBlogRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        private string CurrentCulture => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        public void Dispose()
        {
            db.Dispose();
        }

        public List<NewsComment> GetNewsComments(int BlogID)
        {
            return db.NewsComments.Where(b => b.NewsId == BlogID && b.IsConfirm == true).OrderByDescending(b => b.CreateDate).ToList();
        }

        public BlogPostCommentsViewModel GetBlogComments(int BlogID , int PageID = 1)
        {
            BlogPostCommentsViewModel model = new BlogPostCommentsViewModel();
            int take = 6;
            int skip = (PageID - 1) * take;
            model.Comments = new List<BlogCommentViewModel>();
            var Headcomments = db.BlogComments.Where(b => b.BlogId == BlogID && b.IsConfirm == true && b.ParentId == null).OrderByDescending(b => b.CreateDate).Skip(skip).Take(take).ToList();
            foreach (var item in Headcomments)
            {
                model.Comments.Add(new BlogCommentViewModel()
                {
                    IsAnswer = item.IsAnswer,
                    BlogID = item.BlogId,
                    CommentID = item.CommentId,
                    CommentText = item.Text,
                    CreateDate = DateConvertor.PassDays(item.CreateDate),
                    Fullname = item.Fullname,
                    IsConfirm = item.IsConfirm,
                    BlogTitle = item.Title,
                    ParentID = item.ParentId
                });
                if (item.IsAnswer)
                {
                    var subComments = db.BlogComments.Where(s => s.ParentId == item.CommentId).ToList();
                    foreach (var sub in subComments)
                    {
                        model.Comments.Add(new BlogCommentViewModel()
                        {
                            IsAnswer = sub.IsAnswer,
                            BlogID = sub.BlogId,
                            CommentID = sub.CommentId,
                            CommentText = sub.Text,
                            CreateDate = DateConvertor.PassDays(sub.CreateDate),
                            Fullname = sub.Fullname,
                            IsConfirm = sub.IsConfirm,
                            BlogTitle = sub.Title,
                            ParentID = sub.ParentId
                        });
                    }
                }
            }
            var CommentsCount = db.BlogComments.Where(b => b.BlogId == BlogID && b.IsConfirm == true).ToList().Count();
            double pCount = Convert.ToDouble(Convert.ToDouble(CommentsCount) / Convert.ToDouble(take));
            model.CommentCount = CommentsCount;
            model.BlogID = BlogID;
            model.Pagination = GetBlogsPagination(pCount, PageID);
            model.Pagination.PageCount = Convert.ToInt32(pCount);
            return model;
        }

        public BlogPostPageDataViewModel GetNewsPageData(int BlogID)
        {
            var groupID = db.SelectedFeatureNews.FirstOrDefault(b => b.NewsId == BlogID).FeatureId;
            var item = db.NewsFeatures.Find(BlogID);
            item.PostView += 1;
            db.NewsFeatures.Update(item);
            db.SaveChanges();
            BlogPostPageDataViewModel blog = new BlogPostPageDataViewModel()
            {
                BlogID = BlogID,
                CreateDate = DateConvertor.GetPersianDate(item.CreateDate),
                Description = item.Description,
                StudyTime = item.StudyTime + " دقیقه ",
                ImageName = item.ImageName,
                ShortDescription = item.ShortDescription,
                Title = item.Title,
                GroupID = groupID,
                GroupName = db.Features.Find(groupID).Title
            };
            return blog;
        }


        public BlogPostPageDataViewModel GetBlogPageData(int BlogID)
        {
            var groupID = db.SelectedBlogGroups.FirstOrDefault(b => b.BlogId == BlogID).BlogGroupId;
            var item = db.Blogs.Find(BlogID);
            item.PostView += 1;
            db.Blogs.Update(item);
            db.SaveChanges();
            var tr = db.BlogTranslations.FirstOrDefault(t => t.BlogId == BlogID && t.Language == CurrentCulture);
            BlogPostPageDataViewModel blog = new BlogPostPageDataViewModel()
            {
                BlogID = BlogID,
                CreateDate = DateConvertor.GetPersianDate(item.CreateDate),
                Description = tr?.Description ?? item.Description,
                StudyTime = item.StudyTime + " دقیقه ",
                ImageName = item.ImageName,
                ShortDescription = tr?.ShortDescription ?? item.ShortDescription,
                Title = tr?.Title ?? item.Title,
                GroupID = groupID,
                GroupName = db.BlogGroups.Find(groupID).GroupName
            };
            return blog;
        }

        public BlogPagesDataViewModel GetGroupPosts(string q = "", int PageID = 1)
        {
            if (string.IsNullOrEmpty(q))
            {
                BlogPagesDataViewModel model = new BlogPagesDataViewModel();
                model.Groups = new List<BlogGroupsItemViewModel>();
                model.Blogs = new List<BlogGroupsPostsPaginationViewModel>();
                int take = 6;
                int skip = (PageID - 1) * take;

                



                var groups = db.BlogGroups.ToList();
                foreach (var group in groups)
                {
                    model.Groups.Add(new BlogGroupsItemViewModel()
                    {
                        GroupID = group.BlogGroupId,
                        GroupName = group.GroupName
                    });
                    model.Blogs.Add(new BlogGroupsPostsPaginationViewModel()
                    {
                        GroupID = group.BlogGroupId
                    });
                }
                model.Groups.Add(new BlogGroupsItemViewModel()
                {
                    GroupID = 0,
                    GroupName = "منوهای اپلیکیشن"
                });
                foreach (var group in model.Blogs)
                {
                    var blogs = db.SelectedBlogGroups.Where(b => b.BlogGroupId == group.GroupID).Skip(skip).Take(take).Include(b => b.Blog).Select(b => b.Blog)
                        .OrderByDescending(b => b.CreateDate).ToList();
                    var posts = new List<BlogItemViewModel>();
                    foreach (var blog in blogs)
                    {
                        posts.Add(new BlogItemViewModel()
                        {
                            GroupID = group.GroupID,
                            BlogID = blog.BlogId,
                            CreateDate = DateConvertor.PassDays(blog.CreateDate),
                            ImageName = blog.ImageName,
                            Source = blog.Source,
                            Title = blog.Title
                        });
                    }
                    group.Posts = posts;
                    double pCount = Convert.ToDouble(Convert.ToDouble(db.SelectedBlogGroups.Where(s => s.BlogGroupId == group.GroupID).ToList().Count()) / Convert.ToDouble(take));
                    group.Pagination = GetBlogsPagination(pCount, PageID);
                    group.Pagination.PageCount = Convert.ToInt32(pCount);
                }


                var appNews = db.NewsFeatures.OrderByDescending(n => n.CreateDate).Skip(skip).Take(take).ToList();
                List<BlogItemViewModel> newsApp = new List<BlogItemViewModel>();
                foreach (var blog in appNews)
                {
                    newsApp.Add(new BlogItemViewModel()
                    {
                        BlogID = blog.NewsId,
                        CreateDate = DateConvertor.PassDays(blog.CreateDate),
                        ImageName = blog.ImageName,
                        Title = blog.Title,
                        Source = blog.Source
                    });
                }
                double pageCount = Convert.ToDouble(Convert.ToDouble(db.NewsFeatures.OrderByDescending(n => n.CreateDate).ToList().Count()) / Convert.ToDouble(take));
                model.Blogs.Add(new BlogGroupsPostsPaginationViewModel() { GroupID = 0, Posts = newsApp, Pagination = GetBlogsPagination(pageCount, PageID) });


                return model;
            }
            else
            {
                BlogPagesDataViewModel model = new BlogPagesDataViewModel();
                model.Groups = new List<BlogGroupsItemViewModel>();
                model.Blogs = new List<BlogGroupsPostsPaginationViewModel>();
                int take = 6;
                int skip = (PageID - 1) * take;

                



                var groups = db.BlogGroups.ToList();
                foreach (var group in groups)
                {
                    model.Groups.Add(new BlogGroupsItemViewModel()
                    {
                        GroupID = group.BlogGroupId,
                        GroupName = group.GroupName
                    });
                    model.Blogs.Add(new BlogGroupsPostsPaginationViewModel()
                    {
                        GroupID = group.BlogGroupId
                    });
                }
                model.Groups.Add(new BlogGroupsItemViewModel()
                {
                    GroupID = 0,
                    GroupName = "منوهای اپلیکیشن"
                });
                foreach (var group in model.Blogs)
                {
                    var blogs = db.SelectedBlogGroups.Include(b => b.Blog).Where(b => b.BlogGroupId == group.GroupID && b.Blog.Title.Contains(q)).Skip(skip).Take(take).Include(b => b.Blog).Select(b => b.Blog)
                        .OrderByDescending(b => b.CreateDate).ToList();
                    var posts = new List<BlogItemViewModel>();
                    foreach (var blog in blogs)
                    {
                        posts.Add(new BlogItemViewModel()
                        {
                            GroupID = group.GroupID,
                            BlogID = blog.BlogId,
                            CreateDate = DateConvertor.PassDays(blog.CreateDate),
                            ImageName = blog.ImageName,
                            Source = blog.Source,
                            Title = blog.Title
                        });
                    }
                    group.Posts = posts;
                    double pCount = Convert.ToDouble(Convert.ToDouble(db.SelectedBlogGroups.Include(b => b.Blog).Where(b => b.BlogGroupId == group.GroupID && b.Blog.Title.Contains(q)).ToList().Count()) / Convert.ToDouble(take));
                    group.Pagination = GetBlogsPagination(pCount, PageID);
                    group.Pagination.PageCount = Convert.ToInt32(pCount);
                }


                var appNews = db.NewsFeatures.Where(n => n.Title.Contains(q)).OrderByDescending(n => n.CreateDate).Skip(skip).Take(take).ToList();
                List<BlogItemViewModel> newsApp = new List<BlogItemViewModel>();
                foreach (var blog in appNews)
                {
                    newsApp.Add(new BlogItemViewModel()
                    {
                        BlogID = blog.NewsId,
                        CreateDate = DateConvertor.PassDays(blog.CreateDate),
                        ImageName = blog.ImageName,
                        Title = blog.Title,
                        Source = blog.Source
                    });
                }
                double pageCount = Convert.ToDouble(Convert.ToDouble(db.NewsFeatures.Where(n => n.Title.Contains(q)).OrderByDescending(n => n.CreateDate).ToList().Count()) / Convert.ToDouble(take));
                model.Blogs.Add(new BlogGroupsPostsPaginationViewModel() { GroupID = 0, Posts = newsApp, Pagination = GetBlogsPagination(pageCount, PageID) });


                return model;
            }
        }
        public List<string> GetNewsTags(int BlogID)
        {
            var items = db.NewsTags.Where(b => b.NewsId == BlogID).Select(b => b.Tag).ToList();
            var views = db.NewsFeatures.Find(BlogID).PostView + " بازدید ";
            items.Add(views);
            return items;

        }
        public List<string> GetBlogTags(int BlogID)
        {
            var items = db.BlogTags.Where(b => b.BlogId == BlogID).Select(b => b.Tag).ToList();
            var views = db.Blogs.Find(BlogID).PostView + " بازدید ";
            items.Add(views);
            return items;

        }

        public IndexBlogsViewModel GetIndexBlogs()
        {

            IndexBlogsViewModel model = new IndexBlogsViewModel();
            model.Blogs = new List<BlogItemViewModel>();
            var blogs = db.Blogs.OrderByDescending(n => n.CreateDate).Take(3).ToList();
            foreach (var blog in blogs)
            {
                var tr = db.BlogTranslations.FirstOrDefault(t => t.BlogId == blog.BlogId && t.Language == CurrentCulture);
                model.Blogs.Add(new BlogItemViewModel()
                {
                    BlogID = blog.BlogId,
                    Title = tr?.Title ?? blog.Title,
                    ImageName = blog.ImageName,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate),
                    Source = blog.Source
                });
            }

            model.News = new List<BlogItemViewModel>();
            var news = db.NewsFeatures.OrderByDescending(n => n.CreateDate).Take(3).ToList();
            foreach (var blog in news)
            {
                model.News.Add(new BlogItemViewModel()
                {
                    BlogID = blog.NewsId,
                    Title = blog.Title,
                    ImageName = blog.ImageName,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate),
                    Source = blog.Source

                });
            }
            return model;
        }

        public List<BlogItemViewModel> GetRelatedPosts(int BlogID)
        {
            var items = db.SelectedRelatedBlogs.Where(b => b.MainBlogId == BlogID).Include(b => b.RelatedBlog).Select(b => b.RelatedBlog).ToList();
            List<BlogItemViewModel> model = new List<BlogItemViewModel>();
            foreach (var blog in items)
            {
                var tr = db.BlogTranslations.FirstOrDefault(t => t.BlogId == blog.BlogId && t.Language == CurrentCulture);
                model.Add(new BlogItemViewModel()
                {
                    BlogID = blog.BlogId,
                    Title = tr?.Title ?? blog.Title,
                    ImageName = blog.ImageName,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate),
                    Source = blog.Source,
                });
            }
            return model;
        }

        public List<BlogItemViewModel> LatestNews()
        {
            var items = db.NewsFeatures.OrderByDescending(b => b.CreateDate).Take(4).ToList();
            List<BlogItemViewModel> model = new List<BlogItemViewModel>();
            foreach (var blog in items)
            {
                int groupID = db.SelectedFeatureNews.FirstOrDefault(g => g.NewsId == blog.NewsId).FeatureId;
                model.Add(new BlogItemViewModel()
                {
                    ImageName = blog.ImageName,
                    Title = blog.Title,
                    BlogID = blog.NewsId,
                    GroupName = db.Features.Find(groupID).Title,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate)
                });
            }
            return model;
        }

        public List<IndexSliderItemViewModel> PopularNews()
        {
            var items = db.NewsFeatures.OrderByDescending(b => b.PostView).Take(4).ToList();
            List<IndexSliderItemViewModel> model = new List<IndexSliderItemViewModel>();
            foreach (var blog in items)
            {
                model.Add(new IndexSliderItemViewModel()
                {
                    Title = blog.Title,
                    ShortDescription = blog.Title.Replace(" ", "-"),
                    ImageName = blog.ImageName,
                    SliderID = blog.NewsId
                });
            }
            return model;
        }

        public List<BlogItemViewModel> LatestBlogs()
        {
            var items = db.Blogs.OrderByDescending(b => b.CreateDate).Take(4).ToList();
            List<BlogItemViewModel> model = new List<BlogItemViewModel>();
            foreach (var blog in items)
            {
                int groupID = db.SelectedBlogGroups.FirstOrDefault(g => g.BlogId == blog.BlogId).BlogGroupId;
                var tr = db.BlogTranslations.FirstOrDefault(t => t.BlogId == blog.BlogId && t.Language == CurrentCulture);
                model.Add(new BlogItemViewModel()
                {
                    ImageName = blog.ImageName,
                    Title = tr?.Title ?? blog.Title,
                    BlogID = blog.BlogId,
                    GroupName = db.BlogGroups.Find(groupID).GroupName,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate)
                });
            }
            return model;
        }

        public List<IndexSliderItemViewModel> PopularBlogs()
        {
            var items = db.Blogs.OrderByDescending(b => b.PostView).Take(4).ToList();
            List<IndexSliderItemViewModel> model = new List<IndexSliderItemViewModel>();
            foreach (var blog in items)
            {
                var tr = db.BlogTranslations.FirstOrDefault(t => t.BlogId == blog.BlogId && t.Language == CurrentCulture);
                model.Add(new IndexSliderItemViewModel()
                {
                    Title = tr?.Title ?? blog.Title,
                    ShortDescription = (tr?.Title ?? blog.Title).Replace(" ", "-"),
                    ImageName = blog.ImageName,
                    SliderID = blog.BlogId
                });
            }
            return model;
        }

        public string SubmitComment(int BlogID, string fullname, string title, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(fullname))
                {
                    return "کاربر گرامی ! لطفا نام و نام خانوادگی را تکمیل فرمایید";
                }
                else if (string.IsNullOrEmpty(title))
                {
                    return "کاربر گرامی ! لطفا عنوان دیدگاه خود را فرمایید";
                }
                else if (string.IsNullOrEmpty(text))
                {
                    return "کاربر گرامی ! لطفا متن دیدگاه خود را فرمایید";
                }
                else
                {
                    BlogComment comment = new BlogComment()
                    {
                        BlogId = BlogID,
                        CreateDate = DateTime.Now,
                        Fullname = fullname,
                        Text = text,
                        Title = title,
                        IsConfirm = false,
                        IsAnswer = false,
                    };
                    db.BlogComments.Add(comment);
                    db.SaveChanges();
                    return $"{fullname} عزیز دیدگاه شما با موفقیت ثبت شد ! ممنون از حسن توجه شما";
                }
            }
            catch
            {
                return "کاربر گرامی ! هنگام عملیات خطایی رخ داد لطفا مجددا تلاش فرمایید";
            }
        }

        public string SubmitNewsComment(int BlogID, string fullname, string title, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(fullname))
                {
                    return "کاربر گرامی ! لطفا نام و نام خانوادگی را تکمیل فرمایید";
                }
                else if (string.IsNullOrEmpty(title))
                {
                    return "کاربر گرامی ! لطفا عنوان دیدگاه خود را فرمایید";
                }
                else if (string.IsNullOrEmpty(text))
                {
                    return "کاربر گرامی ! لطفا متن دیدگاه خود را فرمایید";
                }
                else
                {
                    NewsComment comment = new NewsComment()
                    {
                        NewsId = BlogID,
                        CreateDate = DateTime.Now,
                        Fullname = fullname,
                        Text = text,
                        Title = title,
                        IsConfirm = false,
                        IsAnswer = false,
                    };
                    db.NewsComments.Add(comment);
                    db.SaveChanges();
                    return $"{fullname} عزیز دیدگاه شما با موفقیت ثبت شد ! ممنون از حسن توجه شما";
                }
            }
            catch
            {
                return "کاربر گرامی ! هنگام عملیات خطایی رخ داد لطفا مجددا تلاش فرمایید";
            }
        }

        public PaginationViewModel GetBlogsPagination(double PageCount, int PageID = 1)
        {
            PaginationViewModel pagination = new PaginationViewModel();
            pagination.PageNumbers = new List<int>();
            int page = Convert.ToInt32(PageCount) + 1;
            if (PageID < 3)
            {
                for (int i = 1; i < 6; i++)
                {
                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }
            else
            {
                for (int i = PageID - 2; i <= PageID + 2; i++)
                {

                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }

            pagination.PageCount = Convert.ToInt32(PageCount);
            pagination.PageID = PageID;

            return pagination;
        }

        public BlogGroupsPostsPaginationViewModel GetPostsByPagination(int GroupID, int PageID=1 , string q ="")
        {
            if (!string.IsNullOrEmpty(q))
            {
                BlogGroupsPostsPaginationViewModel model = new BlogGroupsPostsPaginationViewModel();
                model.GroupID = GroupID;
                int take = 6;
                int skip = (PageID - 1) * take;

                var blogs = db.SelectedBlogGroups.Include(b => b.Blog).Where(b => b.BlogGroupId == GroupID && b.Blog.Title.Contains(q)).Skip(skip).Take(take).Select(b => b.Blog)
                        .OrderByDescending(b => b.CreateDate).ToList();
                model.Posts = new List<BlogItemViewModel>();
                foreach (var blog in blogs)
                {
                    model.Posts.Add(new BlogItemViewModel()
                    {
                        GroupID = GroupID,
                        BlogID = blog.BlogId,
                        CreateDate = DateConvertor.PassDays(blog.CreateDate),
                        ImageName = blog.ImageName,
                        Source = blog.Source,
                        Title = blog.Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.SelectedBlogGroups.Include(b => b.Blog).Where(b => b.BlogGroupId == GroupID && b.Blog.Title.Contains(q)).ToList().Count()) / Convert.ToDouble(take));
                model.Pagination = GetBlogsPagination(pCount, PageID);
                model.Pagination.PageCount = Convert.ToInt32(pCount);
                return model;
            }
            else
            {
                BlogGroupsPostsPaginationViewModel model = new BlogGroupsPostsPaginationViewModel();
                model.GroupID = GroupID;
                int take = 6;
                int skip = (PageID - 1) * take;

                var blogs = db.SelectedBlogGroups.Where(b => b.BlogGroupId == GroupID).Skip(skip).Take(take).Include(b => b.Blog).Select(b => b.Blog)
                        .OrderByDescending(b => b.CreateDate).ToList();
                model.Posts = new List<BlogItemViewModel>();
                foreach (var blog in blogs)
                {
                    model.Posts.Add(new BlogItemViewModel()
                    {
                        GroupID = GroupID,
                        BlogID = blog.BlogId,
                        CreateDate = DateConvertor.PassDays(blog.CreateDate),
                        ImageName = blog.ImageName,
                        Source = blog.Source,
                        Title = blog.Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.SelectedBlogGroups.Where(s => s.BlogGroupId == GroupID).ToList().Count()) / Convert.ToDouble(take));
                model.Pagination = GetBlogsPagination(pCount, PageID);
                model.Pagination.PageCount = Convert.ToInt32(pCount);
                return model;
            }
        }
        public BlogGroupsPostsPaginationViewModel GetNewsByPagination(int PageID)
        {
            BlogGroupsPostsPaginationViewModel model = new BlogGroupsPostsPaginationViewModel();
            model.GroupID = 0;
            int take = 6;
            int skip = (PageID - 1) * take;

            var blogs = db.NewsFeatures.OrderByDescending(n => n.CreateDate).Skip(skip).Take(take).ToList();
            model.Posts = new List<BlogItemViewModel>();
            foreach (var blog in blogs)
            {
                model.Posts.Add(new BlogItemViewModel()
                {
                    GroupID = 0,
                    BlogID = blog.NewsId,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate),
                    ImageName = blog.ImageName,
                    Source = blog.Source,
                    Title = blog.Title
                });
            }
            double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsFeatures.ToList().Count()) / Convert.ToDouble(take));
            model.Pagination = GetBlogsPagination(pCount, PageID);
            model.Pagination.PageCount = Convert.ToInt32(pCount);
            return model;
        }

        public AdminBlogPageDataViewModel GetBlogs(string q = "", int PageID = 1)
        {
            AdminBlogPageDataViewModel blogPosts = new AdminBlogPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            blogPosts.Posts = new List<BlogPostsItemViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var posts = db.Blogs.OrderByDescending(b => b.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var item in posts)
                {
                    List<string> groupName = db.SelectedBlogGroups.Where(b => b.BlogId == item.BlogId).Include(b => b.BlogGroup).Select(b => b.BlogGroup.GroupName).ToList();
                    blogPosts.Posts.Add(new BlogPostsItemViewModel()
                    {
                        BlogID = item.BlogId,
                        Title = item.Title,
                        CreateDate = DateConvertor.ToShamsi(item.CreateDate),
                        View = item.PostView,
                        GroupName = string.Join(" ، ", groupName)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.Blogs.ToList().Count()) / Convert.ToDouble(take));
                blogPosts.PageCount = pCount;
                blogPosts.Pagination = GetBlogsPagination(pCount, PageID);
                return blogPosts;
            }
            else
            {
                var posts = db.Blogs.Where(b => b.Title.Contains(q)).OrderByDescending(b => b.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var item in posts)
                {
                    List<string> groupName = db.SelectedBlogGroups.Where(b => b.BlogId == item.BlogId).Include(b => b.BlogGroup).Select(b => b.BlogGroup.GroupName).ToList();
                    blogPosts.Posts.Add(new BlogPostsItemViewModel()
                    {
                        BlogID = item.BlogId,
                        Title = item.Title,
                        CreateDate = DateConvertor.ToShamsi(item.CreateDate),
                        View = item.PostView,
                        GroupName = string.Join(" ، ", groupName)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.Blogs.Where(b => b.Title.Contains(q)).ToList().Count()) / Convert.ToDouble(take));
                blogPosts.PageCount = pCount;
                blogPosts.Pagination = GetBlogsPagination(pCount, PageID);
                return blogPosts;
            }
        }

        public BlogCommentsPageDataViewModel GetComments(string q = "", int PageID = 1)
        {
            BlogCommentsPageDataViewModel blogComments = new BlogCommentsPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            blogComments.Comments = new List<BlogCommentViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var comments = db.BlogComments.Where(c => c.ParentId == null).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.BlogId,
                        CommentID = comment.CommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.Blogs.Find(comment.BlogId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.BlogComments.Where(c => c.ParentId == null).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
            else
            {
                var comments = db.BlogComments.Where(c => c.Title.Contains(q) && c.ParentId == null).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.BlogId,
                        CommentID = comment.CommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.Blogs.Find(comment.BlogId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.BlogComments.Where(c => c.Title.Contains(q) && c.ParentId == null).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
        }

        public BlogCommentsPageDataViewModel GetConfirmComments(string q = "", int PageID = 1)
        {
            BlogCommentsPageDataViewModel blogComments = new BlogCommentsPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            blogComments.Comments = new List<BlogCommentViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var comments = db.BlogComments.Where(c => c.ParentId == null && c.IsConfirm == false).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.BlogId,
                        CommentID = comment.CommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.Blogs.Find(comment.BlogId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.BlogComments.Where(c => c.ParentId == null && c.IsConfirm == false).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
            else
            {
                var comments = db.BlogComments.Where(c => c.Title.Contains(q) && c.ParentId == null && c.IsConfirm == false).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.BlogId,
                        CommentID = comment.CommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.Blogs.Find(comment.BlogId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.BlogComments.Where(c => c.Title.Contains(q) && c.ParentId == null && c.IsConfirm == false).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
        }

        public BlogCommentsPageDataViewModel GetAnswerComments(string q = "", int PageID = 1)
        {
            BlogCommentsPageDataViewModel blogComments = new BlogCommentsPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            blogComments.Comments = new List<BlogCommentViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var comments = db.BlogComments.Where(c => c.ParentId == null && c.IsAnswer == false).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.BlogId,
                        CommentID = comment.CommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.Blogs.Find(comment.BlogId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.BlogComments.Where(c => c.ParentId == null && c.IsAnswer == false).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
            else
            {
                var comments = db.BlogComments.Where(c => c.Title.Contains(q) && c.ParentId == null && c.IsAnswer == false).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.BlogId,
                        CommentID = comment.CommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.Blogs.Find(comment.BlogId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.BlogComments.Where(c => c.Title.Contains(q) && c.ParentId == null && c.IsAnswer == false).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
        }

        public AdminBlogPageDataViewModel GetNews(string q = "", int PageID = 1)
        {
            AdminBlogPageDataViewModel blogPosts = new AdminBlogPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            blogPosts.Posts = new List<BlogPostsItemViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var posts = db.NewsFeatures.OrderByDescending(b => b.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var item in posts)
                {
                    List<string> groupName = db.SelectedFeatureNews.Where(b => b.NewsId == item.NewsId).Include(b => b.Feature).Select(b => b.Feature.Title).ToList();
                    blogPosts.Posts.Add(new BlogPostsItemViewModel()
                    {
                        BlogID = item.NewsId,
                        Title = item.Title,
                        CreateDate = DateConvertor.ToShamsi(item.CreateDate),
                        View = item.PostView,
                        GroupName = string.Join(" ، ", groupName)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsFeatures.ToList().Count()) / Convert.ToDouble(take));
                blogPosts.PageCount = pCount;
                blogPosts.Pagination = GetBlogsPagination(pCount, PageID);
                return blogPosts;
            }
            else
            {
                var posts = db.NewsFeatures.Where(b => b.Title.Contains(q)).OrderByDescending(b => b.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var item in posts)
                {
                    List<string> groupName = db.SelectedFeatureNews.Where(b => b.NewsId == item.NewsId).Include(b => b.Feature).Select(b => b.Feature.Title).ToList();
                    blogPosts.Posts.Add(new BlogPostsItemViewModel()
                    {
                        BlogID = item.NewsId,
                        Title = item.Title,
                        CreateDate = DateConvertor.ToShamsi(item.CreateDate),
                        View = item.PostView,
                        GroupName = string.Join(" ، ", groupName)
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsFeatures.Where(b => b.Title.Contains(q)).ToList().Count()) / Convert.ToDouble(take));
                blogPosts.PageCount = pCount;
                blogPosts.Pagination = GetBlogsPagination(pCount, PageID);
                return blogPosts;
            }
        }

        public BlogCommentsPageDataViewModel GetNewsComments(string q = "", int PageID = 1)
        {
            BlogCommentsPageDataViewModel blogComments = new BlogCommentsPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            blogComments.Comments = new List<BlogCommentViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var comments = db.NewsComments.Where(c => c.ParentId == null).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.NewsId,
                        CommentID = comment.NewsCommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.NewsFeatures.Find(comment.NewsId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsComments.Where(c => c.ParentId == null).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
            else
            {
                var comments = db.NewsComments.Where(c => c.Title.Contains(q) && c.ParentId == null).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.NewsId,
                        CommentID = comment.NewsCommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.NewsFeatures.Find(comment.NewsId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsComments.Where(c => c.Title.Contains(q) && c.ParentId == null).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
        }

        public BlogCommentsPageDataViewModel GetNewsConfirmComments(string q = "", int PageID = 1)
        {
            BlogCommentsPageDataViewModel blogComments = new BlogCommentsPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            blogComments.Comments = new List<BlogCommentViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var comments = db.NewsComments.Where(c => c.ParentId == null && c.IsConfirm == false).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.NewsId,
                        CommentID = comment.NewsCommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.NewsFeatures.Find(comment.NewsId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsComments.Where(c => c.ParentId == null && c.IsConfirm == false).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
            else
            {
                var comments = db.NewsComments.Where(c => c.Title.Contains(q) && c.ParentId == null && c.IsConfirm == false).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.NewsId,
                        CommentID = comment.NewsCommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.NewsFeatures.Find(comment.NewsId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsComments.Where(c => c.Title.Contains(q) && c.ParentId == null && c.IsConfirm == false).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
        }

        public BlogCommentsPageDataViewModel GetNewsAnswerComments(string q = "", int PageID = 1)
        {
            BlogCommentsPageDataViewModel blogComments = new BlogCommentsPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            blogComments.Comments = new List<BlogCommentViewModel>();
            if (string.IsNullOrEmpty(q))
            {
                var comments = db.NewsComments.Where(c => c.ParentId == null && c.IsAnswer == false).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.NewsId,
                        CommentID = comment.NewsCommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.NewsFeatures.Find(comment.NewsId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsComments.Where(c => c.ParentId == null && c.IsAnswer == false).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
            else
            {
                var comments = db.NewsComments.Where(c => c.Title.Contains(q) && c.ParentId == null && c.IsAnswer == false).OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                foreach (var comment in comments)
                {
                    blogComments.Comments.Add(new BlogCommentViewModel()
                    {
                        BlogID = comment.NewsId,
                        CommentID = comment.NewsCommentId,
                        CommentText = comment.Title,
                        IsAnswer = comment.IsAnswer,
                        IsConfirm = comment.IsConfirm,
                        CreateDate = DateConvertor.ToShamsi(comment.CreateDate),
                        Fullname = comment.Fullname,
                        BlogTitle = db.NewsFeatures.Find(comment.NewsId).Title
                    });
                }
                double pCount = Convert.ToDouble(Convert.ToDouble(db.NewsComments.Where(c => c.Title.Contains(q) && c.ParentId == null && c.IsAnswer == false).ToList().Count()) / Convert.ToDouble(take));
                blogComments.PageCount = pCount;
                blogComments.Pagination = GetBlogsPagination(pCount, PageID);
                return blogComments;
            }
        }

        public bool Create(BlogCrudViewModel blog, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null || !blog.SelectedGroups.Any() || string.IsNullOrEmpty(blog.Tags))
                {
                    return false;
                }
                blog.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", blog.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                Blog model = new Blog()
                {
                    CreateDate = DateTime.Now,
                    Description = blog.Description,
                    ShortDescription = blog.ShortDescription,
                    IsSlider = blog.IsSlider,
                    ImageName = blog.ImageName,
                    Source = blog.Source,
                    Title = blog.Title,
                    StudyTime = blog.StudyTime,
                    PostView = 0,
                };
                db.Blogs.Add(model);
                db.SaveChanges();

                var tags = blog.Tags.Split('،').ToList();

                foreach (var item in tags)
                {
                    db.BlogTags.Add(new BlogTag() { BlogId = model.BlogId, Tag = item });
                    db.SaveChanges();
                }

                foreach (var item in blog.SelectedGroups)
                {
                    db.SelectedBlogGroups.Add(new SelectedBlogGroup() { BlogId = model.BlogId, BlogGroupId = item });
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(BlogCrudViewModel blog, IFormFile ImageName)
        {
            try
            {
                if (!blog.SelectedGroups.Any() || string.IsNullOrEmpty(blog.Tags))
                {
                    return false;
                }
                var model = db.Blogs.Find(blog.BlogID);
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", model.ImageName);

                    System.IO.File.Delete(imagePath);

                    model.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", model.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }

                model.Description = blog.Description;
                model.IsSlider = blog.IsSlider;
                model.Source = blog.Source;
                model.Title = blog.Title;
                model.StudyTime = blog.StudyTime;
                db.Blogs.Update(model);
                db.SaveChanges();

                db.BlogTags.Where(b => b.BlogId == model.BlogId).ToList().ForEach(b => db.BlogTags.Remove(b));
                db.SaveChanges();

                var tags = blog.Tags.Split('،').ToList();

                foreach (var item in tags)
                {
                    db.BlogTags.Add(new BlogTag() { BlogId = model.BlogId, Tag = item });
                    db.SaveChanges();
                }

                db.SelectedBlogGroups.Where(b => b.BlogId == model.BlogId).ToList().ForEach(b => db.SelectedBlogGroups.Remove(b));
                db.SaveChanges();

                foreach (var item in blog.SelectedGroups)
                {
                    db.SelectedBlogGroups.Add(new SelectedBlogGroup() { BlogId = model.BlogId, BlogGroupId = item });
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool Delete(int ID)
        {
            try
            {
                var blog = db.Blogs.Find(ID);

                db.SelectedBlogGroups.Where(b => b.BlogId == blog.BlogId).ToList().ForEach(b => db.SelectedBlogGroups.Remove(b));
                db.SaveChanges();

                db.BlogTags.Where(b => b.BlogId == blog.BlogId).ToList()
                                       .ForEach(b => db.BlogTags.Remove(b));
                db.SaveChanges();

                db.SelectedRelatedBlogs.Where(b => b.RelatedBlogId == blog.BlogId || b.MainBlogId == blog.BlogId).ToList()
                                       .ForEach(b => db.SelectedRelatedBlogs.Remove(b));
                db.SaveChanges();

                db.BlogComments.Where(b => b.BlogId == blog.BlogId).ToList()
                                       .ForEach(b => db.BlogComments.Remove(b));
                db.SaveChanges();

                db.Blogs.Remove(blog);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public BlogCrudViewModel GetModelForCreate()
        {
            BlogCrudViewModel model = new BlogCrudViewModel();
            model.Groups = new List<BlogGroupNameViewModel>();
            var items = db.BlogGroups.ToList();
            foreach (var item in items)
            {
                model.Groups.Add(new BlogGroupNameViewModel()
                {
                    GroupID = item.BlogGroupId,
                    GroupName = item.GroupName
                });
            }
            return model;
        }

        public BlogCrudViewModel GetByID(int BlogID)
        {
            var blog = db.Blogs.Find(BlogID);
            BlogCrudViewModel model = new BlogCrudViewModel()
            {
                BlogID = BlogID,
                Description = blog.Description,
                ShortDescription = blog.ShortDescription,
                ImageName = blog.ImageName,
                IsSlider = blog.IsSlider,
                StudyTime = blog.StudyTime,
                Source = blog.Source,
                Title = blog.Title,
                Tags = string.Join("،", db.BlogTags.Where(t => t.BlogId == BlogID).Select(b => b.Tag).ToList()),
                SelectedGroups = db.SelectedBlogGroups.Where(s => s.BlogId == BlogID).Select(s => s.BlogGroupId).ToList()
            };

            var groups = db.BlogGroups.ToList();
            model.Groups = new List<BlogGroupNameViewModel>();
            foreach (var item in groups)
            {
                model.Groups.Add(new BlogGroupNameViewModel()
                {
                    GroupID = item.BlogGroupId,
                    GroupName = item.GroupName
                });
            }
            return model;
        }

        #region NewsCrud
        public bool CreateNews(BlogCrudViewModel blog, IFormFile ImageName)
        {
            try
            {
                if (ImageName == null || !blog.SelectedGroups.Any() || string.IsNullOrEmpty(blog.Tags))
                {
                    return false;
                }
                blog.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", blog.ImageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageName.CopyTo(stream);
                }
                NewsFeature model = new NewsFeature()
                {
                    CreateDate = DateTime.Now,
                    Description = blog.Description,
                    ShortDescription = blog.ShortDescription,
                    ImageName = blog.ImageName,
                    Source = blog.Source,
                    Title = blog.Title,
                    StudyTime = blog.StudyTime,
                    PostView = 0,
                };
                db.NewsFeatures.Add(model);
                db.SaveChanges();

                var tags = blog.Tags.Split('،').ToList();

                foreach (var item in tags)
                {
                    db.NewsTags.Add(new NewsTag() { NewsId = model.NewsId, Tag = item });
                    db.SaveChanges();
                }

                foreach (var item in blog.SelectedGroups)
                {
                    db.SelectedFeatureNews.Add(new SelectedFeatureNews() { NewsId = model.NewsId, FeatureId = item });
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateNews(BlogCrudViewModel blog, IFormFile ImageName)
        {
            try
            {
                if (!blog.SelectedGroups.Any() || string.IsNullOrEmpty(blog.Tags))
                {
                    return false;
                }
                var model = db.NewsFeatures.Find(blog.BlogID);
                if (ImageName != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", model.ImageName);

                    System.IO.File.Delete(imagePath);

                    model.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", model.ImageName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }

                }

                model.Description = blog.Description;
                model.Source = blog.Source;
                model.Title = blog.Title;
                model.StudyTime = blog.StudyTime;
                db.NewsFeatures.Update(model);
                db.SaveChanges();

                db.NewsTags.Where(b => b.NewsId == model.NewsId).ToList().ForEach(b => db.NewsTags.Remove(b));
                db.SaveChanges();

                var tags = blog.Tags.Split('،').ToList();

                foreach (var item in tags)
                {
                    db.NewsTags.Add(new NewsTag() { NewsId = model.NewsId, Tag = item });
                    db.SaveChanges();
                }

                db.SelectedFeatureNews.Where(b => b.NewsId == model.NewsId).ToList().ForEach(b => db.SelectedFeatureNews.Remove(b));
                db.SaveChanges();

                foreach (var item in blog.SelectedGroups)
                {
                    db.SelectedFeatureNews.Add(new SelectedFeatureNews() { NewsId = model.NewsId, FeatureId = item });
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteNews(int ID)
        {
            try
            {
                var blog = db.NewsFeatures.Find(ID);

                db.SelectedFeatureNews.Where(b => b.NewsId == blog.NewsId).ToList().ForEach(b => db.SelectedFeatureNews.Remove(b));
                db.SaveChanges();

                db.NewsTags.Where(b => b.NewsId == blog.NewsId).ToList()
                                       .ForEach(b => db.NewsTags.Remove(b));
                db.SaveChanges();

                db.NewsComments.Where(b => b.NewsId == blog.NewsId).ToList()
                                       .ForEach(b => db.NewsComments.Remove(b));
                db.SaveChanges();

                db.NewsFeatures.Remove(blog);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BlogCrudViewModel GetNewsModelForCreate()
        {
            BlogCrudViewModel model = new BlogCrudViewModel();
            model.Groups = new List<BlogGroupNameViewModel>();
            var items = db.Features.ToList();
            foreach (var item in items)
            {
                model.Groups.Add(new BlogGroupNameViewModel()
                {
                    GroupID = item.FeatureId,
                    GroupName = item.Title
                });
            }
            return model;
        }

        public BlogCrudViewModel GetNewsByID(int BlogID)
        {
            var blog = db.NewsFeatures.Find(BlogID);
            BlogCrudViewModel model = new BlogCrudViewModel()
            {
                BlogID = BlogID,
                Description = blog.Description,
                ShortDescription = blog.ShortDescription,
                ImageName = blog.ImageName,
                Source = blog.Source,
                Title = blog.Title,
                Tags = string.Join("،", db.NewsTags.Where(t => t.NewsId == BlogID).Select(b => b.Tag).ToList()),
                SelectedGroups = db.SelectedFeatureNews.Where(s => s.NewsId == BlogID).Select(s => s.FeatureId).ToList()
            };
            if (blog.StudyTime != null)
            {
                model.StudyTime = blog.StudyTime.Value;
            }

            var groups = db.Features.ToList();
            model.Groups = new List<BlogGroupNameViewModel>();
            foreach (var item in groups)
            {
                model.Groups.Add(new BlogGroupNameViewModel()
                {
                    GroupID = item.FeatureId,
                    GroupName = item.Title
                });
            }
            return model;
        }
        #endregion NewsCrud

        #region NewsComment
        public bool AnswerComment(NewsComment comment)
        {
            try
            {
                comment.CreateDate = DateTime.Now;
                comment.IsAnswer = false;
                comment.IsConfirm = true;
                comment.NewsCommentId = 0;
                db.NewsComments.Add(comment);
                db.SaveChanges();

                var blog = db.NewsComments.Find(comment.ParentId);
                blog.IsAnswer = true;
                db.NewsComments.Update(blog);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateComment(NewsComment comment)
        {
            try
            {
                db.NewsComments.Update(comment);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(NewsComment comment)
        {
            try
            {
                var blogs = db.NewsComments.Where(b => b.ParentId == comment.NewsCommentId).ToList();
                foreach (var blog in blogs)
                {
                    db.NewsComments.Remove(blog);
                    db.SaveChanges();
                }
                db.NewsComments.Remove(comment);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public NewsComment GetNewsCommentByID(int id)
        {
            return db.NewsComments.Find(id);
        }

        public NewsComment GetNewsAnswerByID(int id)
        {
            var blog = db.NewsComments.FirstOrDefault(b => b.ParentId == id);
            return blog;
        }
        #endregion NewsComment

        public bool AnswerComment(BlogComment comment)
        {
            try
            {
                comment.CreateDate = DateTime.Now;
                comment.IsAnswer = false;
                comment.IsConfirm = true;
                comment.CommentId = 0;
                db.BlogComments.Add(comment);
                db.SaveChanges();

                var blog = db.BlogComments.Find(comment.ParentId);
                blog.IsAnswer = true;
                db.BlogComments.Update(blog);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateComment(BlogComment comment)
        {
            try
            {
                db.BlogComments.Update(comment);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(BlogComment comment)
        {
            try
            {
                var blogs = db.BlogComments.Where(b => b.ParentId == comment.CommentId).ToList();
                foreach (var blog in blogs)
                {
                    db.BlogComments.Remove(blog);
                    db.SaveChanges();
                }
                db.BlogComments.Remove(comment);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public BlogComment GetCommentByID(int id)
        {
            return db.BlogComments.Find(id);
        }

        public BlogComment GetAnswerByID(int id)
        {
            var blog = db.BlogComments.FirstOrDefault(b => b.ParentId == id);
            return blog;
        }

        public List<BlogItemViewModel> GetGroupsForRelated(string q="")
        {
            if (string.IsNullOrEmpty(q))
            {
                List<BlogItemViewModel> model = new List<BlogItemViewModel>();
                var items = db.Blogs.Select(b => new { b.BlogId, b.Title }).ToList();
                foreach (var item in items)
                {
                    model.Add(new BlogItemViewModel() { BlogID = item.BlogId, Title = item.Title });
                }
                return model;
            }
            else
            {
                List<BlogItemViewModel> model = new List<BlogItemViewModel>();
                var items = db.Blogs.Where(b => b.Title.Contains(q)).Select(b => new { b.BlogId, b.Title }).ToList();
                foreach (var item in items)
                {
                    model.Add(new BlogItemViewModel() { BlogID = item.BlogId, Title = item.Title });
                }
                return model;
            }
        }

        public CreateRelatedBlogsViewModel GetModelForCreateRelatedBlog(int BlogID, string q = "")
        {
            CreateRelatedBlogsViewModel model = new CreateRelatedBlogsViewModel() 
            {
                BlogID = BlogID,
                Blogs = GetGroupsForRelated(q),
                SelectedBlogs = db.SelectedRelatedBlogs.Where(b =>b.MainBlogId == BlogID).Select(b => b.RelatedBlogId).ToList()
            };
            return model;
        }

        public bool CreateRelatedBlogs(CreateRelatedBlogsViewModel blogs)
        {
            try
            {
                if (!blogs.SelectedBlogs.Any())
                {
                    return false;
                }
                db.SelectedRelatedBlogs.Where(b => b.MainBlogId == blogs.BlogID).ToList().ForEach(b => db.SelectedRelatedBlogs.Remove(b));
                db.SaveChanges();

                foreach (var item in blogs.SelectedBlogs)
                {
                    db.SelectedRelatedBlogs.Add(new SelectedRelatedBlog { MainBlogId = blogs.BlogID , RelatedBlogId = item });
                    db.SaveChanges() ;
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public string GetBlogBanner()
        {
            return db.SiteSettings.FirstOrDefault().BlogBannerImage;
        }

        public SeoTagsViewModel GetBlogSeoTag(int blogID)
        {
            var modelDB = GetByID(blogID);
            var content = new SeoTagsViewModel()
            {
                ImageName = modelDB.ImageName,
                BlogID = modelDB.BlogID,
                Title = modelDB.Title,
                Description = modelDB.ShortDescription,
            };
            return content;
        }
        public SeoTagsViewModel GetNewsSeoTag(int blogID)
        {
            var modelDB = GetNewsByID(blogID);
            var content = new SeoTagsViewModel()
            {
                ImageName = modelDB.ImageName,
                BlogID = modelDB.BlogID,
                Title = modelDB.Title,
                Description = modelDB.ShortDescription,
            };
            return content;
        }

        public List<BlogItemViewModel> GetBlogsByTag(string tag)
        {
            List<BlogItemViewModel> content = new List<BlogItemViewModel>();
            var blogModelDB = db.BlogTags.Where(t => t.Tag == tag).Include(t=> t.Blog).Select(t=> t.Blog).ToList();
            var newsModelDB = db.NewsTags.Where(t => t.Tag == tag).Include(t => t.News).Select(t => t.News).ToList();

            foreach (var blog in blogModelDB)
            {
                var tr = db.BlogTranslations.FirstOrDefault(t => t.BlogId == blog.BlogId && t.Language == CurrentCulture);
                content.Add(new BlogItemViewModel()
                {
                    BlogID = blog.BlogId,
                    Title = tr?.Title ?? blog.Title,
                    ImageName = blog.ImageName,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate),
                    Source = blog.Source,
                    IsBlog = true
                });
            }

            foreach (var blog in newsModelDB)
            {
                content.Add(new BlogItemViewModel()
                {
                    BlogID = blog.NewsId,
                    Title = blog.Title,
                    ImageName = blog.ImageName,
                    CreateDate = DateConvertor.PassDays(blog.CreateDate),
                    Source = blog.Source,
                    IsBlog = false
                });
            }

            return content;
        }
    }
}
