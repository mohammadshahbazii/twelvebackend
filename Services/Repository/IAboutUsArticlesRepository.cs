using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAboutUsArticlesRepository : IDisposable
    {
        public List<AboutUsArticle> GetAboutUsArticles();
        public AboutUsArticle GetByID(int aboutUsArticleID);
        public bool Create(AboutUsArticle aboutUsArticle , IFormFile ImageName);
        public bool Update(AboutUsArticle aboutUsArticle , IFormFile ImageName);
        public bool Delete(AboutUsArticle aboutUsArticle);
    }
}
