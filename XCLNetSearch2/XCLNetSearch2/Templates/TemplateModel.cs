using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCLNetSearch.Templates
{
    /// <summary>
    /// 模板标记实体
    /// </summary>
    internal class TemplateModel
    {
        public string 提示信息 { get; set; }

        public string 动态内容区JS插件名称 { get; set; }

        public string 容器ID { get; set; }
        public string 查询项所在容器ID { get; set; }

        public string 展开搜索框的图片按钮ID { get; set; }
        public string 展开搜索框的图片URL { get; set; }
        public string 隐藏搜索框的图片URL { get; set; }


        public string 存放最终解析结果的控件Name { get; set; }
        
        public string 左括号选择框的Name { get; set; }
        public string 左括号选择框的Options { get; set; }
        public string 右括号选择框的Name { get; set; }
        public string 右括号选择框的Options { get; set; }

        public string 字段选择框的Name { get; set; }
        public string 字段选择框的Options { get; set; }

        public string 符号选择框的Name { get; set; }
        public string 符号选择框的Options { get; set; }

        public string 输入区域控件的Name { get; set; }
        public string 输入区域控件的父标签Class { get { return string.Format("{0}Span", this.输入区域控件的Name); } }

        public string 逻辑选择框的Name { get; set; }
        public string 逻辑选择框的Options { get; set; }

        public string 动态增删行的行Class { get; set; }
        public string 动态增删行的模板行Class { get; set; }

        public string 增加搜索条件按钮的图片URL { get; set; }
        public string 增加搜索条件按钮的Class { get; set; }
        public string 删除搜索条件按钮的图片URL { get; set; }
        public string 删除搜索条件按钮的Class { get; set; }

        public string 查询提交按钮的ID { get; set; }
        public string 查询提交按钮的图片URL { get; set; }
        public string 查询提交按钮鼠标划过的图片URL { get; set; }

        public int 动态增删行的行的最大行数 { get; set; }

        public string 输入区域控件类型Json字符串 {
            get {
                StringBuilder str = new StringBuilder();
                List<EnumObj> lst=EnumObj.GetList(typeof(EnumTypes.InputControlTypeEnum));
                if (null != lst && lst.Count > 0)
                {
                    foreach (var m in lst)
                    {
                        str.AppendFormat(@"""{0}"":""{1}"",",m.Text,m.Value);
                    }
                }
                return string.Format(@"{{{0}}}", str.ToString().TrimEnd(','));
            }
        }

        public string 查询处理后的URL值的Json { get; set; }
    }
}
