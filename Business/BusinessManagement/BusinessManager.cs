using System;
using System.Collections;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Business.BusinessManagement
{
    public delegate void QueryDelegate();
    public delegate void DataToSignDelegate();
    public delegate IEnumerable<T> OrderBy<T>(IEnumerable<T> dataSource);

    /// <summary>
    /// BusinessManager 的摘要描述。
    /// </summary>
    public class BusinessManager
    {
        public BusinessManager()
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

        Expression<Func<T,bool>> QueryCondition
        {
            get;
        }

        void BuildQuery();

    }

    public interface IInquire2<T> : IInquire<T>
    {

        Dictionary<String, Object> QueryDisplayValue
        {
            get;
        }

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
        IEnumerable<T> OrderBy(IEnumerable<T> dataSource,Dictionary<String,OrderByType> sortExpression);
    }

    public enum OrderByType
    {
        Ascending,
        Descending
    }

    public static partial class ExtensionMethods
    {
        public static OrderByType DetermineOrderBy(this LinkButton button)
        {
            if (button.Text.EndsWith("↓"))
            {
                String s = button.Text;
                button.Text = s.Substring(0, s.Length - 1) + "↑";
                return OrderByType.Ascending;
            }
            else if (button.Text.EndsWith("↑"))
            {
                String s = button.Text;
                button.Text = s.Substring(0, s.Length - 1) + "↓";
                return OrderByType.Descending;
            }
            else
            {
                String s = button.Text;
                button.Text = s + "↑";
                return OrderByType.Ascending;
            }

        }
    }
}
