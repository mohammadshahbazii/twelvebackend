using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public partial class TwelveDbContext : DbContext
{
    public TwelveDbContext()
    {
    }

    public TwelveDbContext(DbContextOptions<TwelveDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AboutUsArticle> AboutUsArticles { get; set; }

    public virtual DbSet<AboutUsContent> AboutUsContents { get; set; }

    public virtual DbSet<AboutUsItem> AboutUsItems { get; set; }

    public virtual DbSet<AboutUsLogo> AboutUsLogoes { get; set; }

    public virtual DbSet<AboutUsSight> AboutUsSights { get; set; }

    public virtual DbSet<Acess> Acesses { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<BlogGroup> BlogGroups { get; set; }

    public virtual DbSet<BlogTag> BlogTags { get; set; }

    public virtual DbSet<CallInfo> CallInfoes { get; set; }

    public virtual DbSet<CallInfoLink> CallInfoLinks { get; set; }

    public virtual DbSet<ContactUsContent> ContactUsContents { get; set; }

    public virtual DbSet<DownloadLink> DownloadLinks { get; set; }

    public virtual DbSet<DownloadLinkGroup> DownloadLinkGroups { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<FaqContent> FaqContents { get; set; }

    public virtual DbSet<FaqGroup> FaqGroups { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<FeatureItem> FeatureItems { get; set; }

    public virtual DbSet<FeaturesContent> FeaturesContents { get; set; }

    public virtual DbSet<FeedBack> FeedBacks { get; set; }

    public virtual DbSet<IndexContent> IndexContents { get; set; }

    public virtual DbSet<Introduce> Introduces { get; set; }

    public virtual DbSet<IntroduceSlider> IntroduceSliders { get; set; }

    public virtual DbSet<NewsComment> NewsComments { get; set; }

    public virtual DbSet<NewsFeature> NewsFeatures { get; set; }

    public virtual DbSet<NewsTag> NewsTags { get; set; }

    public virtual DbSet<OptionFeature> OptionFeatures { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<SelectedAdminAccess> SelectedAdminAccesses { get; set; }

    public virtual DbSet<SelectedBlogGroup> SelectedBlogGroups { get; set; }

    public virtual DbSet<SelectedFaqGroup> SelectedFaqGroups { get; set; }

    public virtual DbSet<SelectedFeatureNews> SelectedFeatureNews { get; set; }

    public virtual DbSet<SelectedRelatedBlog> SelectedRelatedBlogs { get; set; }

    public virtual DbSet<SiteSetting> SiteSettings { get; set; }

    public virtual DbSet<SiteTag> SiteTags { get; set; }

    public virtual DbSet<SiteVisit> SiteVisits { get; set; }

    public virtual DbSet<Slider> Sliders { get; set; }

    public virtual DbSet<SliderGroup> SliderGroups { get; set; }

    public virtual DbSet<SocialMedium> SocialMedia { get; set; }

    public virtual DbSet<BlogTranslation> BlogTranslations { get; set; }

    public virtual DbSet<FaqTranslation> FaqTranslations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=185.79.157.4\\MSSQLSERVER2019;User ID =Twelve;Password =!48HhTv$4rUrZ8;initial catalog=Twelve_DB;Trusted_Connection=False;integrated security=False;MultipleActiveResultSets=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AboutUsArticle>(entity =>
        {
            entity.Property(e => e.AboutUsArticleId).HasColumnName("AboutUsArticleID");
            entity.Property(e => e.ImageName).HasMaxLength(150);
        });

        modelBuilder.Entity<AboutUsContent>(entity =>
        {
            entity.ToTable("AboutUsContent");

            entity.Property(e => e.AboutUsContentId).HasColumnName("AboutUsContentID");
            entity.Property(e => e.DownloadImageName).HasMaxLength(150);
            entity.Property(e => e.ImageName).HasMaxLength(150);
        });

        modelBuilder.Entity<AboutUsItem>(entity =>
        {
            entity.Property(e => e.AboutUsItemId).HasColumnName("AboutUsItemID");
            entity.Property(e => e.ImageName).HasMaxLength(150);
        });

        modelBuilder.Entity<AboutUsLogo>(entity =>
        {
            entity.Property(e => e.AboutUsLogoId).HasColumnName("AboutUsLogoID");
            entity.Property(e => e.ImageName).HasMaxLength(150);
        });

        modelBuilder.Entity<AboutUsSight>(entity =>
        {
            entity.ToTable("AboutUsSight");

            entity.Property(e => e.AboutUsSightId).HasColumnName("AboutUsSightID");
            entity.Property(e => e.ImageName).HasMaxLength(150);
        });

        modelBuilder.Entity<Acess>(entity =>
        {
            entity.HasKey(e => e.AccessId);

            entity.Property(e => e.AccessId).HasColumnName("AccessID");
            entity.Property(e => e.AccessText).HasMaxLength(150);
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Address).HasMaxLength(350);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(350);
        });

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.Property(e => e.AdvertisementId).HasColumnName("AdvertisementID");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Source).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(350);
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(350);
            entity.Property(e => e.Fullname).HasMaxLength(350);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Title).HasMaxLength(350);

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlogComments_Blogs");
        });

        modelBuilder.Entity<BlogGroup>(entity =>
        {
            entity.Property(e => e.BlogGroupId).HasColumnName("BlogGroupID");
            entity.Property(e => e.GroupName).HasMaxLength(250);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
        });

        modelBuilder.Entity<BlogTag>(entity =>
        {
            entity.HasKey(e => e.BlogTagsId);

            entity.Property(e => e.BlogTagsId).HasColumnName("BlogTagsID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.Tag).HasMaxLength(50);

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogTags)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlogTags_Blogs");
        });

        modelBuilder.Entity<CallInfo>(entity =>
        {
            entity.Property(e => e.CallInfoId).HasColumnName("CallInfoID");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.ShortDescription).HasMaxLength(450);
            entity.Property(e => e.Title).HasMaxLength(350);
        });

        modelBuilder.Entity<CallInfoLink>(entity =>
        {
            entity.Property(e => e.CallInfoLinkId).HasColumnName("CallInfoLinkID");
            entity.Property(e => e.CallInfoId).HasColumnName("CallInfoID");

            entity.HasOne(d => d.CallInfo).WithMany(p => p.CallInfoLinks)
                .HasForeignKey(d => d.CallInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CallInfoLinks_CallInfoes");
        });

        modelBuilder.Entity<ContactUsContent>(entity =>
        {
            entity.ToTable("ContactUsContent");

            entity.Property(e => e.ContactUsContentId).HasColumnName("ContactUsContentID");
        });

        modelBuilder.Entity<DownloadLink>(entity =>
        {
            entity.Property(e => e.DownloadLinkId).HasColumnName("DownloadLinkID");
            entity.Property(e => e.DlgroupId).HasColumnName("DLGroupID");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.ShortDescription).HasMaxLength(350);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Dlgroup).WithMany(p => p.DownloadLinks)
                .HasForeignKey(d => d.DlgroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DownloadLinks_DownloadLinkGroups");
        });

        modelBuilder.Entity<DownloadLinkGroup>(entity =>
        {
            entity.HasKey(e => e.DlgroupId);

            entity.Property(e => e.DlgroupId).HasColumnName("DLGroupID");
            entity.Property(e => e.GroupName).HasMaxLength(350);
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.Property(e => e.FaqId).HasColumnName("FaqID");
        });

        modelBuilder.Entity<FaqContent>(entity =>
        {
            entity.Property(e => e.FaqContentId).HasColumnName("FaqContentID");
        });

        modelBuilder.Entity<FaqGroup>(entity =>
        {
            entity.Property(e => e.FaqGroupId).HasColumnName("FaqGroupID");
            entity.Property(e => e.GroupName).HasMaxLength(250);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.AnimateFilename).HasMaxLength(150);
            entity.Property(e => e.FirstArticleImage).HasMaxLength(150);
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.IntroduceImageName).HasMaxLength(50);
            entity.Property(e => e.SecondArticleImage).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<FeatureItem>(entity =>
        {
            entity.HasKey(e => e.FeaturesItemId);

            entity.Property(e => e.FeaturesItemId).HasColumnName("FeaturesItemID");
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Feature).WithMany(p => p.FeatureItems)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeatureItems_Features");
        });

        modelBuilder.Entity<FeaturesContent>(entity =>
        {
            entity.HasKey(e => e.FeatureContentId);

            entity.ToTable("FeaturesContent");

            entity.Property(e => e.FeatureContentId).HasColumnName("FeatureContentID");
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.Property(e => e.FeedBackId).HasColumnName("FeedBackID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(350);
            entity.Property(e => e.Fullname).HasMaxLength(350);
            entity.Property(e => e.ImageName).HasMaxLength(150);
            entity.Property(e => e.Subject).HasMaxLength(250);
            entity.Property(e => e.Text).HasMaxLength(400);
        });

        modelBuilder.Entity<IndexContent>(entity =>
        {
            entity.ToTable("IndexContent");

            entity.Property(e => e.IndexContentId).HasColumnName("IndexContentID");
        });

        modelBuilder.Entity<Introduce>(entity =>
        {
            entity.Property(e => e.IntroduceId).HasColumnName("IntroduceID");
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");

            entity.HasOne(d => d.Feature).WithMany(p => p.Introduces)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Introduces_Features");
        });

        modelBuilder.Entity<IntroduceSlider>(entity =>
        {
            entity.Property(e => e.IntroduceSliderId).HasColumnName("IntroduceSliderID");
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Feature).WithMany(p => p.IntroduceSliders)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IntroduceSliders_Features");
        });

        modelBuilder.Entity<NewsComment>(entity =>
        {
            entity.ToTable("NewsComment");

            entity.Property(e => e.NewsCommentId).HasColumnName("NewsCommentID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(350);
            entity.Property(e => e.Fullname).HasMaxLength(350);
            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Title).HasMaxLength(350);

            entity.HasOne(d => d.News).WithMany(p => p.NewsComments)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NewsComment_NewsFeatures");
        });

        modelBuilder.Entity<NewsFeature>(entity =>
        {
            entity.HasKey(e => e.NewsId);

            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Source).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(450);
        });

        modelBuilder.Entity<NewsTag>(entity =>
        {
            entity.HasKey(e => e.FntId);

            entity.ToTable("NewsTag");

            entity.Property(e => e.FntId).HasColumnName("FNT_ID");
            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.Tag).HasMaxLength(50);

            entity.HasOne(d => d.News).WithMany(p => p.NewsTags)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NewsTag_NewsFeatures");
        });

        modelBuilder.Entity<OptionFeature>(entity =>
        {
            entity.HasKey(e => e.OptionId);

            entity.Property(e => e.OptionId).HasColumnName("OptionID");
            entity.Property(e => e.Description).HasMaxLength(350);
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Feature).WithMany(p => p.OptionFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OptionFeatures_Features");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SelectedAdminAccess>(entity =>
        {
            entity.HasKey(e => e.SaaId);

            entity.Property(e => e.SaaId).HasColumnName("SAA_ID");
            entity.Property(e => e.AccessId).HasColumnName("AccessID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");

            entity.HasOne(d => d.Access).WithMany(p => p.SelectedAdminAccesses)
                .HasForeignKey(d => d.AccessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedAdminAccesses_Acesses");

            entity.HasOne(d => d.Admin).WithMany(p => p.SelectedAdminAccesses)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedAdminAccesses_Admins");
        });

        modelBuilder.Entity<SelectedBlogGroup>(entity =>
        {
            entity.HasKey(e => e.SbgId);

            entity.Property(e => e.SbgId).HasColumnName("SBG_ID");
            entity.Property(e => e.BlogGroupId).HasColumnName("BlogGroupID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");

            entity.HasOne(d => d.BlogGroup).WithMany(p => p.SelectedBlogGroups)
                .HasForeignKey(d => d.BlogGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedBlogGroups_BlogGroups");

            entity.HasOne(d => d.Blog).WithMany(p => p.SelectedBlogGroups)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedBlogGroups_Blogs");
        });

        modelBuilder.Entity<SelectedFaqGroup>(entity =>
        {
            entity.HasKey(e => e.SfgId);

            entity.Property(e => e.SfgId).HasColumnName("SFG_ID");
            entity.Property(e => e.FaqGroupId).HasColumnName("FaqGroupID");
            entity.Property(e => e.FaqId).HasColumnName("FaqID");

            entity.HasOne(d => d.FaqGroup).WithMany(p => p.SelectedFaqGroups)
                .HasForeignKey(d => d.FaqGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedFaqGroups_FaqGroups");

            entity.HasOne(d => d.Faq).WithMany(p => p.SelectedFaqGroups)
                .HasForeignKey(d => d.FaqId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedFaqGroups_Faqs");
        });

        modelBuilder.Entity<SelectedFeatureNews>(entity =>
        {
            entity.HasKey(e => e.SfnId);

            entity.Property(e => e.SfnId).HasColumnName("SFN_ID");
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.NewsId).HasColumnName("NewsID");

            entity.HasOne(d => d.Feature).WithMany(p => p.SelectedFeatureNews)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedFeatureNews_Features");

            entity.HasOne(d => d.News).WithMany(p => p.SelectedFeatureNews)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedFeatureNews_NewsFeatures");
        });

        modelBuilder.Entity<SelectedRelatedBlog>(entity =>
        {
            entity.HasKey(e => e.SrbId);

            entity.Property(e => e.SrbId).HasColumnName("SRB_ID");
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.MainBlogId).HasColumnName("MainBlogID");
            entity.Property(e => e.RelatedBlogId).HasColumnName("RelatedBlogID");

            entity.HasOne(d => d.MainBlog).WithMany(p => p.SelectedRelatedBlogMainBlogs)
                .HasForeignKey(d => d.MainBlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedRelatedBlogs_Blogs");

            entity.HasOne(d => d.RelatedBlog).WithMany(p => p.SelectedRelatedBlogRelatedBlogs)
                .HasForeignKey(d => d.RelatedBlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SelectedRelatedBlogs_Blogs1");
        });

        modelBuilder.Entity<SiteSetting>(entity =>
        {
            entity.Property(e => e.SiteSettingId).HasColumnName("SiteSettingID");
            entity.Property(e => e.BlogBannerImage).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(350);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.SiteLink).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<SiteTag>(entity =>
        {
            entity.Property(e => e.SiteTagId).HasColumnName("SiteTagID");
            entity.Property(e => e.Tag).HasMaxLength(150);
        });

        modelBuilder.Entity<SiteVisit>(entity =>
        {
            entity.ToTable("SiteVisit");

            entity.Property(e => e.SiteVisitId).HasColumnName("SiteVisitID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Slider>(entity =>
        {
            entity.Property(e => e.SliderId).HasColumnName("SliderID");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.SliderGroupId).HasColumnName("SliderGroupID");
            entity.Property(e => e.Title).HasMaxLength(390);

            entity.HasOne(d => d.SliderGroup).WithMany(p => p.Sliders)
                .HasForeignKey(d => d.SliderGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sliders_SliderGroups");
        });

        modelBuilder.Entity<SliderGroup>(entity =>
        {
            entity.Property(e => e.SliderGroupId).HasColumnName("SliderGroupID");
            entity.Property(e => e.GroupName).HasMaxLength(250);
        });

        modelBuilder.Entity<SocialMedium>(entity =>
        {
            entity.HasKey(e => e.SocialMediaId);

            entity.Property(e => e.SocialMediaId).HasColumnName("SocialMediaID");
            entity.Property(e => e.ClassName).HasMaxLength(50);
            entity.Property(e => e.ColorCode).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<BlogTranslation>(entity =>
        {
            entity.Property(e => e.BlogTranslationId).HasColumnName("BlogTranslationID");
            entity.Property(e => e.Language).HasMaxLength(5);
            entity.HasOne(d => d.Blog).WithMany(p => p.BlogTranslations)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FaqTranslation>(entity =>
        {
            entity.Property(e => e.FaqTranslationId).HasColumnName("FaqTranslationID");
            entity.Property(e => e.Language).HasMaxLength(5);
            entity.HasOne(d => d.Faq).WithMany(p => p.FaqTranslations)
                .HasForeignKey(d => d.FaqId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
