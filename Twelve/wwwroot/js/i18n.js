const translations = {
  en: {
    home: "Home",
    privacy: "Privacy",
    blog: "Blog & News",
    faq: "FAQ",
    menu: "Menu Intro",
    about: "About Us",
    contact: "Contact Us",
    download: "Download App",
    directDownload: "Direct download link",
    downloadDesc: "You can now download the Twelve app via Bazaar, Myket, or a direct link",
    enterPhone: "Enter your mobile number",
    downloadVersion: "Download version",
    followUs: "Follow us on social media",
    sendLink: "Send link",
    downloadDirectApp: "Direct app download",
    stayUpdated: "Stay with us for the latest events",
    latestArticlesNews: "Latest articles and news",
    appNews: "App news",
    appMenuTutorial: "App menu tutorials",
    viewAllPosts: "View all posts",
    readPost: "Read post",
    allPosts: "All posts"
  },
  fa: {
    home: "صفحه اصلی",
    privacy: "حریم خصوصی",
    blog: "بلاگ و اخبار",
    faq: "سوالات متداول",
    menu: "معرفی منو ها",
    about: "درباره ما",
    contact: "ارتباط با ما",
    download: "دانلود اپلیکیشن",
    directDownload: "دانلود با لینک مستقیم",
    downloadDesc: "همین حالا میتونید، اپلیکیشن ((دوازده)) رو از طریق بازار، مایکت و یا با لینک مستقیم دریافت کنید",
    enterPhone: "شماره تلفن همراه خود را وارد کنید",
    downloadVersion: "دانلود نسخه",
    followUs: "ما را در شبکه های اجتماعی دنبال کنید",
    sendLink: "ارسال لینک",
    downloadDirectApp: "دانلود مستقیم اپلیکیشن",
    stayUpdated: "برای اطلاع از جدیدترین رویدادها با ما همراه باشید",
    latestArticlesNews: "آخرین مقالات و اخبار",
    appNews: "اخبار اپلیکیشن",
    appMenuTutorial: "آموزش منو های اپلیکیشن",
    viewAllPosts: "مشاهده تمام پست ها",
    readPost: "مطالعه پست",
    allPosts: "همه پست ها"
  },
  ar: {
    home: "الرئيسية",
    privacy: "الخصوصية",
    blog: "المدونة والأخبار",
    faq: "الأسئلة المتكررة",
    menu: "تعريف القوائم",
    about: "من نحن",
    contact: "اتصل بنا",
    download: "تحميل التطبيق",
    directDownload: "رابط التحميل المباشر",
    downloadDesc: "يمكنك الآن تنزيل تطبيق تويلف عبر بازار أو مايكت أو من الرابط المباشر",
    enterPhone: "أدخل رقم هاتفك المحمول",
    downloadVersion: "تحميل نسخة",
    followUs: "تابعونا على شبكات التواصل الاجتماعي",
    sendLink: "أرسل الرابط",
    downloadDirectApp: "تنزيل التطبيق مباشرة",
    stayUpdated: "ابق معنا للاطلاع على أحدث الأحداث",
    latestArticlesNews: "أحدث المقالات والأخبار",
    appNews: "أخبار التطبيق",
    appMenuTutorial: "شروحات قوائم التطبيق",
    viewAllPosts: "عرض جميع المنشورات",
    readPost: "قراءة المنشور",
    allPosts: "كل المنشورات"
  },
  ur: {
    home: "ہوم",
    privacy: "پرائیویسی",
    blog: "بلاگ اور خبریں",
    faq: "عمومی سوالات",
    menu: "مینوز کا تعارف",
    about: "ہمارے بارے میں",
    contact: "ہم سے رابطہ کریں",
    download: "ایپ ڈاؤنلوڈ کریں",
    directDownload: "براہ راست ڈاؤنلوڈ لنک",
    downloadDesc: "ابھی آپ بازاز، مائیکٹ یا براہ راست لنک کے ذریعے ٹوئیلَو ایپ ڈاؤن لوڈ کر سکتے ہیں",
    enterPhone: "اپنا موبائل نمبر درج کریں",
    downloadVersion: "ورژن ڈاؤن لوڈ کریں",
    followUs: "ہمیں سوشل میڈیا پر فالو کریں",
    sendLink: "لنک بھیجیں",
    downloadDirectApp: "براہ راست ایپ ڈاؤن لوڈ کریں",
    stayUpdated: "تازہ ترین واقعات سے باخبر رہنے کے لیے ہمارے ساتھ رہیں",
    latestArticlesNews: "تازہ ترین مضامین اور خبریں",
    appNews: "ایپ کی خبریں",
    appMenuTutorial: "ایپ مینو کی رہنمائی",
    viewAllPosts: "تمام پوسٹس دیکھیں",
    readPost: "پوسٹ پڑھیں",
    allPosts: "تمام پوسٹس"
  }
};

function getCurrentLang() {
  const match = document.cookie.match(/\.AspNetCore\.Culture=c=([^|]+)\|uic=([^;]+)/);
  return match ? match[1] : 'fa';
}

function applyTranslations(lang) {
  document.querySelectorAll('[data-i18n-key]').forEach(el => {
    const key = el.getAttribute('data-i18n-key');
    const translation = translations[lang] && translations[lang][key];
    if (translation) {
      el.textContent = translation;
    }
  });
  document.documentElement.setAttribute('lang', lang);
  const rtlLangs = ['fa', 'ar', 'ur'];
  document.documentElement.setAttribute('dir', rtlLangs.includes(lang) ? 'rtl' : 'ltr');
}

function setLanguage(lang) {
  document.cookie = `.AspNetCore.Culture=c=${lang}|uic=${lang}; path=/`;
  applyTranslations(lang);
}

document.addEventListener('DOMContentLoaded', () => {
  const lang = getCurrentLang();
  document.querySelectorAll('.language-selector').forEach(selector => {
    selector.value = lang;
    selector.addEventListener('change', (e) => {
      setLanguage(e.target.value);
      location.reload();
    });
  });
  applyTranslations(lang);
});
