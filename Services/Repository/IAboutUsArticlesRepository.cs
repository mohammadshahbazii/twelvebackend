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
    public interface IAboutUsArticlesRepository : IDisposable
    {
        public List<AboutUsArticle> GetAboutUsArticles();
        public AboutUsArticle GetByID(int aboutUsArticleID);
        public AboutUsArticleCrudViewModel GetForEdit(int aboutUsArticleID);
        public bool Create(AboutUsArticleCrudViewModel aboutUsArticle, IFormFile ImageName);
        public bool Update(AboutUsArticleCrudViewModel aboutUsArticle, IFormFile ImageName);
        public bool Delete(AboutUsArticle aboutUsArticle);
        public void SaveTranslations(AboutUsArticleCrudViewModel aboutUsArticle);
    }
}
