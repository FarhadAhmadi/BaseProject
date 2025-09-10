using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Shared.Constants
{
    public static partial class CacheKey
    {
        #region ACL

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string ACLRECORD_PATTERN_KEY => "Grand.aclrecord.";

        #endregion

        #region Permission

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer role ID
        /// {1} : permission system name
        /// </remarks>
        public static string PERMISSIONS_ALLOWED_KEY => "Grand.permission.allowed-{0}-{1}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer role ID
        /// {1} : permission system name
        /// {2} : permission action name
        /// </remarks>
        public static string PERMISSIONS_ALLOWED_ACTION_KEY => "Grand.permission.allowed.action-{0}-{1}-{2}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string PERMISSIONS_PATTERN_KEY => "Grand.permission.";

        #endregion
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string KNOWLEDGEBASE_CATEGORIES_PATTERN_KEY => "Knowledgebase.category.";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string ARTICLES_PATTERN_KEY => "Knowledgebase.article.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : category ID
        /// {1} : customer roles
        /// {2} : store id
        /// </remarks>
        public static string KNOWLEDGEBASE_CATEGORY_BY_ID => "Knowledgebase.category.id-{0}-{1}-{2}";

        /// <summary>
        /// Key for caching
        /// {0} : customer roles
        /// {1} : store id
        /// </summary>
        public static string KNOWLEDGEBASE_CATEGORIES => "Knowledgebase.category.all-{0}-{1}";

        /// <summary>
        /// Key for caching
        /// {0} : customer roles
        /// {1} : store id
        /// </summary>
        public static string ARTICLES => "Knowledgebase.article.all-{0}-{1}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : article ID
        /// {1} : customer roles
        /// {2} : store id
        /// </remarks>
        public static string ARTICLE_BY_ID => "Knowledgebase.article.id-{0}-{1}-{2}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : category ID
        /// {1} : customer roles
        /// {2} : store id
        /// </remarks>
        public static string ARTICLES_BY_CATEGORY_ID => "Knowledgebase.article.categoryid-{0}-{1}-{2}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : keyword
        /// {1} : customer roles
        /// {2} : store id
        /// </remarks>
        public static string ARTICLES_BY_KEYWORD => "Knowledgebase.article.keyword-{0}-{1}-{2}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : keyword
        /// {1} : customer roles
        /// {2} : store id
        /// </remarks>
        public static string KNOWLEDGEBASE_CATEGORIES_BY_KEYWORD => "Knowledgebase.category.keyword-{0}-{1}-{2}";

        /// <summary>
        /// Key for caching
        /// {0} : customer roles
        /// {1} : store id
        /// </summary>
        public static string HOMEPAGE_ARTICLES => "Knowledgebase.article.homepage-{0}-{1}";
        public static string CUSTOMER_ACTION_TYPE => "Grand.customer.action.type";

        /// <summary>
        /// Key for caching
        /// </summary>
        public static string CUSTOMERATTRIBUTES_ALL_KEY => "Grand.customerattribute.all";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer attribute ID
        /// </remarks>
        public static string CUSTOMERATTRIBUTES_BY_ID_KEY => "Grand.customerattribute.id-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string CUSTOMERATTRIBUTES_PATTERN_KEY => "Grand.customerattribute.";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string CUSTOMERATTRIBUTEVALUES_PATTERN_KEY => "Grand.customerattributevalue.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : ident
        /// </remarks>
        public static string CUSTOMERROLES_BY_KEY => "Grand.customerrole.key-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static string CUSTOMERROLES_BY_SYSTEMNAME_KEY => "Grand.customerrole.systemname-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string CUSTOMERROLES_PATTERN_KEY => "Grand.customerrole.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer role Id?
        /// </remarks>
        public static string CUSTOMERROLESPRODUCTS_ROLE_KEY => "Grand.customerroleproducts.role-{0}";

        #region Customer activity

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : ident
        /// </remarks>
        public static string ACTIVITYTYPE_BY_KEY => "Grand.activitytype.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        public static string ACTIVITYTYPE_ALL_KEY => "Grand.activitytype.all";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string ACTIVITYTYPE_PATTERN_KEY => "Grand.activitytype.";

        #endregion

        #region Sales person

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : salesemployee ID
        /// </remarks>
        public static string SALESEMPLOYEE_BY_ID_KEY => "Grand.salesemployee.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        public static string SALESEMPLOYEE_ALL => "Grand.salesemployee.all";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string SALESEMPLOYEE_PATTERN_KEY => "Grand.salesemployee.";

        #endregion
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string PRODUCTS_PATTERN_KEY => "Grand.product.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string PRODUCTS_BY_ID_KEY => "Grand.product.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer ID
        /// {1} : store ID
        /// </remarks>
        public static string PRODUCTS_CUSTOMER_ROLE => "Grand.product.cr-{0}-{1}";

        /// <summary>
        /// Key for caching
        /// </summary>
        public static string PRODUCTS_CUSTOMER_ROLE_PATTERN => "Grand.product.cr";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer ID
        /// </remarks>
        public static string PRODUCTS_CUSTOMER_TAG => "Grand.product.ct-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        public static string PRODUCTS_CUSTOMER_TAG_PATTERN => "Grand.product.ct";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer tag Id?
        /// </remarks>
        public static string CUSTOMERTAGPRODUCTS_ROLE_KEY => "Grand.customertagproducts.tag-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// {0} customer id
        /// </summary>
        public static string PRODUCTS_CUSTOMER_PERSONAL_KEY => "Grand.product.personal-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        public static string PRODUCTS_CUSTOMER_PERSONAL_PATTERN => "Grand.product.personal";

        /// <summary>
        /// Key for cache 
        /// {0} - customer id
        /// {1} - product id
        /// </summary>
        public static string CUSTOMER_PRODUCT_PRICE_KEY_ID => "Grand.product.price-{0}-{1}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string PRODUCTS_SHOWONHOMEPAGE => "Grand.product.showonhomepage";

        /// <summary>
        /// Compare products cookie name
        /// </summary>
        public static string PRODUCTS_COMPARE_COOKIE_NAME => "Grand.CompareProducts";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string PRODUCTTAG_COUNT_KEY => "Grand.producttag.count-{0}";

        /// <summary>
        /// Key for all tags
        /// </summary>
        public static string PRODUCTTAG_ALL_KEY => "Grand.producttag.all";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string PRODUCTTAG_PATTERN_KEY => "Grand.producttag.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer id
        /// {1} : number
        /// </remarks>
        public static string RECENTLY_VIEW_PRODUCTS_KEY => "Grand.recentlyviewedproducts-{0}-{1}";

        /// <summary>
        /// Key pattern to clear cache
        /// {0} customer id
        /// </summary>
        public static string RECENTLY_VIEW_PRODUCTS_PATTERN_KEY => "Grand.recentlyviewedproducts-{0}";

        #region Currency

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : currency ID
        /// </remarks>
        public static string CURRENCIES_BY_ID_KEY => "Grand.currency.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : currency code
        /// </remarks>
        public static string CURRENCIES_BY_CODE => "Grand.currency.code-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static string CURRENCIES_ALL_KEY => "Grand.currency.all-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string CURRENCIES_PATTERN_KEY => "Grand.currency.";

        #endregion

        #region Measure

        /// <summary>
        /// Key for caching
        /// </summary>
        public static string MEASUREDIMENSIONS_ALL_KEY => "Grand.measuredimension.all";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : dimension ID
        /// </remarks>
        public static string MEASUREDIMENSIONS_BY_ID_KEY => "Grand.measuredimension.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        public static string MEASUREWEIGHTS_ALL_KEY => "Grand.measureweight.all";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : weight ID
        /// </remarks>
        public static string MEASUREWEIGHTS_BY_ID_KEY => "Grand.measureweight.id-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string MEASUREDIMENSIONS_PATTERN_KEY => "Grand.measuredimension.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string MEASUREWEIGHTS_PATTERN_KEY => "Grand.measureweight.";

        /// <summary>
        /// Key for caching
        /// </summary>
        public static string MEASUREUNITS_ALL_KEY => "Grand.measureunit.all";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : dimension ID
        /// </remarks>
        public static string MEASUREUNITS_BY_ID_KEY => "Grand.measureunit.id-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string MEASUREUNITS_PATTERN_KEY => "Grand.measureunit.";

        #endregion

        #region Country
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// </remarks>
        public static string COUNTRIES_ALL_KEY => "Grand.country.all-{0}-{1}";

        /// <summary>
        /// key for caching by country id
        /// </summary>
        /// <remarks>
        /// {0} : country ID
        /// </remarks>
        public static string COUNTRIES_BY_KEY => "Grand.country.id-{0}";

        /// <summary>
        /// key for caching by country id
        /// </summary>
        /// <remarks>
        /// {0} : twoletter
        /// </remarks>
        public static string COUNTRIES_BY_TWOLETTER => "Grand.country.twoletter-{0}";

        /// <summary>
        /// key for caching by country id
        /// </summary>
        /// <remarks>
        /// {0} : threeletter
        /// </remarks>
        public static string COUNTRIES_BY_THREELETTER => "Grand.country.threeletter-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string COUNTRIES_PATTERN_KEY => "Grand.country.";

        #endregion

        #region StateProvinces

        /// {0} : state ID
        /// </remarks>
        public static string STATEPROVINCES_BY_KEY => "Grand.stateprovince.{0}";

        /// {0} : country ID
        /// {1} : language ID
        /// {2} : show hidden records?
        /// </remarks>
        public static string STATEPROVINCES_ALL_KEY => "Grand.stateprovince.all-{0}-{1}-{2}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        public static string STATEPROVINCES_PATTERN_KEY => "Grand.stateprovince.";

        #endregion
    }
}
