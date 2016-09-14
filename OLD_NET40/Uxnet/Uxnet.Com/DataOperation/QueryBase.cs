using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI.WebControls;

namespace Uxnet.Com.DataOperation
{
    public delegate void QueryDelegate();
    public delegate void DataToSignDelegate();
    public delegate IEnumerable<T> OrderBy<T>(IEnumerable<T> dataSource);

    /// <summary>
    /// BusinessManager 的摘要描述。
    /// </summary>
    public class QueryBase
    {
        public QueryBase()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }
    }

    public interface IQuery
    {
        QueryDelegate DoQuery
        {
            get;
            set;
        }

    }

    public interface IQueryHelper
    {
        object DoQuery();
    }

    public interface IInquire<T> : IQuery
    {

        Expression<Func<T, bool>> QueryCondition
        {
            get;
        }

        void BuildQuery();

    }

    public interface IInquiryControl<T>
    {
        Expression<Func<T, bool>> QueryCondition
        {
            get;
        }

        void BuildQuery();
    }

    public interface IInquiryOrderByControl<T> : IInquiryControl<T>
    {
        IEnumerable<T> OrderBy(IEnumerable<T> dataSource, Dictionary<String, SortDirection> sortExpression);
    }


    public partial class UserInquiryControl<T> : System.Web.UI.UserControl, IInquiryControl<T>
    {
        protected internal Expression<Func<T, bool>> _queryCond;

        #region IInquiryControl<T> 成員

        public Expression<Func<T, bool>> QueryCondition
        {
            get { return _queryCond; }
        }

        public virtual void BuildQuery()
        {

        }

        #endregion
    }

    public static partial class ExtensionMethods
    {
        public static SortDirection DetermineOrderBy(this LinkButton button)
        {
            if (button.Text.EndsWith("↓"))
            {
                String s = button.Text;
                button.Text = s.Substring(0, s.Length - 1) + "↑";
                return SortDirection.Ascending;
            }
            else if (button.Text.EndsWith("↑"))
            {
                String s = button.Text;
                button.Text = s.Substring(0, s.Length - 1) + "↓";
                return SortDirection.Descending;
            }
            else
            {
                String s = button.Text;
                button.Text = s + "↑";
                return SortDirection.Ascending;
            }

        }
    }
}
