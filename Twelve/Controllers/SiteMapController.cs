using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Twelve.Controllers
{
    public class SiteMapController : Controller
    {
        private string cacheKey = "BlogList";
        private string NewscacheKey = "NewsList";
        IMemoryCache _memoryCache;

        public SiteMapController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [Route("/SiteMap.xml")]
        public async Task<ActionResult> Index()
        {

            List<Blog> blogs;
            List<NewsFeature> news;
            if (!_memoryCache.TryGetValue(cacheKey, out blogs))
            {
                using (TwelveDbContext db = new TwelveDbContext())
                {
                    blogs = await db.Blogs.ToListAsync();

                }

                _memoryCache.Set(cacheKey, blogs, TimeSpan.FromDays(1));
            }
            if (!_memoryCache.TryGetValue(NewscacheKey, out news))
            {
                using (TwelveDbContext db = new TwelveDbContext())
                {
                    news = await db.NewsFeatures.ToListAsync();

                }

                _memoryCache.Set(NewscacheKey, news, TimeSpan.FromDays(1));
            }


            //// list of items to add
            // courses = await _db.Courses.ToListAsync();

            var siteMapBuilder = new Utilities.SiteMapBuilder();

            // add the blog posts to the sitemap
            foreach (var blog in blogs)
            {
                siteMapBuilder.AddUrl("https://12application.ir/PostBlogs/" + blog.BlogId, modified: blog.CreateDate.Date, changeFrequency: Utilities.ChangeFrequency.Daily, priority: 0.9);
            }
            foreach (var post in news)
            {
                siteMapBuilder.AddUrl("https://12application.ir/PostNews/" + post.NewsId, modified: post.CreateDate.Date, changeFrequency: Utilities.ChangeFrequency.Daily, priority: 0.9);
            }

            // generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            return Content(xml, "text/xml");
        }
    }
}
